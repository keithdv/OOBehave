using OOBehave.Core;
using OOBehave.Rules;

namespace OOBehave
{
    public interface IFactory
    {
        IPropertyValue<P> CreatePropertyValue<P>(IRegisteredProperty<P> registeredProperty, P value);
        IValidatePropertyValue<P> CreateValidatePropertyValue<P>(IRegisteredProperty<P> registeredProperty, P value);
        IEditPropertyValue<P> CreateEditPropertyValue<P>(IRegisteredProperty<P> registeredProperty, P value);
    }

}