using OOBehave.Rules;

namespace OOBehave
{
    public interface IFactory
    {
        IRegisteredProperty<T> CreateRegisteredProperty<T>(string name);
        IRuleExecute<T> CreateRuleExecute<T>(T target);

    }

}