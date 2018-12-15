using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace OOBehave.Rules
{

    public interface IRegisteredRuleManager
    {
        IReadOnlyDictionary<Type, IRegisteredRuleList> RegisteredRules { get; }
        void RegisterRules<T>(Action<IRuleList> action);
    }

    /// <summary>
    /// Maintain the rules for each OOBehave.ValidateBase type
    /// </summary>
    public class RegisteredRuleManager : IRegisteredRuleManager
    {

        private ConcurrentDictionary<Type, IRegisteredRuleList> _registeredRules { get; } = new ConcurrentDictionary<Type, IRegisteredRuleList>();
        IDictionary<Type, IRegisteredRuleList> RegisteredRules => _registeredRules;
        IReadOnlyDictionary<Type, IRegisteredRuleList> IRegisteredRuleManager.RegisteredRules => _registeredRules;

        public void RegisterRules<T>(Action<IRuleList> action)
        {
            if (!RegisteredRules.ContainsKey(typeof(T)))
            {
                var ruleList = Core.Factory.StaticFactory.CreateRuleList();
                action(ruleList);
                RegisteredRules.Add(typeof(T), Core.Factory.StaticFactory.CreateRegisteredRuleList(ruleList));
            }
        }


    }
}
