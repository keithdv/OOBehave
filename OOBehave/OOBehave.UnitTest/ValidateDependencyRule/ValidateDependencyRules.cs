using System;
using System.Collections.Generic;
using System.Text;
using OOBehave.Rules;

namespace OOBehave.UnitTest.ValidateDependencyRule
{

    public class ValidateDependencyRules : ValidateBase<ValidateDependencyRules>, IValidate
    {

        public ValidateDependencyRules(IValidateBaseServices<ValidateDependencyRules> services,
                ShortNameCascadeRule<ValidateDependencyRules> shortNameRule,
                FullNameCascadeRule<ValidateDependencyRules> fullNameRule,
                FirstNameTargetDependencyRule<ValidateDependencyRules> firstNameRule) : base(services)
        {
            RuleExecute.AddRule(shortNameRule);
            RuleExecute.AddRule(fullNameRule);
            RuleExecute.AddRule(firstNameRule);
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
