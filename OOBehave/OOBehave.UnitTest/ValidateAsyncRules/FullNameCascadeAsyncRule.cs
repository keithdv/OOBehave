using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.ValidateAsyncRules
{
    public class FullNameCascadeAsyncRule : CascadeAsyncRule<ValidateAsyncRules>
    {

        public FullNameCascadeAsyncRule() : base()
        {

            TriggerProperties.Add(nameof(ValidateAsyncRules.Title));
            TriggerProperties.Add(nameof(ValidateAsyncRules.ShortName));
        }

        public override async Task<IRuleResult> Execute(ValidateAsyncRules target, CancellationToken token)
        {
            await Task.Delay(10, token);

            System.Diagnostics.Debug.WriteLine($"FullNameCascadeAsyncRule {target.Title} {target.ShortName}");

            target.FullName = $"{target.Title} {target.ShortName}";

            return RuleResult.Empty();

        }

    }
}
