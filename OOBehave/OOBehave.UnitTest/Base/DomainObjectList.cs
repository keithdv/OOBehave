using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave.UnitTest.Base
{

    public interface IDomainObjectList : IListBase<IDomainObject>
    {
        Guid Id { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }

    }
    public class DomainObjectList : ListBase<DomainObjectList, IDomainObject>, IDomainObjectList
    {

        public DomainObjectList(IListBaseServices<DomainObjectList, IDomainObject> services) : base(services) { }

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


    }
}
