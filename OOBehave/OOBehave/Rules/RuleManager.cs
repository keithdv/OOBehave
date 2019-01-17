using OOBehave.Attributes;
using OOBehave.Core;
using OOBehave.Rules.Rules;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OOBehave.Rules
{


    public interface IRuleManager
    {

        bool IsValid { get; }
        bool IsBusy { get; }
        Task WaitForRules { get; }
        IEnumerable<IRule> Rules { get; }
        IRuleResult OverrideResult { get; }

        /// <summary>
        /// Set OverrideResult and permenantly mark as invalid
        /// </summary>
        /// <param name="message"></param>
        void MarkInvalid(string message);

        void AddRule(IRule rule);
        void AddRules(params IRule[] rules);
        IEnumerable<string> this[string propertyName] { get; }
        IRuleResultReadOnlyList Results { get; }
        Task CheckRulesForProperty(string propertyName);
        Task CheckAllRules(CancellationToken token = new CancellationToken());
        event PropertyChangedEventHandler PropertyChanged;

    }

    public interface IRuleManager<T> : IRuleManager
    {
        FluentRule<T> AddRule(Func<T, IRuleResult> func, params string[] triggerProperty);

    }

    [PortalDataContract]
    public class RuleManager<T> : IRuleManager<T>, ISetTarget
    {

        protected T Target { get; set; }

        [PortalDataMember]
        protected IRuleResultList Results { get; private set; }

        IRuleResultReadOnlyList IRuleManager.Results => Results.RuleResultList;

        protected bool TransferredResults = false;
        public bool IsBusy => isRunningRules;
        public event PropertyChangedEventHandler PropertyChanged;

        public RuleManager(IRuleResultList results, IAttributeToRule attributeToRule, IRegisteredPropertyManager<T> registeredPropertyManager)
        {
            Results = results;
            AddAttributeRules(attributeToRule, registeredPropertyManager);
        }

        IEnumerable<IRule> IRuleManager.Rules => Rules.Values;

        private IDictionary<uint, IRule> Rules { get; } = new ConcurrentDictionary<uint, IRule>();

        IEnumerable<string> IRuleManager.this[string propertyName]
        {
            get { return Results[propertyName]; }
        }

        private ConcurrentQueue<uint> ruleQueue = new ConcurrentQueue<uint>();

        protected virtual void AddAttributeRules(IAttributeToRule attributeToRule, IRegisteredPropertyManager<T> registeredPropertyManager)
        {
            var requiredRegisteredProp = registeredPropertyManager.GetRegisteredProperties();

            foreach (var r in requiredRegisteredProp)
            {
                foreach (var a in r.PropertyInfo.GetCustomAttributes(true))
                {
                    var rule = attributeToRule.GetRule(r, a.GetType());
                    if (rule != null) { AddRule(rule); }
                }
            }
        }

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

        public FluentRule<T> AddRule(Func<T, IRuleResult> func, params string[] triggerProperty)
        {
            FluentRule<T> rule = new FluentRule<T>(func, triggerProperty); // TODO - DI
            Rules.Add(rule.UniqueIndex, rule);
            return rule;
        }

        public Task CheckRulesForProperty(string propertyName)
        {
            if (OverrideResult == null)
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

                return WaitForRules;
            }
            else
            {
                return Task.CompletedTask;
            }
        }

        public Task CheckAllRules(CancellationToken token = new CancellationToken())
        {
            if (OverrideResult == null)
            {
                Results.Clear(); // Cover in case something unexpected has happened like a weird Serialization cover or maybe a Rule that exists on the client or not the server

                foreach (var ruleIndex in Rules.Keys)
                {
                    ruleQueue.Enqueue(ruleIndex);
                }

                CheckRulesQueue();

                return WaitForRules;
            }
            else
            {
                return Task.CompletedTask;
            }
        }

        public bool IsValid
        {
            get { return !Results.Values.Where(r => r.IsError).Any(); }
        }

        [PortalDataMember]
        public IRuleResult OverrideResult
        {
            get { return Results.OverrideResult; }
            set { Results.OverrideResult = value; }
        }

        public void MarkInvalid(string message)
        {
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
            }

            Results.OverrideResult = RuleResult.PropertyError(typeof(T).FullName, message);
        }

        public Task WaitForRules { get; private set; } = Task.CompletedTask;

        private TaskCompletionSource<object> waitForRulesSource;
        private CancellationTokenSource cancellationTokenSource;
        private ConcurrentQueue<string> propertyHasChanged;

        private bool isRunningRules = false;

        public void CheckRulesQueue(bool isRecursiveCall = false)
        {

            if (Target == null) { throw new TargetIsNullException(); }

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
                    propertyHasChanged = new ConcurrentQueue<string>();
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

                    // What if this calls other rules??
                    var phc = propertyHasChanged;
                    foreach (var p in phc)
                    {
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
                    }

                    waitForRulesSource.SetResult(new object());


                }
            }

            if (OverrideResult == null && !isRunningRules || isRecursiveCall)
            {
                Start();
                var token = cancellationTokenSource.Token; // Local stack copy important

                while (ruleQueue.TryDequeue(out var ruleIndex))
                {

                    // System.Diagnostics.Debug.WriteLine($"Dequeue {propertyName}");

                    var ruleManagerTask = RunRule(Rules[ruleIndex], token);

                    if (!ruleManagerTask.IsCompleted)
                    {
                        // Really important
                        // If there there is not an asyncronous fork all of the async methods will run synchronously
                        // Which is great! Because we are likely within a property change
                        // However, if there was an asyncronous fork we need to handle it's completion
                        // In WPF there's an executable so this will continue "hands off"
                        // In request response the WaitForRules needs to be awaited!
                        ruleManagerTask.ContinueWith(x =>
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
                    result = await rule.Execute(Target, token).ConfigureAwait(false);
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

            if (Results.ContainsKey((int)r.UniqueIndex))
            {
                var existingResult = Results[(int)r.UniqueIndex];
                foreach (var pr in existingResult.PropertyErrorMessages)
                {
                    if (!propertyHasChanged.Contains(pr.Key))
                    {
                        propertyHasChanged.Enqueue(pr.Key);
                    }
                }
                Results.Remove((int)r.UniqueIndex);
            }

            if (result.IsError)
            {
                result.TriggerProperties = r.TriggerProperties;
                Results[(int)r.UniqueIndex] = result;

                foreach (var pr in result.PropertyErrorMessages)
                {
                    if (!propertyHasChanged.Contains(pr.Key))
                    {
                        propertyHasChanged.Enqueue(pr.Key);
                    }
                }
            }

        }

        void ISetTarget.SetTarget(IBase target)
        {
            if (target is T tt)
            {
                Target = tt;
            }
            else
            {
                throw new InvalidTargetTypeException(target.GetType().FullName);
            }
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            if (Results.Any())
            {
                Results.SetKeysToNegative();
                TransferredResults = true;
            }
        }

        private void SetSerializedResults(IRuleResultList transfferedResults, IRuleResult overrideResult)
        {
            if (transfferedResults.Any())
            {
                if (overrideResult == null)
                {
                    Results = transfferedResults;
                    Results.SetKeysToNegative();
                    TransferredResults = true;
                }
                else
                {
                    OverrideResult = overrideResult;
                }
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


    [Serializable]
    public class InvalidTargetTypeException : Exception
    {
        public InvalidTargetTypeException() { }
        public InvalidTargetTypeException(string message) : base(message) { }
        public InvalidTargetTypeException(string message, Exception inner) : base(message, inner) { }
        protected InvalidTargetTypeException(
          SerializationInfo info,
          StreamingContext context) : base(info, context) { }
    }


    [Serializable]
    public class TargetIsNullException : Exception
    {
        public TargetIsNullException() { }
        public TargetIsNullException(string message) : base(message) { }
        public TargetIsNullException(string message, Exception inner) : base(message, inner) { }
        protected TargetIsNullException(
          SerializationInfo info,
          StreamingContext context) : base(info, context) { }
    }
}

