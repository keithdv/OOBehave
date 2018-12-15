using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.Validate
{
    public class TestRule : Rule
    {

        public TestRule(IRegisteredProperty registeredProperty) : base(registeredProperty)
        {

        }

        public override Task<IRuleResult> Execute(IRuleContext context)
        {
            throw new NotImplementedException();
        }

    }
}
