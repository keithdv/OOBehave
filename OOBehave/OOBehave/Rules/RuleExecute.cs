﻿using System;
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

                while (propertyQueue.TryDequeue(out var property))
                {

                    System.Diagnostics.Debug.WriteLine($"Dequeue {property.Name}");

                    var cascadeRuleTask = RunCascadeRulesRecursive(property, token);

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



        private async Task RunCascadeRulesRecursive(IRegisteredProperty property, CancellationToken token)
        {
            foreach (var r in Rules.OfType<ICascadeRule<T>>().Where(r => r.TriggerProperties.Contains(property)).ToList())
            {
                // TODO - Wrap with a Try/Catch
                var result = await r.Execute(Target, token);

                if (token.IsCancellationRequested)
                {
                    break;
                }

                Results[r.UniqueIndex] = result;

            }
        }

        private async Task RunTargetRulesSequential(CancellationToken token)
        {
            foreach (var r in Rules.OfType<ITargetRule<T>>().ToList())
            {
                // TODO - Wrap with a Try/Catch
                var result = await r.Execute(Target, token);

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
}

