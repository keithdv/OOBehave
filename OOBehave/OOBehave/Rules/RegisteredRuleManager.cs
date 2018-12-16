using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOBehave.Rules
{

    public interface IRegisteredRuleManager
    {
        IReadOnlyDictionary<Type, IRegisteredRuleList> RegisteredRules { get; }
        void RegisterRules<T>(Action<IRuleList<T>> action);
        IReadOnlyList<IRule<T>> GetRegisteredRules<T>();
    }

    /// <summary>
    /// Maintain the rules for each OOBehave.ValidateBase type
    /// </summary>
    public class RegisteredRuleManager : IRegisteredRuleManager
    {

        private ConcurrentDictionary<Type, IRegisteredRuleList> _registeredRules { get; } = new ConcurrentDictionary<Type, IRegisteredRuleList>();
        IDictionary<Type, IRegisteredRuleList> RegisteredRules => _registeredRules;
        IReadOnlyDictionary<Type, IRegisteredRuleList> IRegisteredRuleManager.RegisteredRules => _registeredRules;

        public void RegisterRules<T>(Action<IRuleList<T>> action)
        {
            if (!RegisteredRules.ContainsKey(typeof(T)))
            {
                var ruleList = Core.Factory.StaticFactory.CreateRuleList<T>();
                action(ruleList);
                RegisteredRules.Add(typeof(T), Core.Factory.StaticFactory.CreateRegisteredRuleList(ruleList));
            }
        }

        public IReadOnlyList<IRule<T>> GetRegisteredRules<T>()
        {
            if (!RegisteredRules.ContainsKey(typeof(T))) { throw new TypeNotFoundException($"Rules not found for {typeof(T)}"); }

            var result = RegisteredRules[typeof(T)] as IRegisteredRuleList<T> ?? throw new WrongTypeException();
            return result.ToList().AsReadOnly();
        }

    }


    [Serializable]
    public class TypeNotFoundException : Exception
    {
        public TypeNotFoundException() { }
        public TypeNotFoundException(string message) : base(message) { }
        public TypeNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected TypeNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }


    [Serializable]
    public class WrongTypeException : Exception
    {
        public WrongTypeException() { }
        public WrongTypeException(string message) : base(message) { }
        public WrongTypeException(string message, Exception inner) : base(message, inner) { }
        protected WrongTypeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
