using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OOBehave.Rules
{

    public interface IRuleExecute
    {
        IEnumerable<IRule> Rules { get; }

        IEnumerable<IRuleResult> Results { get; }

        void CheckRulesForProperty(string propertyName);

        Task WaitForRules { get; }

        bool IsValid { get; }

        bool IsBusy { get; }
        void AddRule(IRule rule);
        void AddRules(params IRule[] rules);
        FluentRule<T> AddRule<T>(string triggerProperty, Func<T, IRuleResult> func);
        Task RunAllRules(CancellationToken token = new CancellationToken());

    }

    public interface IRuleExecute<T> : IRuleExecute
        where T : IValidateBase
    {

    }

    [DataContract]
    public class RuleExecute<T> : IRuleExecute<T>
        where T : IValidateBase
    {

        [DataMember]
        protected T Target { get; }
        [DataMember]
        protected IDictionary<int, IRuleResult> Results { get; private set; } = new ConcurrentDictionary<int, IRuleResult>();
        protected bool TransferredResults = false;
        public bool IsBusy => isRunningRules;
        public RuleExecute(T target)
        {
            this.Target = target;
        }

        IEnumerable<IRule> IRuleExecute.Rules => Rules.Values;

        private IDictionary<uint, IRule> Rules { get; } = new ConcurrentDictionary<uint, IRule>();

        IEnumerable<IRuleResult> IRuleExecute.Results => Results.Values;

        private ConcurrentQueue<uint> ruleQueue = new ConcurrentQueue<uint>();


        public void AddRules(params IRule[] rules)
        {
            foreach (var r in rules) { AddRule(r); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rule"></param>
        public void AddRule(IRule rule)
        {
            // TODO - Only allow Rule Types to be added - not instances
            Rules.Add(rule.UniqueIndex, rule ?? throw new ArgumentNullException(nameof(rule)));
        }

        public FluentRule<T2> AddRule<T2>(string triggerProperty, Func<T2, IRuleResult> func)
        {
            FluentRule<T2> rule = new FluentRule<T2>(func, triggerProperty); // TODO - DI
            Rules.Add(rule.UniqueIndex, rule);
            return rule;
        }

        public void CheckRulesForProperty(string propertyName)
        {

            if (TransferredResults)
            {
                var oldResults = Results.Where(x => x.Key < 0 && x.Value.TriggerProperties.Contains(propertyName)).ToList();
                oldResults.ForEach(r => Results.Remove(r.Key));

                if (!Results.Where(x => x.Key < 0).Any())
                {
                    TransferredResults = false;
                }
            }

            foreach (var index in Rules.Values.Where(r => r.TriggerProperties.Contains(propertyName)).Select(r => r.UniqueIndex))
            {
                if (!ruleQueue.Contains(index))
                {
                    // System.Diagnostics.Debug.WriteLine($"Enqueue {propertyName}");
                    ruleQueue.Enqueue(index);
                }
            }

            CheckRulesQueue();
        }

        public async Task RunAllRules(CancellationToken token = new CancellationToken())
        {
            Results.Clear(); // Cover in case something unexpected has happened like a weird Serialization cover or maybe a Rule that exists on the client or not the server

            foreach (var ruleIndex in Rules.Keys)
            {
                ruleQueue.Enqueue(ruleIndex);
            }

            CheckRulesQueue();

            await WaitForRules;

        }

        public bool IsValid
        {
            get { return !Results.Values.Where(r => r.IsError).Any(); }
        }

        public Task WaitForRules { get; private set; } = Task.CompletedTask;
        private TaskCompletionSource<object> waitForRulesSource;
        private CancellationTokenSource cancellationTokenSource;

        private bool isRunningRules = false;

        public void CheckRulesQueue(bool isRecursiveCall = false)
        {
            // DISCUSS : Runes the rules sequentially - even Async Rules
            // Make async rules changing properties a non-issue

            void Start()
            {
                if (!isRecursiveCall)
                {

#if DEBUG
                    if (!WaitForRules.IsCompleted) throw new Exception("Unexpected WaitForRules.IsCompleted is false");
#endif
                    isRunningRules = true;
                    cancellationTokenSource = new CancellationTokenSource();
                    waitForRulesSource = new TaskCompletionSource<object>();
                    WaitForRules = waitForRulesSource.Task;
                }
            }

            void Stop()
            {
                // We need to handle if properties changed by the user while rules were running

                if (ruleQueue.Any())
                {
                    CheckRulesQueue(true);
                }
                else
                {
#if DEBUG
                    if (WaitForRules.IsCompleted) throw new Exception("Unexpected WaitForRules.IsCompleted is false");
#endif

                    isRunningRules = false;
                    waitForRulesSource.SetResult(new object());
                }
            }

            if (!isRunningRules || isRecursiveCall)
            {
                Start();
                var token = cancellationTokenSource.Token; // Local stack copy important

                while (ruleQueue.TryDequeue(out var ruleIndex))
                {

                    // System.Diagnostics.Debug.WriteLine($"Dequeue {propertyName}");

                    var ruleExecuteTask = RunRule(Rules[ruleIndex], token);

                    if (!ruleExecuteTask.IsCompleted)
                    {
                        // Really important
                        // If there there is not an asyncronous fork all of the async methods will run synchronously
                        // Which is great! Because we are likely within a property change
                        // However, if there was an asyncronous fork we need to handle it's completion
                        // In WPF there's an executable so this will continue "hands off"
                        // In request response the WaitForRules needs to be awaited!
                        ruleExecuteTask.ContinueWith(x =>
                        {
                            CheckRulesQueue(true);
                        });

                        return; // Let the ContinueWith call CheckRulesQueue again
                    }
                }

                Stop();
            }
        }

        private async Task RunRule(IRule r, CancellationToken token)
        {
            IRuleResult result = null;

            try
            {
                if (r is IRule<T> rule)
                {
                    result = await rule.Execute(Target, token);
                }
                else if (r is IRule<IBase> sharedRule)
                {
                    result = await sharedRule.Execute(Target, token);
                }
                else
                {
                    throw new InvalidRuleTypeException($"{r.GetType().FullName} cannot be executed for {typeof(T).FullName}");
                }

                if (token.IsCancellationRequested)
                {
                    return;
                }

            }

            catch (Exception ex)
            {
                // If there is an error mark all properties as failed
                foreach (var p in r.TriggerProperties)
                {
                    result = RuleResult.PropertyError(p, ex.Message);
                }
            }

            if (result.IsError)
            {
                result.TriggerProperties = r.TriggerProperties;
                Results[(int)r.UniqueIndex] = result;
            }
            else if (Results.ContainsKey((int)r.UniqueIndex))
            {
                // Optimized approach for when/if this is serialized
                Results.Remove((int)r.UniqueIndex);
            }

        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            if (Results.Any())
            {
                Results = new ConcurrentDictionary<int, IRuleResult>(Results.Select(x => new KeyValuePair<int, IRuleResult>(x.Key * -1, x.Value)));
                TransferredResults = true;
            }
        }
    }


    [Serializable]
    public class TargetRulePropertyChangeException : Exception
    {
        public TargetRulePropertyChangeException() { }
        public TargetRulePropertyChangeException(string message) : base(message) { }
        public TargetRulePropertyChangeException(string message, Exception inner) : base(message, inner) { }
        protected TargetRulePropertyChangeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }


    [Serializable]
    public class InvalidRuleTypeException : Exception
    {
        public InvalidRuleTypeException() { }
        public InvalidRuleTypeException(string message) : base(message) { }
        public InvalidRuleTypeException(string message, Exception inner) : base(message, inner) { }
        protected InvalidRuleTypeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}

