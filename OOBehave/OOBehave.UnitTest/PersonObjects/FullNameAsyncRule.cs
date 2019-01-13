using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.PersonObjects
{

    public interface IFullNameAsyncRule<T> : IRule<T> where T : IPersonBase { int RunCount { get; } }

    public class FullNameAsyncRule<T> : AsyncRuleBase<T>, IFullNameAsyncRule<T>
        where T : IPersonBase
    {

        public FullNameAsyncRule() : base()
        {

            TriggerProperties.Add(nameof(IPersonBase.FirstName));
            TriggerProperties.Add(nameof(IPersonBase.ShortName));
        }

        public int RunCount { get; private set; }

        public override async Task<IRuleResult> Execute(T target, CancellationToken token)
        {
            RunCount++;

            await Task.Delay(10, token);

            // System.Diagnostics.Debug.WriteLine($"FullNameAsyncRule {target.Title} {target.ShortName}");

            target.FullName = $"{target.Title} {target.ShortName}";

            return RuleResult.Empty();

        }

    }
}
