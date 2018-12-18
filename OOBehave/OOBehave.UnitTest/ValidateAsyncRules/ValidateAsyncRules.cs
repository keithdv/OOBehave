using System;
using System.Collections.Generic;
using System.Text;
using OOBehave.Rules;

namespace OOBehave.UnitTest.ValidateAsyncRules
{
    public class ValidateAsyncRules : ValidateBase<ValidateAsyncRules>
    {

        public ValidateAsyncRules(IValidateBaseServices<ValidateAsyncRules> services) : base(services)
        {
            RuleExecute.AddRule(new ShortNameCascadeAsyncRule());
            RuleExecute.AddRule(new FullNameCascadeAsyncRule());
            RuleExecute.AddRule(new FirstNameTargetAsyncRule());
        }

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

    }
}
