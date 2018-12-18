using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.Validate
{
    public class FirstNameTargetRule : TargetRule<Validate> 
    {

        public override IRuleResult Execute(Validate target)
        {

            System.Diagnostics.Debug.WriteLine($"FullNameTargetRule {target.FullName}");

            if (target.FirstName.StartsWith("Error"))
            {
                return RuleResult.PropertyError(nameof(Validate.FirstName), target.FirstName);
            }


            return RuleResult.Empty();
        }

    }
}
