using OOBehave.Portal;
using OOBehave.UnitTest.PersonObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.EditBaseTests
{

    public interface IEditPersonList : IEditListBase<IEditPerson>, IPersonBase
    {
    }

    public class EditPersonList : EditListBase<IEditPerson>, IEditPersonList
    {
        public EditPersonList(IEditListBaseServices<EditPersonList, IEditPerson> services,
            IShortNameAsyncRule<EditPersonList> shortNameRule,
            IFullNameAsyncRule<EditPersonList> fullNameRule) : base(services)
        {
            RuleExecute.AddRules(shortNameRule, fullNameRule);
        }


        public Guid PersonId { get { return Getter<Guid>(); } }

        public string FirstName { get { return Getter<string>(); } set { Setter(value); } }

        public string LastName { get { return Getter<string>(); } set { Setter(value); } }

        public string ShortName { get { return Getter<string>(); } set { Setter(value); } }

        public string Title { get { return Getter<string>(); } set { Setter(value); } }

        public string FullName { get { return Getter<string>(); } set { Setter(value); } }

        public uint? Age { get => Getter<uint>(); set => Setter(value); }

        [Fetch]
        private async Task FillFromDto(PersonDto dto, IReadOnlyList<PersonDto> personTable)
        {
            LoadProperty(nameof(PersonId), dto.PersonId);

            // These will not mark IsModified to true
            // as long as within ObjectPortal operation
            FirstName = dto.FirstName;
            LastName = dto.LastName;
            Title = dto.Title;

            var children = personTable.Where(p => p.FatherId == PersonId);

            foreach (var child in children)
            {
                Add(await ItemPortal.FetchChild(child));
            }
        }



    }
}
