using OOBehave.Portal;
using OOBehave.Rules;
using OOBehave.UnitTest.PersonObjects;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.ValidateBaseTests
{
    public interface IValidateAsyncObject : IPersonBase
    {
        IValidateAsyncObject Child { get; set; }
        int RuleRunCount { get; }

        string NoRules { get; set; }
    }

    public class ValidateAsyncObject : PersonValidateBase<ValidateAsyncObject>, IValidateAsyncObject
    {
        public IShortNameAsyncRule<ValidateAsyncObject> ShortNameRule { get; }
        public IFullNameAsyncRule<ValidateAsyncObject> FullNameRule { get; }

        public ValidateAsyncObject(IValidateBaseServices<ValidateAsyncObject> services,
            IShortNameAsyncRule<ValidateAsyncObject> shortNameRule,
            IFullNameAsyncRule<ValidateAsyncObject> fullNameRule
            ) : base(services)
        {
            RuleManager.AddRules(shortNameRule, fullNameRule);
            ShortNameRule = shortNameRule;
            FullNameRule = fullNameRule;
        }

        public IValidateAsyncObject Child { get { return Getter<IValidateAsyncObject>(); } set { Setter(value); } }

        public string NoRules { get => Getter<string>(); set => Setter(value); }

        [Fetch]
        [FetchChild]
        public async Task Fetch(PersonDto person, IReceivePortalChild<IValidateAsyncObject> portal, IReadOnlyList<PersonDto> personTable)
        {
            base.FillFromDto(person);

            var childDto = personTable.FirstOrDefault(p => p.FatherId == Id);

            if (childDto != null)
            {
                Child = await portal.FetchChild(childDto);
            }
        }

        public int RuleRunCount => ShortNameRule.RunCount + FullNameRule.RunCount;

    }


    public interface IValidateAsyncObjectList : IValidateListBase<IValidateAsyncObject>, IPersonBase
    {
        int RuleRunCount { get; }
        void Add(IValidateAsyncObject o);
    }

    public class ValidateAsyncObjectList : PersonValidateListBase<ValidateAsyncObjectList, IValidateAsyncObject>, IValidateAsyncObjectList
    {

        public ValidateAsyncObjectList(IValidateListBaseServices<ValidateAsyncObjectList, IValidateAsyncObject> services,
            IShortNameAsyncRule<ValidateAsyncObjectList> shortNameRule,
            IFullNameAsyncRule<ValidateAsyncObjectList> fullNameRule
            ) : base(services)
        {
            RuleManager.AddRules(shortNameRule, fullNameRule);
            ShortNameRule = shortNameRule;
            FullNameRule = fullNameRule;

        }

        public int RuleRunCount => ShortNameRule.RunCount + FullNameRule.RunCount + this.Select(v => v.RuleRunCount).Sum();
        public IShortNameAsyncRule<ValidateAsyncObjectList> ShortNameRule { get; }
        public IFullNameAsyncRule<ValidateAsyncObjectList> FullNameRule { get; }
    }

}
