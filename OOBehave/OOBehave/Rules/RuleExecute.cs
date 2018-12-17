using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.Rules
{

    public interface IRuleExecute<T>
    {
        IReadOnlyCollection<IRule<T>> Rules { get; }

        IReadOnlyList<IRuleResult> Results { get; }

        void CheckRulesForProperty<P>(IRegisteredProperty<P> property);

        Task WaitForRules { get; }

        bool IsValid { get; }
    }

    public class RuleExecute<T> : IRuleExecute<T>
    {

        protected T Target { get; }
        protected IDictionary<uint, IRuleResult> Results { get; } = new ConcurrentDictionary<uint, IRuleResult>();

        public RuleExecute(T target, IReadOnlyCollection<IRule<T>> rules)
        {
            this.Rules = rules;
            this.Target = target;
        }

        public IReadOnlyCollection<IRule<T>> Rules { get; }

        IReadOnlyList<IRuleResult> IRuleExecute<T>.Results => Results.Values.ToList().AsReadOnly();

        private ConcurrentQueue<IRegisteredProperty> propertyQueue = new ConcurrentQueue<IRegisteredProperty>();

        public void CheckRulesForProperty<P>(IRegisteredProperty<P> property)
        {
            if (!propertyQueue.Contains(property))
            {
                System.Diagnostics.Debug.WriteLine($"Enqueue {property.Name}");
                propertyQueue.Enqueue(property);
            }

            CheckRulesQueue();
        }

        public bool IsValid
        {
            get { return !Results.Values.Where(r => r.IsError).Any(); }
        }

        public Task WaitForRules { get; private set; } = Task.CompletedTask;
        private TaskCompletionSource<object> waitForRulesSource;

        private bool isRunningRules = false;

        public void CheckRulesQueue()
        {
            // DISCUSS : Runes the rules sequentially - even Async Rules
            // Make async rules changing properties a non-issue

            void ResetWaitForRules()
            {
                if (WaitForRules.IsCompleted)
                {
                    waitForRulesSource = new TaskCompletionSource<object>();
                    WaitForRules = waitForRulesSource.Task;
                }
            }

            void CompleteWaitForRules()
            {
                if (propertyQueue.Any())
                {
                    CheckRulesQueue();
                }
                else if (!WaitForRules.IsCompleted)
                {
                    waitForRulesSource.SetResult(new object());
                }
            }

            if (!isRunningRules)
            {
                while (propertyQueue.TryDequeue(out var property))
                {
                    isRunningRules = true;

                    System.Diagnostics.Debug.WriteLine($"Dequeue {property.Name}");

                    var cascadeRuleTask = RunCascadeRulesRecursive(property);

                    if (!cascadeRuleTask.IsCompleted)
                    {
                        ResetWaitForRules();

                        cascadeRuleTask.ContinueWith(x =>
                        {
                            isRunningRules = false;
                            CheckRulesQueue();
                        });

                        return; // Let the ContinueWith call CheckRulesQueue again
                    }
                    else
                    {
                        isRunningRules = false;
                    }
                }

                isRunningRules = true;

                var targetRuleTask = RunTargetRulesSequential();

                if (!targetRuleTask.IsCompleted)
                {
                    ResetWaitForRules();

                    targetRuleTask.ContinueWith(x =>
                    {
                        isRunningRules = false;
                        CompleteWaitForRules();
                    });

                    return;
                }
                else
                {
                    isRunningRules = false;
                    CompleteWaitForRules();
                }
            }
        }

        private async Task RunCascadeRulesRecursive(IRegisteredProperty property)
        {
            foreach (var r in Rules.OfType<ICascadeRule<T>>().Where(r => r.TriggerProperties.Contains(property)).ToList())
            {
                // TODO - Wrap with a Try/Catch
                Results[r.UniqueIndex] = await r.Execute(Target);
                // Todo - Deal with result
            }
        }

        private async Task RunTargetRulesSequential()
        {
            foreach (var r in Rules.OfType<ITargetRule<T>>().ToList())
            {
                // TODO - Wrap with a Try/Catch
                Results[r.UniqueIndex] = await r.Execute(Target);
                // Todo - Deal with result
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
}
