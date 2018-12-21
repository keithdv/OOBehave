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

        private IServiceScope scope { get; }

        public DefaultFactory(IServiceScope scope)
        {
            this.scope = scope;
        }

        public IRegisteredProperty<T> CreateRegisteredProperty<T>(string name)
        {
            System.Diagnostics.Debug.WriteLine($"Register Property {name}");
            return new RegisteredProperty<T>(name, NextIndex());
        }

        public IRuleExecute<T> CreateRuleExecute<T>(T target)
        {
            return new RuleExecute<T>(target);
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