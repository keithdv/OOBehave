using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave.UnitTest.Base
{
    public class Base : Base<Base>
    {

        public Base(IBaseServices<Base> services) : base(services) { }


        public string Name
        {
            get { return ReadProperty<string>(); }
            set { LoadProperty(value); }
        }


    }
}
