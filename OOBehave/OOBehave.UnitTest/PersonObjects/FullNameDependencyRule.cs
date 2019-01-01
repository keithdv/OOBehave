using OOBehave.Rules;
using OOBehave.UnitTest.Objects;
using System;


namespace OOBehave.UnitTest.PersonObjects
{
    public interface IFullNameDependencyRule<T> : IRule<T> where T : IPersonBase { }

    public class FullNameDependencyRule<T> : Rule<T>, IFullNameDependencyRule<T>
        where T : IPersonBase
    {

        public FullNameDependencyRule(IDisposableDependency dd) : base()
        {
            TriggerProperties.Add(nameof(IPersonBase.Title));
            TriggerProperties.Add(nameof(IPersonBase.ShortName));

            this.DisposableDependency = dd;
        }

        private IDisposableDependency DisposableDependency { get; }

        public override IRuleResult Execute(T target)
        {

            var dd = DisposableDependency ?? throw new ArgumentNullException(nameof(DisposableDependency));

            target.FullName = $"{target.Title} {target.ShortName}";

            return RuleResult.Empty();

        }

    }
}
