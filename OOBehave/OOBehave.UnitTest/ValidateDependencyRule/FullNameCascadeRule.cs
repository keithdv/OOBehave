using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.ValidateDependencyRule
{
    public class FullNameCascadeRule<T> : CascadeRule<T>
        where T : IValidate
    {

        public FullNameCascadeRule(IDisposableDependency dd) : base()
        {
            TriggerProperties.Add(nameof(ValidateDependencyRules.Title));
            TriggerProperties.Add(nameof(ValidateDependencyRules.ShortName));

            this.DisposableDependency = dd;
        }

        private IDisposableDependency DisposableDependency { get; }

        public override IRuleResult Execute(T target)
        {

            var dd = DisposableDependency ?? throw new ArgumentNullException(nameof(DisposableDependency));

            target.FullName = $"{target.Title} {target.ShortName}";

            return RuleResult.Empty();

        }

    }
}
