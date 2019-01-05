using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave.UnitTest.BaseTests
{
    public interface IA { }

    public interface IB : IA { }

    public class B : IB { }

    public interface IBaseObject : IBase
    {
        Guid Id { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        IA TestPropertyType { get; set; }
        void LoadPropertyTest(B propertyValue);
        IBaseObject Child { get; set; }
    }
    public class BaseObject : Base<BaseObject>, IBaseObject
    {

        public BaseObject(IBaseServices<BaseObject> services) : base(services) { }

        public Guid Id
        {
            get { return Getter<Guid>(); }
            set { Setter(value); }
        }

        public string FirstName
        {
            get { return Getter<string>(); }
            set { Setter(value); }
        }

        public string LastName
        {
            get { return Getter<string>(); }
            set { Setter(value); }
        }

        public IBaseObject Child
        {
            get { return Getter<IBaseObject>(); }
            set { Setter(value); }
        }

        private IRegisteredProperty<IA> TestPropertyTypeProperty => GetRegisteredProperty<IA>(nameof(TestPropertyType));
        public IA TestPropertyType
        {
            get { return Getter<IA>(); }
            set { Setter(value); }
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
            LoadProperty<IA>(TestPropertyTypeProperty, propertyValue);

        }
    }
}
