using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave.Rules
{

    public interface ITargetRule : IRule
    {

    }

    public interface ITargetRule<T> : ITargetRule, IRule<T>
    {

    }

    public abstract class TargetRule<T> : Rule<T>, ITargetRule<T>
    {
        public TargetRule() : base() { }


    }
}
