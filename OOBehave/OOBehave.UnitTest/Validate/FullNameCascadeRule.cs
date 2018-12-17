using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.Validate
{
    public class FullNameCascadeRule : CascadeRule<Validate>
    {

        public FullNameCascadeRule() : base(new List<IRegisteredProperty> { Validate.TitleProperty, Validate.ShortNameProperty })
        { }

        public override Task<IRuleResult> Execute(Validate target)
        {
            target.FullName = $"{target.Title} {target.ShortName}";

            return Task.FromResult(RuleResult.Empty());

        }

    }
}
