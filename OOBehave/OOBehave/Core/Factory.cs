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

        public IPropertyValue<P> CreatePropertyValue<P>(IRegisteredProperty<P> registeredProperty, P value)
        {
            return new PropertyValue<P>(registeredProperty.Name, value);
        }
        public IValidatePropertyValue<P> CreateValidatePropertyValue<P>(IRegisteredProperty<P> registeredProperty, P value)
        {
            return new ValidatePropertyValue<P>(registeredProperty.Name, value);
        }

        public IEditPropertyValue<P> CreateEditPropertyValue<P>(IRegisteredProperty<P> registeredProperty, P value)
        {
            return new EditPropertyValue<P>(registeredProperty.Name, value);
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