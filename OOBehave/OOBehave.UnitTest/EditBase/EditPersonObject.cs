using OOBehave.Portal;
using OOBehave.UnitTest.PersonObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.EditBase
{

    public interface IEditPerson : IPersonBase, IEditBase<IEditPerson>
    {
        IEditPerson Child { get; }
    }

    public class EditPerson : PersonEditBase<EditPerson>, IEditPerson
    {
        public EditPerson(IEditableBaseServices<EditPerson> services,
            IShortNameAsyncRule<EditPerson> shortNameRule,
            IFullNameAsyncRule<EditPerson> fullNameRule,
            IPersonAsyncRule<EditPerson> personRule) : base(services)
        {
            RuleExecute.AddRules(shortNameRule, fullNameRule, personRule);
        }

        public IEditPerson Child
        {
            get { return ReadProperty<IEditPerson>(); }
            set { SetProperty(value); }
        }

        [Fetch]
        public async Task Fetch(PersonDto person, IReceivePortal<IEditPerson> portal, IReadOnlyList<PersonDto> personTable)
        {
            base.FillFromDto(person);

            var child = personTable.FirstOrDefault(p => p.FatherId == PersonId);

            if (child != null)
            {
                LoadProperty(await portal.FetchChild(child), nameof(Child));
            }
        }


        [FetchChild]
        public void Fetch(PersonDto dto)
        {
            base.FillFromDto(dto);
        }

    }
}
