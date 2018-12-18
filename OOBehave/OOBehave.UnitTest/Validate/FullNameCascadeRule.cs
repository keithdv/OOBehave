using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.Validate
{
    public class FullNameCascadeRule : CascadeRule<Validate>
    {

        public FullNameCascadeRule() : base()
        {
            TriggerProperties.Add(nameof(Validate.Title));
            TriggerProperties.Add(nameof(Validate.ShortName));
        }

        public override IRuleResult Execute(Validate target)
        {
            target.FullName = $"{target.Title} {target.ShortName}";

            return RuleResult.Empty();

        }

    }
}
