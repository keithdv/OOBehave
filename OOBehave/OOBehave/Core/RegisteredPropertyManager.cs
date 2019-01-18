using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OOBehave.Core
{

    public class RegisteredPropertyManager<T> : IRegisteredPropertyManager<T>
    {

        protected CreateRegisteredProperty CreateRegisteredProperty { get; }
        private IDictionary<string, IRegisteredProperty> RegisteredProperties { get; } = new ConcurrentDictionary<string, IRegisteredProperty>();
        public RegisteredPropertyManager(CreateRegisteredProperty createRegisteredProperty)
        {
            CreateRegisteredProperty = createRegisteredProperty;

#if DEBUG
            if (typeof(T).IsInterface) { throw new Exception($"RegisteredPropertyManager should be service type not interface. {typeof(T).FullName}"); }
#endif
            RegisterProperties();
        }

        private static Type[] ooBehaveTypes = new Type[] { typeof(Base<>), typeof(ListBase<,>), typeof(ValidateBase<>), typeof(ValidateListBase<,>), typeof(EditBase<>), typeof(EditListBase<,>) };

        protected void RegisterProperties()
        {
            var type = typeof(T);

            // If a type does a 'new' on the property you will have duplicate PropertyNames
            // So honor to top-level type that has that propertyName

            // Problem -- this will include All of the properties even ones we don't declare
            do
            {
                var properties = type.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | BindingFlags.DeclaredOnly).ToList();

                foreach (var p in properties)
                {
                    var prop = CreateRegisteredProperty(p);
                    if (!RegisteredProperties.ContainsKey(p.Name))
                    {
                        RegisteredProperties.Add(p.Name, prop);
                    }
                }

                type = type.BaseType;

            } while (!type.IsGenericType || !ooBehaveTypes.Contains(type.GetGenericTypeDefinition()));

        }

        public IRegisteredProperty<P> GetRegisteredProperty<P>(string propertyName)
        {
            var prop = GetRegisteredProperty(propertyName);

            var ret = prop as IRegisteredProperty<P> ?? throw new PropertyTypeMismatchException($"Cannot cast {prop.GetType().FullName} to {typeof(IRegisteredProperty<P>).FullName}.");
            return ret;

        }

        public IRegisteredProperty GetRegisteredProperty(string propertyName)
        {
            if (!RegisteredProperties.TryGetValue(propertyName, out var prop))
            {
                throw new Exception($"{propertyName} missing on {typeof(T).FullName}");
            }

            return prop;
        }

        public IEnumerable<IRegisteredProperty> GetRegisteredProperties()
        {
            return RegisteredProperties.Values;
        }
    }
}
