using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave.UnitTest.Base
{
    public class Base : Base<Base>
    {

        public Base(IBaseServices<Base> services) : base(services) { }

        public static IRegisteredProperty<string> NameProperty = RegisterProperty<string>(nameof(Name));

        public string Name
        {
            get { return ReadProperty(NameProperty); }
            set { LoadProperty(NameProperty, value); }
        }


    }
}
