using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave.Rules
{

    public interface IRuleList : IList<IRule>
    {
        void AddRule(IRule rule);
    }


    public class RuleList : List<IRule>, IRuleList
    {
        public void AddRule(IRule rule)
        {
            base.Add(rule);
        }

    }
}
