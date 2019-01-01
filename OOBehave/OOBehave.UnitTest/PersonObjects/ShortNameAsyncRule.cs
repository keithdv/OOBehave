using OOBehave.Rules;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.PersonObjects
{
    public interface IShortNameAsyncRule<T> : IRule<T> where T : IPersonBase { int RunCount { get; } }

    public class ShortNameAsyncRule<T> : AsyncRule<T>, IShortNameAsyncRule<T>
        where T : IPersonBase
    {

        public ShortNameAsyncRule() : base()
        {
            TriggerProperties.Add(nameof(IPersonBase.FirstName));
            TriggerProperties.Add(nameof(IPersonBase.LastName));
        }

        public int RunCount { get; private set; }

        public override async Task<IRuleResult> Execute(T target, CancellationToken token)
        {
            RunCount++;

            await Task.Delay(10, token);

            // System.Diagnostics.Debug.WriteLine($"ShortNameAsyncRule {target.FirstName} {target.LastName}");

            if (target.FirstName?.StartsWith("Error") ?? false)
            {
                return RuleResult.PropertyError(nameof(IPersonBase.FirstName), target.FirstName);
            }


            target.ShortName = $"{target.FirstName} {target.LastName}";

            return RuleResult.Empty();
        }

    }
}
