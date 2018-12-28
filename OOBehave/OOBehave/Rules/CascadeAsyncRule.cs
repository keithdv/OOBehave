using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OOBehave.Rules
{
    public interface ICascadeRule<T> : IRule<T>
    {

        IEnumerable<string> TriggerProperties { get; }

    }

    public abstract class CascadeAsyncRule<T> : Rule<T>, ICascadeRule<T>
    {

        protected CascadeAsyncRule() : base()
        {

        }

        public CascadeAsyncRule(IEnumerable<string> triggerProperties) : this()
        {
            TriggerProperties.AddRange(triggerProperties);
        }

        IEnumerable<string> ICascadeRule<T>.TriggerProperties => TriggerProperties.AsEnumerable();
        protected List<string> TriggerProperties { get; } = new List<string>();

    }

    public abstract class CascadeRule<T> : CascadeAsyncRule<T>
    {
        protected CascadeRule() : base()
        {

        }

        public CascadeRule(IEnumerable<string> triggerProperties) : this()
        {
            TriggerProperties.AddRange(triggerProperties);
        }

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
