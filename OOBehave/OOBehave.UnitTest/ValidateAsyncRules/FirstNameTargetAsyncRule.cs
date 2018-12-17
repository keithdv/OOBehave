using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.ValidateAsyncRules
{
    public class FirstNameTargetAsyncRule : TargetRule<ValidateAsyncRules> 
    {

        public override async Task<IRuleResult> Execute(ValidateAsyncRules target)
        {

            await Task.Delay(10);

            System.Diagnostics.Debug.WriteLine($"FullNameTargetRule {target.FullName}");

            if (target.FirstName?.StartsWith("Error") ?? false)
            {
                return RuleResult.PropertyError(ValidateAsyncRules.FirstNameProperty, target.FirstName);
            }

            return RuleResult.Empty();
        }

    }
}
