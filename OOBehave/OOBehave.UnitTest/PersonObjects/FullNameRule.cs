using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.PersonObjects
{
    public interface IFullNameRule<T> : IRule<T> where T : IPersonBase { int RunCount { get; } }

    public class FullNameRule<T> : RuleBase<T>, IFullNameRule<T>
        where T : IPersonBase
    {
        public int RunCount { get; private set; } = 0;

        public FullNameRule() : base()
        {
            TriggerProperties.Add(nameof(IPersonBase.Title));
            TriggerProperties.Add(nameof(IPersonBase.ShortName));
        }

        public override IRuleResult Execute(T target)
        {

            RunCount++;

            target.FullName = $"{target.Title} {target.ShortName}";

            return RuleResult.Empty();

        }

    }
}
