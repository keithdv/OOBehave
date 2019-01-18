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
            this.PropertyChanged += SimpleValidateObject_PropertyChanged;
            Properties = new Properties_(this);
            PropertyIsBusy = new IsBusy(this);
        }

        private void SimpleValidateObject_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IValidateBase.IsValid))
            {
                PropertyHasChanged(nameof(Properties));
            }

            if (e.PropertyName == nameof(IValidateBase.IsBusy))
            {
                PropertyHasChanged(nameof(PropertyIsBusy));
            }
        }

        public Guid Id { get { return Getter<Guid>(); } }

        public string FirstName { get { return Getter<string>(); } set { Setter(value); } }

        public string LastName { get { return Getter<string>(); } set { Setter(value); } }

        public string ShortName { get { return Getter<string>(); } set { Setter(value); } }

        public string Title { get { return Getter<string>(); } set { Setter(value); } }

        public string FullName { get { return Getter<string>(); } set { Setter(value); } }

        public Properties_ Properties { get; }

        public IsBusy PropertyIsBusy { get; }

    }

    public class IsBusy
    {
        public IsBusy(SimpleValidateObject simpleValidateObject)
        {
            SimpleValidateObject = simpleValidateObject;
        }

        public bool this[string propertyName]
        {
            get
            {
                return SimpleValidateObject[propertyName]?.IsBusy ?? false;
            }
        }

        public SimpleValidateObject SimpleValidateObject { get; }
    }

    public class Properties_
    {
        public Properties_(SimpleValidateObject simpleValidateObject)
        {
            SimpleValidateObject = simpleValidateObject;
        }

        public SimpleValidateObject SimpleValidateObject { get; }
        public IValidatePropertyMeta this[string propertyName]
        {
            get
            {
                return SimpleValidateObject[propertyName];
            }
        }


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
