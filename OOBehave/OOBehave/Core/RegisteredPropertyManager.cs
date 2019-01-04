using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOBehave.Core
{

    public class RegisteredPropertyManager<T> : IRegisteredPropertyManager<T>
    {

        private IFactory Factory { get; }
        private IDictionary<string, IRegisteredProperty> RegisteredProperties { get; } = new ConcurrentDictionary<string, IRegisteredProperty>();
        public RegisteredPropertyManager(IFactory factory)
        {
            this.Factory = factory;

#if DEBUG
            if (typeof(T).IsInterface) { throw new Exception($"RegisteredPropertyManager should be service type not interface. {typeof(T).FullName}"); }
#endif
        }

        public IRegisteredProperty<P> GetOrRegisterProperty<P>(string name)
        {

            if (!RegisteredProperties.TryGetValue(name, out var prop))
            {
                // Check that the correct type of object is being sent in
                var property = typeof(T).GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public)
                    .Where(f => f.Name == name).FirstOrDefault();

                if (property == null) { throw new PropertyNotFoundException($"Property {name} not found on {typeof(T).FullName}"); }
                if (property.PropertyType != typeof(P)) { throw new PropertyNotFoundException($"Property {name} isn't of type {typeof(P).FullName}. Explicitly define type of LoadProperty method to LoadProperty<{property.PropertyType.Name}> for this senario."); }

                prop = Factory.CreateRegisteredProperty<P>(name);
                RegisteredProperties.Add(name, prop);
            }

            var ret = prop as IRegisteredProperty<P> ?? throw new PropertyTypeMismatchException($"Cannot cast {prop.GetType().FullName} to {typeof(IRegisteredProperty<P>).FullName}.");
            return ret;
        }

        public IEnumerable<IRegisteredProperty> GetRegisteredProperties()
        {
            return RegisteredProperties.Values;
        }
    }
}
