using System;
using System.Collections.Generic;
using System.Text;
using OOBehave.Rules;

namespace OOBehave.UnitTest.PersonObjects

{

    public abstract class PersonBase<T> : ValidateBase<T>, IValidate, IPersonBase
        where T : PersonBase<T>
    {

        public PersonBase(IValidateBaseServices<T> services) : base(services)
        {
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
