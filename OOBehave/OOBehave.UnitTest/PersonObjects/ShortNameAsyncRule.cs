using OOBehave.Rules;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.PersonObjects
{
    public class ShortNameAsyncRule<T> : CascadeAsyncRule<T>
        where T : IPersonBase
    {

        public ShortNameAsyncRule() : base()
        {
            TriggerProperties.Add(nameof(IPersonBase.FirstName));
            TriggerProperties.Add(nameof(IPersonBase.LastName));
        }

        public override async Task<IRuleResult> Execute(T target, CancellationToken token)
        {

            await Task.Delay(10, token);

            // System.Diagnostics.Debug.WriteLine($"ShortNameCascadeAsyncRule {target.FirstName} {target.LastName}");

            if (target.FirstName?.StartsWith("Error") ?? false)
            {
                return RuleResult.PropertyError(nameof(IPersonBase.FirstName), target.FirstName);
            }


            target.ShortName = $"{target.FirstName} {target.LastName}";

            return RuleResult.Empty();
        }

    }
}
