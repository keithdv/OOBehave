using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.PersonObjects
{
    public class PersonAsyncRule<T> : TargetAsyncRule<T>
        where T : IPersonBase
    {

        public override async Task<IRuleResult> Execute(T target, CancellationToken token)
        {

            await Task.Delay(10, token);

            var ruleResult = RuleResult.Empty();

            if (string.IsNullOrWhiteSpace(target.ShortName))
            {
                ruleResult.AddPropertyError(nameof(IPersonBase.ShortName), target.ShortName);
            }

            if (string.IsNullOrWhiteSpace(target.FullName))
            {
                ruleResult.AddPropertyError(nameof(IPersonBase.FullName), target.FullName);
            }

            return ruleResult;
        }

    }
}
