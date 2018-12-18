using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.ValidateAsyncRules
{
    public class ShortNameCascadeAsyncRule : CascadeAsyncRule<ValidateAsyncRules>
    {

        public ShortNameCascadeAsyncRule() : base(new List<IRegisteredProperty> { ValidateAsyncRules.FirstNameProperty, ValidateAsyncRules.LastNameProperty })
        {

        }

        public override async Task<IRuleResult> Execute(ValidateAsyncRules target, CancellationToken token)
        {

            await Task.Delay(10, token);

            System.Diagnostics.Debug.WriteLine($"ShortNameCascadeAsyncRule {target.FirstName} {target.LastName}");

            if (target.FirstName.StartsWith("Error"))
            {
                return RuleResult.PropertyError(ValidateAsyncRules.FirstNameProperty, target.FirstName);
            }

            target.ShortName = $"{target.FirstName} {target.LastName}";

            return RuleResult.Empty();
        }

    }
}
