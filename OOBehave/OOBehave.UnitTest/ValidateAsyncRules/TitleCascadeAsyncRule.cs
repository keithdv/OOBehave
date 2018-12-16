using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.ValidateAsyncRules
{
    public class TitleCascadeAsyncRule : CascadeRule<ValidateAsyncRules>
    {

        public TitleCascadeAsyncRule() : base(new List<IRegisteredProperty>() { ValidateAsyncRules.ShortNameProperty, ValidateAsyncRules.TitleProperty })
        {

        }

        public override async Task<RuleResult> Execute(ValidateAsyncRules target)
        {

            await Task.Delay(100);

            System.Diagnostics.Debug.WriteLine($"Running TitleCascadeRule {target.Title} {target.ShortName}");

            target.FullName = $"{target.Title} {target.ShortName}";

            return new RuleResult();
        }

    }
}
