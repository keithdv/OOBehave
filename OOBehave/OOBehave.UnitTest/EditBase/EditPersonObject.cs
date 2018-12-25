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
        IEditPerson Child { get; set; }
        List<int> InitiallyNull { get; set; }
        List<int> InitiallyDefined { get; set; }
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
            get { return Getter<IEditPerson>(); }
            set { Setter(value); }
        }

        [Fetch]
        public async Task Fetch(PersonDto person, IReceivePortal<IEditPerson> portal, IReadOnlyList<PersonDto> personTable)
        {
            base.FillFromDto(person);

            var childDto = personTable.FirstOrDefault(p => p.FatherId == PersonId);

            if (childDto != null)
            {
                Child = await portal.FetchChild(childDto);
            }

            InitiallyDefined = new List<int>() { 1, 2, 3 };

        }

        public List<int> InitiallyNull { get => Getter<List<int>>(); set => Setter(value); }
        public List<int> InitiallyDefined { get => Getter<List<int>>(); set => Setter(value); }

        [FetchChild]
        public void Fetch(PersonDto dto)
        {
            base.FillFromDto(dto);
        }

    }
}
