using OOBehave.Core;
using OOBehave.Rules;

namespace OOBehave
{
    public interface IFactory
    {
        IRegisteredProperty<T> CreateRegisteredProperty<T>(string name);
        IPropertyValue CreatePropertyValue<P>(string name, P value);
        IValidatePropertyValue CreateValidatePropertyValue<P>(string name, P value);
        IEditPropertyValue CreateEditPropertyValue<P>(string name, P value);
    }

}