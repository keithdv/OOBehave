using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.ValidateAsyncRules
{
    public class NameCascadeAsyncRule : CascadeRule<ValidateAsyncRules>
    {

        public NameCascadeAsyncRule() : base(new List<IRegisteredProperty> { ValidateAsyncRules.FirstNameProperty, ValidateAsyncRules.LastNameProperty })
        {

        }

        public override async Task<RuleResult> Execute(ValidateAsyncRules target)
        {

            await Task.Delay(100);

            System.Diagnostics.Debug.WriteLine($"Run Rule {target.FirstName} {target.LastName}");

            target.ShortName = $"{target.FirstName} {target.LastName}";

            return new RuleResult();
        }

    }
}
