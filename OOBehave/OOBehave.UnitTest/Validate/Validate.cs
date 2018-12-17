using System;
using System.Collections.Generic;
using System.Text;
using OOBehave.Rules;

namespace OOBehave.UnitTest.Validate
{
    public class Validate : ValidateBase<Validate>
    {

        public Validate(IValidateBaseServices<Validate> services) : base(services) { }

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

        protected override void RegisterRules(IRuleList<Validate> rules)
        {
            rules.AddRule(new ShortNameCascadeRule());
            rules.AddRule(new FullNameCascadeRule());
            rules.AddRule(new FirstNameTargetRule());
        }

    }
}
