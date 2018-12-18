using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.ValidateDependencyRule
{
    public class FirstNameTargetDependencyRule<T> : TargetRule<T>
        where T : IValidate
    {

        public FirstNameTargetDependencyRule(IDisposableDependency dd)
        {
            DisposableDependency = dd;
        }

        private IDisposableDependency DisposableDependency { get; }

        public override IRuleResult Execute(T target)
        {
            var dd = DisposableDependency ?? throw new ArgumentNullException(nameof(DisposableDependency));

            // System.Diagnostics.Debug.WriteLine($"FullNameTargetRule {target.FullName}");

            if (string.IsNullOrWhiteSpace(target.FirstName))
            {
                return RuleResult.PropertyError(nameof(ValidateDependencyRules.FirstName), target.FirstName);
            }


            return RuleResult.Empty();
        }

    }
}
