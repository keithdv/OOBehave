using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.ValidateAsyncRules
{
    public class FirstNameTargetAsyncRule : TargetAsyncRule<ValidateAsyncRules> 
    {

        public override async Task<IRuleResult> Execute(ValidateAsyncRules target, CancellationToken token)
        {

            await Task.Delay(10, token);

            System.Diagnostics.Debug.WriteLine($"FullNameTargetRule {target.FullName}");

            if (target.FirstName?.StartsWith("Error") ?? false)
            {
                return RuleResult.PropertyError(nameof(ValidateAsyncRules.FirstName), target.FirstName);
            }

            return RuleResult.Empty();
        }

    }
}
