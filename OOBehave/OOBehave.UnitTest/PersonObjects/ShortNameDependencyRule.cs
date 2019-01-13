
using OOBehave.Rules;
using OOBehave.UnitTest.Objects;
using System;

namespace OOBehave.UnitTest.PersonObjects
{
    public interface IShortNameDependencyRule<T> : IRule<T> where T : IPersonBase { }

    public class ShortNameDependencyRule<T> : RuleBase<T>, IShortNameDependencyRule<T>
        where T : IPersonBase
    {

        public ShortNameDependencyRule(IDisposableDependency dd) : base()
        {
            TriggerProperties.Add(nameof(IPersonBase.FirstName));
            TriggerProperties.Add(nameof(IPersonBase.LastName));
            DisposableDependency = dd;
        }

        private IDisposableDependency DisposableDependency { get; }

        public override IRuleResult Execute(T target)
        {

            // System.Diagnostics.Debug.WriteLine($"Run Rule {target.FirstName} {target.LastName}");

            var dd = DisposableDependency ?? throw new ArgumentNullException(nameof(DisposableDependency));

            if (target.FirstName?.StartsWith("Error") ?? false)
            {
                return RuleResult.PropertyError(nameof(IPersonBase.FirstName), target.FirstName);
            }


            target.ShortName = $"{target.FirstName} {target.LastName}";

            return RuleResult.Empty();
        }

    }
}
