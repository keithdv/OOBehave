using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace OOBehave.Core
{
    public interface IFactory
    {
        IRegisteredProperty<T> CreateRegisteredProperty<T>(string name);
        IRuleExecute<T> CreateRuleExecute<T>(T target);
    }

    /// <summary>
    /// You can't register generic delegates in C#
    /// so you make a factory class
    /// </summary>
    public class DefaultFactory : IFactory
    {


        public IRegisteredProperty<T> CreateRegisteredProperty<T>(string name)
        {
            return new RegisteredProperty<T>(name);
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