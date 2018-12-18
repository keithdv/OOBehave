using System;
using System.Collections.Generic;
using System.Text;
using OOBehave.Rules;

namespace OOBehave.UnitTest.ValidateAsyncRules
{
    public class ValidateAsyncRules : ValidateBase<ValidateAsyncRules>
    {

        public ValidateAsyncRules(IValidateBaseServices<ValidateAsyncRules> services) : base(services) { }

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




        protected override void RegisterRules(IRuleList<ValidateAsyncRules> rules)
        {
            rules.AddRule(new ShortNameCascadeAsyncRule());
            rules.AddRule(new FullNameCascadeAsyncRule());
            rules.AddRule(new FirstNameTargetAsyncRule());
        }

    }
}
