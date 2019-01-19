using OOBehave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf
{
    public class SimpleObject : EditBase<SimpleObject>, ISimpleObject
    {
        public SimpleObject(IEditBaseServices<SimpleObject> services,
                                    IShortNameRule shortNameRule, IFullNameRule fullNameRule) : base(services)
        {
            FirstName = "John"; 
            RuleManager.AddRule(shortNameRule);
            RuleManager.AddRule(fullNameRule);

            // Needed because not using the Portal
            MarkUnmodified();
            MarkOld();
        }


        public Guid Id { get { return Getter<Guid>(); } }

        public string FirstName { get { return Getter<string>(); } set { Setter(value); } }

        public string LastName { get { return Getter<string>(); } set { Setter(value); } }

        public string ShortName { get { return Getter<string>(); } set { Setter(value); } }

        public string Title { get { return Getter<string>(); } set { Setter(value); } }

        public string FullName { get { return Getter<string>(); } set { Setter(value); } }

    }

    public interface ISimpleObject : IValidateBase
    {
        Guid Id { get; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string ShortName { get; set; }
        string Title { get; set; }
        string FullName { get; set; }

    }
}
