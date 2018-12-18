using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OOBehave.Rules
{

    public interface ITargetRule : IRule
    {

    }

    public interface ITargetRule<T> : ITargetRule, IRule<T>
    {

    }

    public abstract class TargetAsyncRule<T> : Rule<T>, ITargetRule<T>
    {
        public TargetAsyncRule() : base() { }


    }

    public abstract class TargetRule<T> : TargetAsyncRule<T>
    {
        public TargetRule() : base() { }

        public abstract IRuleResult Execute(T target);

        public sealed override Task<IRuleResult> Execute(T target, CancellationToken token)
        {
            return Task.FromResult(Execute(target));
        }

    }

}
