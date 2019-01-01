using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OOBehave.Rules
{

    public interface IRule
    {
        /// <summary>
        /// Must be unique for every rule across all types
        /// </summary>
        uint UniqueIndex { get; }
        IReadOnlyList<string> TriggerProperties { get; }

    }

    public interface IRule<T> : IRule
    {
        Task<IRuleResult> Execute(T target, CancellationToken token);
    }

    public abstract class AsyncRule<T> : IRule<T>
    {

        private static uint indexer = 1;

        protected AsyncRule()
        {
            /// Must be unique for every rule across all types so Static counter is important
            UniqueIndex = indexer;
            indexer++;
        }

        public AsyncRule(params string[] triggerProperties) : this(triggerProperties.AsEnumerable()) { }

        public AsyncRule(IEnumerable<string> triggerProperties) : this()
        {
            TriggerProperties.AddRange(triggerProperties);
        }

        public uint UniqueIndex { get; }

        IReadOnlyList<string> IRule.TriggerProperties => TriggerProperties.AsReadOnly();
        protected List<string> TriggerProperties { get; } = new List<string>();


        // TODO - Pass Cancellation Token and Cancel if we reach this 
        // rule again and it is currently running

        public abstract Task<IRuleResult> Execute(T target, CancellationToken token);

    }


    public abstract class Rule<T> : AsyncRule<T>
    {
        protected Rule() : base() { }

        public Rule(IEnumerable<string> triggerProperties) : base(triggerProperties) { }

        public Rule(params string[] triggerProperties) : this(triggerProperties.AsEnumerable()) { }

        public abstract IRuleResult Execute(T target);

        public sealed override Task<IRuleResult> Execute(T target, CancellationToken token)
        {
            return Task.FromResult(Execute(target));
        }

    }

    public class FluentRule<T> : Rule<T>
    {
        private Func<T, IRuleResult> ExecuteFunc { get; }
        public FluentRule(Func<T, IRuleResult> execute, params string[] triggerProperties) : base(triggerProperties)
        {
            this.ExecuteFunc = execute;
        }

        public override IRuleResult Execute(T target)
        {
            return ExecuteFunc(target);
        }
    }
}
