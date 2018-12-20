using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.PersonObjects
{
    public class FullNameAsyncRule<T> : CascadeAsyncRule<T>
        where T : IPersonBase
    {

        public FullNameAsyncRule() : base()
        {

            TriggerProperties.Add(nameof(IPersonBase.FirstName));
            TriggerProperties.Add(nameof(IPersonBase.ShortName));
        }

        public override async Task<IRuleResult> Execute(T target, CancellationToken token)
        {
            await Task.Delay(10, token);

            // System.Diagnostics.Debug.WriteLine($"FullNameCascadeAsyncRule {target.Title} {target.ShortName}");

            target.FullName = $"{target.Title} {target.ShortName}";

            return RuleResult.Empty();

        }

    }
}
