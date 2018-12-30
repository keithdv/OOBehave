using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OOBehave.Rules
{

    public interface ICascadeRule : IRule
    {
        IReadOnlyList<string> TriggerProperties { get; }

    }

    public interface ICascadeRule<T> : ICascadeRule, IRule<T>
    {


    }

    public abstract class CascadeAsyncRule<T> : Rule<T>, ICascadeRule<T>
    {

        protected CascadeAsyncRule() : base() { }

        public CascadeAsyncRule(params string[] triggerProperties) : this(triggerProperties.AsEnumerable()) { }

        public CascadeAsyncRule(IEnumerable<string> triggerProperties)
        {
            TriggerProperties.AddRange(triggerProperties);
        }

        IReadOnlyList<string> ICascadeRule.TriggerProperties => TriggerProperties.AsReadOnly();
        protected List<string> TriggerProperties { get; } = new List<string>();

    }

    public abstract class CascadeRule<T> : CascadeAsyncRule<T>
    {
        protected CascadeRule() : base() { }

        public CascadeRule(IEnumerable<string> triggerProperties) : base(triggerProperties) { }

        public CascadeRule(params string[] triggerProperties) : this(triggerProperties.AsEnumerable()) { }

        public abstract IRuleResult Execute(T target);

        public sealed override Task<IRuleResult> Execute(T target, CancellationToken token)
        {
            return Task.FromResult(Execute(target));
        }

    }

    public class FluentRule<T> : CascadeRule<T>
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
