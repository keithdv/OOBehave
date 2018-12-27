using OOBehave.Rules;
using OOBehave.UnitTest.Objects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.PersonObjects
{

    public interface IPersonDependencyRule<T> : IRule<T>
    {

    }

    public class PersonDependencyRule<T> : TargetRule<T>, IPersonDependencyRule<T>
        where T : IPersonBase
    {
        public PersonDependencyRule(IDisposableDependency dd)
        {
            DisposableDependency = dd;
        }

        private IDisposableDependency DisposableDependency { get; }

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
