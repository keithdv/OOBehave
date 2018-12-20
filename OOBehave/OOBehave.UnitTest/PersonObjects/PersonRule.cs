using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.PersonObjects
{
    public class PersonRule<T> : TargetRule<T>
        where T : IPersonBase
    {

        public override IRuleResult Execute(T target)
        {

            var ruleResult = RuleResult.Empty();

            if (string.IsNullOrWhiteSpace(target.ShortName))
            {
                ruleResult.AddPropertyError(nameof(IPersonBase.ShortName), target.ShortName);
            }

            if (string.IsNullOrWhiteSpace(target.FullName))
            {
                ruleResult.AddPropertyError(nameof(IPersonBase.FullName), target.FullName);
            }

            return ruleResult;
        }

    }
}
