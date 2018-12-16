using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.Validate
{
    public class NameCascadeRule : CascadeRule<Validate>
    {

        public NameCascadeRule() : base(new List<IRegisteredProperty> { Validate.FirstNameProperty, Validate.LastNameProperty })
        {

        }

        public override Task<RuleResult> Execute(Validate target)
        {

            System.Diagnostics.Debug.WriteLine($"Run Rule {target.FirstName} {target.LastName}");

            target.ShortName = $"{target.FirstName} {target.LastName}";

            return Task.FromResult(new RuleResult());
        }

    }
}
