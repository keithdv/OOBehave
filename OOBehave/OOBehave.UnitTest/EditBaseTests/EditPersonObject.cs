using OOBehave.Portal;
using OOBehave.UnitTest.PersonObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.EditBaseTests
{

    public interface IEditPerson : IPersonBase, IEditBase
    {
        List<int> InitiallyNull { get; set; }
        List<int> InitiallyDefined { get; set; }
    }

    public class EditPerson : PersonEditBase<EditPerson>, IEditPerson
    {
        public EditPerson(IEditBaseServices<EditPerson> services,
            IShortNameAsyncRule<EditPerson> shortNameRule,
            IFullNameAsyncRule<EditPerson> fullNameRule) : base(services)
        {
            RuleExecute.AddRules(shortNameRule, fullNameRule);
        }

        [Fetch]
        [FetchChild]
        public void Fetch(PersonDto person, IReceivePortal<IEditPerson> portal, IReadOnlyList<PersonDto> personTable)
        {
            base.FillFromDto(person);

            InitiallyDefined = new List<int>() { 1, 2, 3 };

        }

        public List<int> InitiallyNull { get => Getter<List<int>>(); set => Setter(value); }
        public List<int> InitiallyDefined { get => Getter<List<int>>(); set => Setter(value); }


    }
}
