using System;
using System.Collections.Generic;
using System.Text;
using OOBehave.Rules;

namespace OOBehave.UnitTest.ValidateAsyncRules
{
    public class ValidateAsyncRules : ValidateBase<ValidateAsyncRules>
    {

        public ValidateAsyncRules(IValidateBaseServices<ValidateAsyncRules> services) : base(services) { }

        public static IRegisteredProperty<string> FirstNameProperty = RegisterProperty<string>(nameof(FirstName));

        public string FirstName
        {
            get { return ReadProperty(FirstNameProperty); }
            set { SetProperty(FirstNameProperty, value); }
        }


        public static IRegisteredProperty<string> LastNameProperty = RegisterProperty<string>(nameof(LastName));

        public string LastName
        {
            get { return ReadProperty(LastNameProperty); }
            set { SetProperty(LastNameProperty, value); }
        }


        public static IRegisteredProperty<string> ShortNameProperty = RegisterProperty<string>(nameof(ShortNameProperty));

        public string ShortName
        {
            get { return ReadProperty(ShortNameProperty); }
            set { SetProperty(ShortNameProperty, value); }
        }

        public static IRegisteredProperty<string> TitleProperty = RegisterProperty<string>(nameof(TitleProperty));

        public string Title
        {
            get { return ReadProperty(TitleProperty); }
            set { SetProperty(TitleProperty, value); }
        }

        public static IRegisteredProperty<string> FullNameProperty = RegisterProperty<string>(nameof(FullNameProperty));

        public string FullName
        {
            get { return ReadProperty(FullNameProperty); }
            set { SetProperty(FullNameProperty, value); }
        }




        protected override void RegisterRules(IRuleList<ValidateAsyncRules> rules)
        {
            rules.AddRule(new ShortNameCascadeAsyncRule());
            rules.AddRule(new FullNameCascadeAsyncRule());
            rules.AddRule(new FirstNameTargetAsyncRule());
        }

    }
}
