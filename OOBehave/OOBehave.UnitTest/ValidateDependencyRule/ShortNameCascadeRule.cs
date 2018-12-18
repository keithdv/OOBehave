using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.ValidateDependencyRule
{
    public class ShortNameCascadeRule<T> : CascadeRule<T>
        where T : IValidate
    {

        public ShortNameCascadeRule(IDisposableDependency dd) : base()
        {
            TriggerProperties.Add(nameof(ValidateDependencyRules.FirstName));
            TriggerProperties.Add(nameof(ValidateDependencyRules.LastName));
            DisposableDependency = dd;
        }

        private IDisposableDependency DisposableDependency { get; }

        public override IRuleResult Execute(T target)
        {

            // System.Diagnostics.Debug.WriteLine($"Run Rule {target.FirstName} {target.LastName}");

            var dd = DisposableDependency ?? throw new ArgumentNullException(nameof(DisposableDependency));

            if (string.IsNullOrWhiteSpace(target.FirstName))
            {
                return RuleResult.PropertyError(nameof(ValidateDependencyRules.FirstName), target.FirstName);
            }

            target.ShortName = $"{target.FirstName} {target.LastName}";

            return RuleResult.Empty();
        }

    }
}
