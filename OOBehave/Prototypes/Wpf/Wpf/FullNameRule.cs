using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Wpf
{
    public interface IFullNameRule : IRule<ISimpleValidateObject> { }

    internal class FullNameRule : AsyncRuleBase<ISimpleValidateObject>, IFullNameRule
    {
        public FullNameRule() : base()
        {
            TriggerProperties.Add(nameof(ISimpleValidateObject.Title));
            TriggerProperties.Add(nameof(ISimpleValidateObject.ShortName));
        }

        public override async Task<IRuleResult> Execute(ISimpleValidateObject target, CancellationToken token)
        {

            await Task.Delay(10).ConfigureAwait(false);

            var result = RuleResult.Empty();

            if (string.IsNullOrWhiteSpace(target.Title))
            {
                result.AddPropertyError(nameof(ISimpleValidateObject.Title), $"{nameof(ISimpleValidateObject.Title)} is required.");
            }

            if (string.IsNullOrWhiteSpace(target.ShortName))
            {
                result.AddPropertyError(nameof(ISimpleValidateObject.ShortName), $"{nameof(ISimpleValidateObject.ShortName)} is required.");
            }

            if (!result.IsError)
            {
                target.FullName = $"{target.Title} {target.ShortName}";
            }
            else
            {
                target.FullName = string.Empty;
            }

            return result;
        }

    }
}
