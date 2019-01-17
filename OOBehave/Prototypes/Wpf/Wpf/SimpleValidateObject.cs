using OOBehave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf
{
    public class SimpleValidateObject : ValidateBase<SimpleValidateObject>, ISimpleValidateObject
    {
        public SimpleValidateObject(IValidateBaseServices<SimpleValidateObject> services,
                                    IShortNameRule shortNameRule, IFullNameRule fullNameRule) : base(services)
        {
            RuleManager.AddRule(shortNameRule);
            RuleManager.AddRule(fullNameRule);
        }

        public Guid Id { get { return Getter<Guid>(); } }

        public string FirstName { get { return Getter<string>(); } set { Setter(value); } }

        public string LastName { get { return Getter<string>(); } set { Setter(value); } }

        public string ShortName { get { return Getter<string>(); } set { Setter(value); } }

        public string Title { get { return Getter<string>(); } set { Setter(value); } }

        public string FullName { get { return Getter<string>(); } set { Setter(value); } }


    }

    public interface ISimpleValidateObject : IValidateBase
    {
        Guid Id { get; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string ShortName { get; set; }
        string Title { get; set; }
        string FullName { get; set; }

    }
}
