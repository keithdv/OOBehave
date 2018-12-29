using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace OOBehave.Core
{

    /// <summary>
    /// You can't register generic delegates in C#
    /// so you make a factory class
    /// </summary>
    public class DefaultFactory : IFactory
    {
        private static uint index = 0;
        private static uint NextIndex() { index++; return index; } // This may be overly simple and in the wrong spot
        private IServiceScope Scope { get; }

        public DefaultFactory(IServiceScope scope)
        {
            Scope = scope;
        }

        public IRegisteredProperty<T> CreateRegisteredProperty<T>(string name)
        {
            System.Diagnostics.Debug.WriteLine($"Register Property {name}");
            return new RegisteredProperty<T>(name, NextIndex());
        }

        public IRuleExecute<T> CreateRuleExecute<T>(T target) where T : IValidateBase
        {
            return new RuleExecute<T>(target);
        }

        public IPropertyValue CreatePropertyValue<P>(string name, P value)
        {
            return new PropertyValue<P>(name, value);
        }
        public IValidatePropertyValue CreateValidatePropertyValue<P>(string name, P value)
        {
            return new ValidatePropertyValue<P>(name, value);
        }

        public IEditPropertyValue CreateEditPropertyValue<P>(string name, P value)
        {
            return new EditPropertyValue<P>(Scope.Resolve<IValuesDiffer>(), name, value);
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

}