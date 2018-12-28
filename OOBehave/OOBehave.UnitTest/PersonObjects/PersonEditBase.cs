using System;
using System.Collections.Generic;
using System.Text;
using OOBehave.Rules;

namespace OOBehave.UnitTest.PersonObjects

{
    public abstract class PersonEditBase<T> : EditBase<T>, IPersonBase
        where T : PersonEditBase<T>
    {

        public PersonEditBase(IEditBaseServices<T> services) : base(services)
        {
        }

        public Guid PersonId { get { return Getter<Guid>(); } }

        public string FirstName { get { return Getter<string>(); } set { Setter(value); } }

        public string LastName { get { return Getter<string>(); } set { Setter(value); } }

        public string ShortName { get { return Getter<string>(); } set { Setter(value); } }

        public string Title { get { return Getter<string>(); } set { Setter(value); } }

        public string FullName { get { return Getter<string>(); } set { Setter(value); } }

        public uint? Age { get => Getter<uint>(); set => Setter(value); }
        protected void FillFromDto(PersonDto dto)
        {
            LoadProperty(nameof(PersonId), dto.PersonId);

            // These will not mark IsModified to true
            // as long as within ObjectPortal operation
            FirstName = dto.FirstName;
            LastName = dto.LastName;
            Title = dto.Title;
        }
    }
}
