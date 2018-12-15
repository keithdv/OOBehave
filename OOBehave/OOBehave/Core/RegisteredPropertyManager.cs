using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOBehave.Core
{

    public class RegisteredPropertyManager : IRegisteredPropertyManager
    {

        public RegisteredPropertyManager()
        {
        }

        IDictionary<Type, IRegisteredPropertyDictionary> RegisteredProperties { get; } = new ConcurrentDictionary<Type, IRegisteredPropertyDictionary>();

        public IRegisteredProperty<P> RegisterProperty<T, P>(string name)
        {
            var prop = Core.Factory.StaticFactory.CreateRegisteredProperty<P>(name);
            RegisterProperty(typeof(T), prop);
            return prop;
        }

        public void RegisterProperty(Type objectType, IRegisteredProperty metaProperty)
        {

            if (!RegisteredProperties.TryGetValue(objectType, out var keyValuePairs))
            {
                RegisteredProperties.Add(objectType, Core.Factory.StaticFactory.CreateRegisteredPropertyDictionary());
                keyValuePairs = RegisteredProperties[objectType];
            }

            if (keyValuePairs.ContainsKey(metaProperty.Key)) { throw new RegisteredPropertyKeyAlreadyExistsException(metaProperty.Key); }

            keyValuePairs.Add(metaProperty.Key, metaProperty);

        }
        
        public IReadOnlyList<IRegisteredProperty> GetRegisteredPropertiesForType(Type objectType)
        {
            if (!RegisteredProperties.ContainsKey(objectType))
            {
                RegisteredProperties.Add(objectType, Core.Factory.StaticFactory.CreateRegisteredPropertyDictionary());
            }

            return RegisteredProperties[objectType].Values.ToList().AsReadOnly();
        }


    }


    [Serializable]
    public class RegisteredPropertyKeyAlreadyExistsException : Exception
    {
        public RegisteredPropertyKeyAlreadyExistsException() { }
        public RegisteredPropertyKeyAlreadyExistsException(string message) : base(message) { }
        public RegisteredPropertyKeyAlreadyExistsException(string message, Exception inner) : base(message, inner) { }
        protected RegisteredPropertyKeyAlreadyExistsException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
