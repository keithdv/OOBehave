﻿using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave.UnitTest.Base
{
    public interface IA { }

    public interface IB : IA { }

    public class B : IB { }

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

        public IA TestPropertyType
        {
            get { return ReadProperty<IA>(); }
            set { LoadProperty(value); }
        }

        /// <summary>
        /// For unit testing purposes only
        /// Do not expose the LoadProperty method like this
        /// </summary>
        /// <param name="propertyValue"></param>
        public void LoadPropertyTest(B propertyValue)
        {
            /// Example - If the types are different you need to explicitly define the type
            /// of the Property
            /// The <IA> in this case
            LoadProperty<IA>(propertyValue, nameof(TestPropertyType));
        }
    }
}
