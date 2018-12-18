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

        IDictionary<Type, IDictionary<string, IRegisteredProperty>> RegisteredProperties { get; } = new ConcurrentDictionary<Type, IDictionary<string, IRegisteredProperty>>();

        public IRegisteredProperty<P> RegisterProperty<T, P>(string name)
        {
            return RegisterProperty<P>(typeof(T), name);
        }

        private IRegisteredProperty<P> RegisterProperty<P>(Type objectType, string name)
        {

            if (!RegisteredProperties.TryGetValue(objectType, out var keyValuePairs))
            {
                RegisteredProperties.Add(objectType, new ConcurrentDictionary<string, IRegisteredProperty>());
                keyValuePairs = RegisteredProperties[objectType];
            }

            if (!keyValuePairs.TryGetValue(name, out var prop))
            {
                prop = Core.Factory.StaticFactory.CreateRegisteredProperty<P>(name);
                keyValuePairs.Add(name, prop);
            }

            return (IRegisteredProperty<P>) prop;
        }

        public IReadOnlyList<IRegisteredProperty> GetRegisteredPropertiesForType(Type objectType)
        {
            if (!RegisteredProperties.ContainsKey(objectType))
            {
                return new List<IRegisteredProperty>().AsReadOnly();
            }

            return RegisteredProperties[objectType].Values.ToList().AsReadOnly();
        }


    }


}
