using OOBehave.Core;
using OOBehave.Rules;

namespace OOBehave
{
    public interface IFactory
    {
        IRegisteredProperty<T> CreateRegisteredProperty<T>(string name);
        IRuleExecute<T> CreateRuleExecute<T>(T target);
        IPropertyValue<P> CreatePropertyValue<P>(string name, P value);
        IValidatePropertyValue<P> CreateValidatePropertyValue<P>(string name, P value);
        IEditPropertyValue<P> CreateEditPropertyValue<P>(string name, P value);
    }

}