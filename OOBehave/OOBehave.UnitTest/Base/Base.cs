using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave.UnitTest.Base
{
    public class Base : Base<Base>
    {

        public Base(IBaseServices<Base> services) : base(services) { }

        public Guid Id
        {
            get { return ReadProperty<Guid>(); }
            set { LoadProperty(value); }
        }

        public string FirstName
        {
            get { return ReadProperty<string>(); }
            set { LoadProperty(value); }
        }

        public string LastName
        {
            get { return ReadProperty<string>(); }
            set { LoadProperty(value); }
        }

    }
}
