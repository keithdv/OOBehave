using OOBehave.Portal;
using OOBehave.UnitTest.PersonObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.EditBase
{

    public interface IEditPersonParentChild : IPersonBase, IEditBase<IEditPersonParentChild>
    {

        IEditPersonParentChild Child { get; }

    }

    public class EditPersonParentChild : PersonEditBase<EditPersonParentChild>, IEditPersonParentChild
    {
        public EditPersonParentChild(IEditBaseServices<EditPersonParentChild> services,
            IShortNameAsyncRule<EditPersonParentChild> shortNameRule,
            IFullNameAsyncRule<EditPersonParentChild> fullNameRule,
            IPersonAsyncRule<EditPersonParentChild> personRule) : base(services)
        {
            RuleExecute.AddRules(shortNameRule, fullNameRule, personRule);
        }

        public IEditPersonParentChild Child { get => Getter<IEditPersonParentChild>(); private set => Setter(value); }

        [Fetch]
        [FetchChild]
        public async Task Fetch(PersonDto person, IReceivePortalChild<IEditPersonParentChild> portal, IReadOnlyList<PersonDto> personTable)
        {
            base.FillFromDto(person);

            var childDto = personTable.FirstOrDefault(p => p.FatherId == PersonId);

            if (childDto != null)
            {
                Child = await portal.FetchChild(childDto);
            }

        }

    }
}
