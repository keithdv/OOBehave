using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace OOBehave.Core
{
    public interface IFactory
    {
        IRegisteredPropertyManager RegisteredPropertyManager { get; }
        IRegisteredRuleManager RuleManager { get; }
        IRegisteredProperty<T> CreateRegisteredProperty<T>(string name);
        IBaseServices<T> CreateBaseServices<T>();
        IValidateBaseServices<T> CreateValidateBaseServices<T>();
        IRegisteredRuleList<T> CreateRegisteredRuleList<T>(IEnumerable<IRule<T>> rules);
        IRuleList<T> CreateRuleList<T>();
        IRuleExecute<T> CreateRuleExecute<T>(T target, IReadOnlyCollection<IRule<T>> rules);
    }
    public class Factory : IFactory
    {

        // Thoughts
        // Funnel everything thru one container with a very simple default container
        // Allow the definition to be defined as a type like in CSLA

        private static IFactory factory;

        public static IFactory StaticFactory
        {
            get
            {
                if (factory == null) { factory = new Factory(); }
                return factory;
            }
            set
            {
                if (factory != null) { throw new GlobalFactoryException("GlobalFactory can only be set once per execution."); }
                factory = null;
            }
        }

        private IRegisteredPropertyManager registeredPropertyManager;
        public IRegisteredPropertyManager RegisteredPropertyManager
        {
            get
            {
                if (this.registeredPropertyManager == null)
                {

                    this.registeredPropertyManager = new RegisteredPropertyManager();
                }
                return this.registeredPropertyManager;
            }
        }

        private IRegisteredRuleManager ruleManager;

        public IRegisteredRuleManager RuleManager
        {
            get
            {
                if (ruleManager == null) { ruleManager = new RegisteredRuleManager(); }
                return ruleManager;
            }
            set { ruleManager = value; }
        }



        public IRegisteredProperty<T> CreateRegisteredProperty<T>(string name)
        {
            return new RegisteredProperty<T>(name);
        }

        public IBaseServices<T> CreateBaseServices<T>()
        {
            return new BaseServices<T>(new RegisteredPropertyDataManager<T>(RegisteredPropertyManager));
        }

        public IValidateBaseServices<T> CreateValidateBaseServices<T>()
        {
            return new ValidateBaseServices<T>(new RegisteredPropertyValidateDataManager<T>(RegisteredPropertyManager), RuleManager);
        }

        public IEditableBaseServices<T> CreateEditableBaseServices<T>()
        {
            return new EditableBaseServices<T>(new RegisteredPropertyValidateDataManager<T>(RegisteredPropertyManager), RuleManager);
        }

        public IRegisteredRuleList<T> CreateRegisteredRuleList<T>(IEnumerable<IRule<T>> rules)
        {
            var list = new RegisteredRuleList<T>();
            foreach (var r in rules) { list.Add(r); }
            return list;
        }

        public IRuleList<T> CreateRuleList<T>()
        {
            return new RuleList<T>();
        }

        public IRuleExecute<T> CreateRuleExecute<T>(T target, IReadOnlyCollection<IRule<T>> rules)
        {
            return new RuleExecute<T>(target, rules);
        }
    }
}


[Serializable]
public class GlobalFactoryException : Exception
{
    public GlobalFactoryException() { }
    public GlobalFactoryException(string message) : base(message) { }
    public GlobalFactoryException(string message, Exception inner) : base(message, inner) { }
    protected GlobalFactoryException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}

