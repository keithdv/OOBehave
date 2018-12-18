using System;
using System.Collections.Generic;
using System.Text;
using OOBehave.Rules;

namespace OOBehave.UnitTest.Validate
{
    public class Validate : ValidateBase<Validate>
    {

        public Validate(IValidateBaseServices<Validate> services) : base(services) { }

        public string FirstName
        {
            get { return ReadProperty<string>(); }
            set { SetProperty(value); }
        }

        public string LastName
        {
            get { return ReadProperty<string>(); }
            set { SetProperty(value); }
        }


        public string ShortName
        {
            get { return ReadProperty<string>(); }
            set { SetProperty(value); }
        }

        public string Title
        {
            get { return ReadProperty<string>(); }
            set { SetProperty(value); }
        }

        public string FullName
        {
            get { return ReadProperty<string>(); }
            set { SetProperty(value); }
        }

        protected override void RegisterRules(IRuleList<Validate> rules)
        {
            rules.AddRule(new ShortNameCascadeRule());
            rules.AddRule(new FullNameCascadeRule());
            rules.AddRule(new FirstNameTargetRule());
        }

    }
}
