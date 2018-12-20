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

        T Create<T>() where T : IOOBehaveObject;

        T Create<T, C>(C criteria) where T : IOOBehaveObject;

    }

    public interface IServiceScope
    {
        T Resolve<T>();

        object Resolve(Type t);

        bool TryResolve<T>(out T result);

        bool TryResolve(Type T, out object result);

        bool IsRegistered<T>();

        bool IsRegistered(Type type);
    }

    /// <summary>
    /// You can't register generic delegates in C#
    /// so you make a factory class
    /// </summary>
    public class DefaultFactory : IFactory
    {
        private IServiceScope scope { get; }

        public DefaultFactory(IServiceScope scope)
        {
            this.scope = scope;
        }

        public IRegisteredProperty<T> CreateRegisteredProperty<T>(string name)
        {
            System.Diagnostics.Debug.WriteLine($"Register Property {name}");
            return new RegisteredProperty<T>(name);
        }

        public IRuleExecute<T> CreateRuleExecute<T>(T target)
        {
            return new RuleExecute<T>(target);
        }

        public T Create<T>() where T : IOOBehaveObject
        {
            return scope.Resolve<T>();
        }

        public T Create<T, C>(C criteria) where T : IOOBehaveObject
        {
            return scope.Resolve<T>();
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