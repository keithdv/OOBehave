using System;
using System.Collections.Generic;
using System.Text;
using OOBehave.Rules;

namespace OOBehave.UnitTest.PersonObjects

{
    public abstract class PersonValidateListBase<L, T> : ValidateListBase<T>, IPersonBase
        where L : PersonValidateListBase<L, T>
        where T : IPersonBase
    {

        public PersonValidateListBase(IValidateListBaseServices<L, T> services) : base(services)
        {
        }

        private IRegisteredProperty<Guid> IdProperty => GetRegisteredProperty<Guid>(nameof(Id));
        public Guid Id { get { return Getter<Guid>(); } }

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

        public string ShortName
        {
            get { return Getter<string>(); }
            set { Setter(value); }
        }

        public string Title
        {
            get { return Getter<string>(); }
            set { Setter(value); }
        }

        public string FullName
        {
            get { return Getter<string>(); }
            set { Setter(value); }
        }

        public uint? Age
        {
            get => Getter<uint>(); set => Setter(value);
        }

        public void FillFromDto(PersonDto dto)
        {
            LoadProperty(IdProperty, dto.PersonId);
            FirstName = dto.FirstName;
            LastName = dto.LastName;
            Title = dto.Title;
        }
    }
}
