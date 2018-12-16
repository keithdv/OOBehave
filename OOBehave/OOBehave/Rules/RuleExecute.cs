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
    }

    public class RuleExecute<T> : IRuleExecute<T>
    {

        protected T Target { get; }
        protected IDictionary<uint, IRuleResult> Results { get; }

        public RuleExecute(T target, IReadOnlyCollection<IRule<T>> rules)
        {
            this.Rules = rules;
            this.Results = new ConcurrentDictionary<uint, IRuleResult>();
            this.Target = target;
        }

        public IReadOnlyCollection<IRule<T>> Rules { get; }

        IReadOnlyList<IRuleResult> IRuleExecute<T>.Results => Results.Values.ToList().AsReadOnly();

        private ConcurrentQueue<IRegisteredProperty> propertyQueue = new ConcurrentQueue<IRegisteredProperty>();

        public void CheckRulesForProperty<P>(IRegisteredProperty<P> property)
        {
            System.Diagnostics.Debug.WriteLine($"Enqueue {property.Name}");
            propertyQueue.Enqueue(property);
            CheckRulesQueue();
        }

        public Task WaitForRules { get; private set; } = Task.CompletedTask;
        public TaskCompletionSource<object> waitForRulesSource;

        public void CheckRulesQueue()
        {
            if (propertyQueue.TryDequeue(out var property))
            {
                if (WaitForRules.IsCompleted)
                {
                    waitForRulesSource = new TaskCompletionSource<object>();
                    WaitForRules = waitForRulesSource.Task;
                }

                System.Diagnostics.Debug.WriteLine($"Dequeue {property.Name}");

                var runningRuleTask = RunCascadeRulesRecursive(property)
                    .ContinueWith(x =>
                    {
                        System.Diagnostics.Debug.WriteLine($"ContinueWith {property.Name}");

                        // Only matters if there is an async fork withing the rules
                        CheckRulesQueue();
                    });

                System.Diagnostics.Debug.WriteLine($"Rules done for {property.Name}");

            }
            else
            {
                if (!WaitForRules.IsCompleted)
                {
                    waitForRulesSource.SetResult(new object());
                }
            }
        }

        private async Task RunCascadeRulesRecursive(IRegisteredProperty property)
        {
            foreach (var r in Rules.OfType<ICascadeRule<T>>().Where(r => r.TriggerProperties.Contains(property)).ToList())
            {
                // TODO - Wrap with a Try/Catch
                var result = await r.Execute(Target);
                // Todo - Deal with result
            }

        }
    }
}
