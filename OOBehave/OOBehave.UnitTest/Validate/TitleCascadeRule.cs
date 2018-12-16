using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.Validate
{
    public class TitleCascadeRule : CascadeRule<Validate>
    {

        public TitleCascadeRule() : base(new List<IRegisteredProperty>() { Validate.ShortNameProperty, Validate.TitleProperty })
        {

        }

        public override Task<RuleResult> Execute(Validate target)
        {
            System.Diagnostics.Debug.WriteLine($"Running TitleCascadeRule {target.Title} {target.ShortName}");

            target.FullName = $"{target.Title} {target.ShortName}";

            return Task.FromResult(new RuleResult());
        }

    }
}
