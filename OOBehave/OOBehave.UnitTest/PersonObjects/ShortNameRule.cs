using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.PersonObjects
{
    public interface IShortNameRule<T> : IRule<T> where T : IPersonBase { }

    public class ShortNameRule<T> : CascadeRule<T>, IShortNameRule<T>
        where T : IPersonBase
    {

        public ShortNameRule() : base()
        {
            TriggerProperties.Add(nameof(IPersonBase.FirstName));
            TriggerProperties.Add(nameof(IPersonBase.LastName));
        }

        public override IRuleResult Execute(T target)
        {

            // System.Diagnostics.Debug.WriteLine($"Run Rule {target.FirstName} {target.LastName}");

            if (target.FirstName?.StartsWith("Error") ?? false)
            {
                return RuleResult.PropertyError(nameof(IPersonBase.FirstName), target.FirstName);
            }

            target.ShortName = $"{target.FirstName} {target.LastName}";

            return RuleResult.Empty();
        }

    }
}
