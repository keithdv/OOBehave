using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.ValidateAsyncRules
{
    public class FullNameCascadeAsyncRule : CascadeRule<ValidateAsyncRules>
    {

        public FullNameCascadeAsyncRule() : base(new List<IRegisteredProperty> { ValidateAsyncRules.TitleProperty, ValidateAsyncRules.ShortNameProperty })
        { }

        public override async Task<IRuleResult> Execute(ValidateAsyncRules target)
        {
            await Task.Delay(10);

            System.Diagnostics.Debug.WriteLine($"FullNameCascadeAsyncRule {target.Title} {target.ShortName}");

            target.FullName = $"{target.Title} {target.ShortName}";

            return RuleResult.Empty();

        }

    }
}
