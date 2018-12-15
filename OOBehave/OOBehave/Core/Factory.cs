﻿using OOBehave.Rules;
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
        IRegisteredPropertyDictionary CreateRegisteredPropertyDictionary();
        IRegisteredProperty<T> CreateRegisteredProperty<T>(string name);
        IBaseServices<T> CreateBaseServices<T>();
        IValidateBaseServices<T> CreateValidateBaseServices<T>();
        IRegisteredRuleList CreateRegisteredRuleList(IEnumerable<IRule> rules);
        IRuleList CreateRuleList();
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

        public IRegisteredPropertyDictionary CreateRegisteredPropertyDictionary()
        {
            return new RegisteredPropertyDictionary();
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

        public IRegisteredRuleList CreateRegisteredRuleList(IEnumerable<IRule> rules)
        {
            var list = new RegisteredRuleList();
            foreach (var r in rules) { list.Add(r); }
            return list;
        }

        public IRuleList CreateRuleList()
        {
            return new RuleList();
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
