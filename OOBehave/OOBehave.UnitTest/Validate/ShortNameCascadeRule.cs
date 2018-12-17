using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.Validate
{
    public class ShortNameCascadeRule : CascadeRule<Validate>
    {

        public ShortNameCascadeRule() : base(new List<IRegisteredProperty> { Validate.FirstNameProperty, Validate.LastNameProperty })
        {

        }

        public override Task<IRuleResult> Execute(Validate target)
        {

            System.Diagnostics.Debug.WriteLine($"Run Rule {target.FirstName} {target.LastName}");

            if(target.FirstName.StartsWith("Error"))
            {
                return Task.FromResult(RuleResult.PropertyError(Validate.FirstNameProperty, target.FirstName));
            }

            target.ShortName = $"{target.FirstName} {target.LastName}";

            return Task.FromResult(RuleResult.Empty());
        }

    }
}
