using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave.UnitTest.Example.SimpleValidate
{

    public class SimpleValidateObject : ValidateBase<SimpleValidateObject>, ISimpleValidateObject
    {
        public SimpleValidateObject(IValidateBaseServices<SimpleValidateObject> services,
                                    IShortNameRule shortNameRule) : base(services)
        {
            RuleManager.AddRule(shortNameRule);
        }

        public Guid Id { get { return Getter<Guid>(); } }

        public string FirstName { get { return Getter<string>(); } set { Setter(value); } }

        public string LastName { get { return Getter<string>(); } set { Setter(value); } }

        public string ShortName { get { return Getter<string>(); } set { Setter(value); } }

    }

    public interface ISimpleValidateObject : IValidateBase
    {
        Guid Id { get; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string ShortName { get; set; }
    }
}
