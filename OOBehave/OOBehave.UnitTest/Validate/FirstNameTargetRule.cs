using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.Validate
{
    public class FirstNameTargetRule : TargetRule<Validate> 
    {

        public override Task<IRuleResult> Execute(Validate target)
        {

            System.Diagnostics.Debug.WriteLine($"FullNameTargetRule {target.FullName}");

            if (target.FirstName.StartsWith("Error"))
            {
                return Task.FromResult(RuleResult.PropertyError(Validate.FirstNameProperty, target.FirstName));
            }


            return Task.FromResult(RuleResult.Empty());
        }

    }
}
