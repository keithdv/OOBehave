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

        public override IRuleResult Execute(Validate target)
        {

            System.Diagnostics.Debug.WriteLine($"Run Rule {target.FirstName} {target.LastName}");

            if(target.FirstName.StartsWith("Error"))
            {
                return RuleResult.PropertyError(Validate.FirstNameProperty, target.FirstName);
            }

            target.ShortName = $"{target.FirstName} {target.LastName}";

            return RuleResult.Empty();
        }

    }
}
