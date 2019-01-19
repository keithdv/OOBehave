using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Wpf
{
    public interface IShortNameRule : IRule<ISimpleObject> { }

    internal class ShortNameRule : AsyncRuleBase<ISimpleObject>, IShortNameRule
    {
        public ShortNameRule() : base()
        {
            TriggerProperties.Add(nameof(ISimpleObject.FirstName));
            TriggerProperties.Add(nameof(ISimpleObject.LastName));
        }

        public override async Task<IRuleResult> Execute(ISimpleObject target, CancellationToken token)
        {

            await Task.Delay(1000).ConfigureAwait(false);

            var result = RuleResult.Empty();

            if (string.IsNullOrWhiteSpace(target.FirstName))
            {
                result.AddPropertyError(nameof(ISimpleObject.FirstName), $"{nameof(ISimpleObject.FirstName)} is required.");
            }

            if (string.IsNullOrWhiteSpace(target.LastName))
            {
                result.AddPropertyError(nameof(ISimpleObject.LastName), $"{nameof(ISimpleObject.LastName)} is required.");
            }

            if (!result.IsError)
            {
                target.ShortName = $"{target.FirstName} {target.LastName}";
            }
            else if(!string.IsNullOrWhiteSpace(target.ShortName)) // Don't mark the object as modified
            {
                target.ShortName = string.Empty;
            }

            return result;
        }

    }
}
