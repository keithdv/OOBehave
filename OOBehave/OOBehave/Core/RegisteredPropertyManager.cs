using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOBehave.Core
{
    // TODO
    // What happens if I make this <T> and registered SingleInstance??

    public class RegisteredPropertyManager : IRegisteredPropertyManager
    {

        private IFactory Factory { get; }
        public RegisteredPropertyManager(IFactory factory)
        {
            this.Factory = factory;
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
                // Check that the correct type of object is being sent in
                var property = objectType.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public)
                    .Where(f => f.Name == name).FirstOrDefault();

                if (property == null) { throw new PropertyNotFoundException($"Property {name} not found on {objectType.FullName}"); }
                if (property.PropertyType != typeof(P)) { throw new PropertyNotFoundException($"Property {name} isn't of type {typeof(P).FullName}. Explicitly define type of LoadProperty method to LoadProperty<{property.PropertyType.Name}> for this senario."); }

                prop = Factory.CreateRegisteredProperty<P>(name);
                keyValuePairs.Add(name, prop);
            }

            var ret = prop as IRegisteredProperty<P> ?? throw new PropertyTypeMismatchException($"Cannot cast {prop.GetType().FullName} to {typeof(IRegisteredProperty<P>).FullName}.");
            return ret;
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
