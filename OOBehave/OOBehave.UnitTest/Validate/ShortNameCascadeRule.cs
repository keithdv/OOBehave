using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.Validate
{
    public class ShortNameCascadeRule : CascadeRule<Validate>
    {

        public ShortNameCascadeRule() : base()
        {
            TriggerProperties.Add(nameof(Validate.FirstName));
            TriggerProperties.Add(nameof(Validate.LastName));
        }

        public override IRuleResult Execute(Validate target)
        {

            System.Diagnostics.Debug.WriteLine($"Run Rule {target.FirstName} {target.LastName}");

            if(target.FirstName.StartsWith("Error"))
            {
                return RuleResult.PropertyError(nameof(Validate.FirstName), target.FirstName);
            }

            target.ShortName = $"{target.FirstName} {target.LastName}";

            return RuleResult.Empty();
        }

    }
}
