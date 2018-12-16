using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace OOBehave.Rules
{

    public interface IRuleList : IList
    {

    }
    public interface IRuleList<T> : IList<IRule<T>>, IRuleList
    {
        void AddRule(IRule<T> rule);
    }


    /// <summary>
    /// Use only for retrieving the rules from the ValidateBase<> object
    /// </summary>
    public class RuleList<T> : List<IRule<T>>, IRuleList<T>
    {
        public void AddRule(IRule<T> rule)
        {
            base.Add(rule);
        }

    }
}
