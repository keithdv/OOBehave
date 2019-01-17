using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Wpf
{
    public interface IShortNameRule : IRule<ISimpleValidateObject> { }

    internal class ShortNameRule : AsyncRuleBase<ISimpleValidateObject>, IShortNameRule
    {
        public ShortNameRule() : base()
        {
            TriggerProperties.Add(nameof(ISimpleValidateObject.FirstName));
            TriggerProperties.Add(nameof(ISimpleValidateObject.LastName));
        }

        public override async Task<IRuleResult> Execute(ISimpleValidateObject target, CancellationToken token)
        {

            await Task.Delay(10).ConfigureAwait(false);

            var result = RuleResult.Empty();

            if (string.IsNullOrWhiteSpace(target.FirstName))
            {
                result.AddPropertyError(nameof(ISimpleValidateObject.FirstName), $"{nameof(ISimpleValidateObject.FirstName)} is required.");
            }

            if (string.IsNullOrWhiteSpace(target.LastName))
            {
                result.AddPropertyError(nameof(ISimpleValidateObject.LastName), $"{nameof(ISimpleValidateObject.LastName)} is required.");
            }

            if (!result.IsError)
            {
                target.ShortName = $"{target.FirstName} {target.LastName}";
            }
            else
            {
                target.ShortName = string.Empty;
            }

            return result;
        }

    }
}
