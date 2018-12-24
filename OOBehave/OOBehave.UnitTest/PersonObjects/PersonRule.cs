using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.PersonObjects
{
    public interface IPersonRule<T> : IRule<T> where T : IPersonBase { }

    public class PersonRule<T> : TargetRule<T>, IPersonRule<T>
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
