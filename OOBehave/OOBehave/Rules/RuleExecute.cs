using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OOBehave.Rules
{

    public interface IRuleExecute<T>
    {
        IEnumerable<IRule> Rules { get; }

        IEnumerable<IRuleResult> Results { get; }

        void CheckRulesForProperty(string propertyName);

        Task WaitForRules { get; }

        bool IsValid { get; }

        bool IsBusy { get; }

        void AddRule(IRule rule);
        void AddRules(params IRule[] rules);
        FluentRule<T> AddRule(string triggerProperty, Func<T, IRuleResult> func);
    }

    public class RuleExecute<T> : IRuleExecute<T>
        where T : IBase
    {

        protected T Target { get; }
        protected IDictionary<uint, IRuleResult> Results { get; } = new ConcurrentDictionary<uint, IRuleResult>();
        public bool IsBusy => isRunningRules;
        public RuleExecute(T target)
        {
            this.Target = target;
        }

        IEnumerable<IRule> IRuleExecute<T>.Rules => Rules.AsReadOnly();

        private List<IRule> Rules { get; } = new List<IRule>();

        IEnumerable<IRuleResult> IRuleExecute<T>.Results => Results.Values;

        private ConcurrentQueue<string> propertyQueue = new ConcurrentQueue<string>();


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
            Rules.Add(rule ?? throw new ArgumentNullException(nameof(rule)));
        }

        public FluentRule<T> AddRule(string triggerProperty, Func<T, IRuleResult> func)
        {
            FluentRule<T> rule = new FluentRule<T>(func, triggerProperty); // TODO - DI
            Rules.Add(rule);
            return rule;
        }

        public void CheckRulesForProperty(string propertyName)
        {
            if (!propertyQueue.Contains(propertyName))
            {
                // System.Diagnostics.Debug.WriteLine($"Enqueue {propertyName}");
                propertyQueue.Enqueue(propertyName);
            }

            CheckRulesQueue();
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
                // Yes, this causes the infinite loop if they change a property in a TargetRule. Which they should not do.

                if (propertyQueue.Any())
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

                while (propertyQueue.TryDequeue(out var propertyName))
                {

                    // System.Diagnostics.Debug.WriteLine($"Dequeue {propertyName}");

                    var cascadeRuleTask = RunCascadeRulesRecursive(propertyName, token);

                    if (!cascadeRuleTask.IsCompleted)
                    {
                        // Really important
                        // If there there is not an asyncronous fork all of the async methods will run synchronously
                        // Which is great! Because we are likely within a property change
                        // However, if there was an asyncronous fork we need to handle it's completion
                        // In WPF there's an executable so this will continue "hands off"
                        // In request response the WaitForRules needs to be awaited!
                        cascadeRuleTask.ContinueWith(x =>
                        {
                            CheckRulesQueue(true);
                        });

                        return; // Let the ContinueWith call CheckRulesQueue again
                    }
                }

                var targetRuleTask = RunTargetRulesSequential(token);

                // Really important
                // If there there is not an asyncronous fork all of the async methods will run synchronously
                // Which is great! Because we are likely within a property change
                // However, if there was an asyncronous fork we need to handle it's completion
                // In WPF there's an executable so this will continue "hands off"
                // In request response the WaitForRules needs to be awaited!
                if (!targetRuleTask.IsCompleted)
                {
                    targetRuleTask.ContinueWith(x =>
                    {
                        Stop();
                    });

                }
                else
                {
                    Stop();
                }
            }
        }



        private async Task RunCascadeRulesRecursive(string propertyName, CancellationToken token)
        {
            foreach (var r in Rules.OfType<ICascadeRule>().Where(r => r.TriggerProperties.Contains(propertyName)).ToList())
            {
                IRuleResult result;

                try
                {
                    if (r is ICascadeRule<T> rule)
                    {
                        result = await rule.Execute(Target, token);
                    }
                    else if (r is ICascadeRule<IBase> sharedRule)
                    {
                        result = await sharedRule.Execute(Target, token);
                    }
                    else
                    {
                        throw new InvalidRuleTypeException($"{r.GetType().FullName} cannot be executed for {typeof(T).FullName}");
                    }

                }
                catch (Exception ex)
                {
                    result = RuleResult.TargetError(ex.Message);
                }

                if (token.IsCancellationRequested)
                {
                    break;
                }

                Results[r.UniqueIndex] = result;

            }
        }

        private async Task RunTargetRulesSequential(CancellationToken token)
        {
            foreach (var r in Rules.OfType<ITargetRule>().ToList())
            {
                IRuleResult result;

                try
                {
                    if (r is ITargetRule<T> rule)
                    {
                        result = await rule.Execute(Target, token);
                    }
                    else if (r is ITargetRule<IBase> baseRule)
                    {
                        result = await baseRule.Execute(Target, token);
                    }
                    else
                    {
                        throw new InvalidRuleTypeException($"{r.GetType().FullName} cannot be executed for {typeof(T).FullName}");
                    }

                }
                catch (Exception ex)
                {
                    result = RuleResult.TargetError(ex.Message);
                }

                if (token.IsCancellationRequested)
                {
                    break;
                }

                Results[r.UniqueIndex] = result;

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

