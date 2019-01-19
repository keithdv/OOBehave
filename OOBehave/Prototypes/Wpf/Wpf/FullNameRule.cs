using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Wpf
{
    public interface IFullNameRule : IRule<ISimpleObject> { }

    internal class FullNameRule : AsyncRuleBase<ISimpleObject>, IFullNameRule
    {
        public FullNameRule() : base()
        {
            TriggerProperties.Add(nameof(ISimpleObject.Title));
            TriggerProperties.Add(nameof(ISimpleObject.ShortName));
        }

        public override async Task<IRuleResult> Execute(ISimpleObject target, CancellationToken token)
        {

            await Task.Delay(1000).ConfigureAwait(false);

            var result = RuleResult.Empty();

            if (string.IsNullOrWhiteSpace(target.Title))
            {
                result.AddPropertyError(nameof(ISimpleObject.Title), $"{nameof(ISimpleObject.Title)} is required.");
            }

            if (string.IsNullOrWhiteSpace(target.ShortName))
            {
                result.AddPropertyError(nameof(ISimpleObject.ShortName), $"{nameof(ISimpleObject.ShortName)} is required.");
            }

            if (!result.IsError)
            {
                target.FullName = $"{target.Title} {target.ShortName}";
            }
            else if (!string.IsNullOrEmpty(target.FullName)) // Don't mark the object as modified
            {
                target.FullName = string.Empty;
            }

            return result;
        }

    }
}
