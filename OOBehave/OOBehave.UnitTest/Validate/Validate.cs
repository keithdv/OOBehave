using System;
using System.Collections.Generic;
using System.Text;
using OOBehave.Rules;

namespace OOBehave.UnitTest.Validate
{
    public class Validate : ValidateBase<Validate>
    {

        public Validate(IValidateBaseServices<Validate> services) : base(services) { }

        public static IRegisteredProperty<string> NameProperty = RegisterProperty<string>(nameof(Name));

        public string Name
        {
            get { return ReadProperty(NameProperty); }
            set { LoadProperty(NameProperty, value); }
        }

        protected override void RegisterRules(IRuleList rules)
        {
            rules.AddRule(new TestRule(NameProperty));
        }

    }
}
