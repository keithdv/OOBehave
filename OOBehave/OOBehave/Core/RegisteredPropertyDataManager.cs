using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOBehave.Core
{
    public interface IRegisteredPropertyDataManager<T>
    {
        void Load<P>(IRegisteredProperty<P> registeredProperty, P newValue);
        P Read<P>(IRegisteredProperty<P> registeredProperty);
    }

    public interface IRegisteredPropertyData
    {
        string Name { get; }

    }

    public interface IRegisteredPropertyData<T> : IRegisteredPropertyData
    {
        T Value { get; set; }
    }

    public class RegisteredPropertyData<T> : IRegisteredPropertyData<T>, IRegisteredPropertyData
    {
        public string Name { get; private set; }
        public virtual T Value { get; set; }

        public RegisteredPropertyData(string name, T value)
        {
            this.Name = name;
            this.Value = value;
        }
    }

    public class RegisteredPropertyDataManager<T> : IRegisteredPropertyDataManager<T>
    {

        protected readonly IReadOnlyList<IRegisteredProperty> registeredPropertyList;
        protected readonly IRegisteredPropertyManager registeredPropertyManager;
        protected readonly Type type;
        protected IDictionary<uint, IRegisteredPropertyData> fieldData = new ConcurrentDictionary<uint, IRegisteredPropertyData>();

        public RegisteredPropertyDataManager(IRegisteredPropertyManager registeredPropertyManager)
        {
            this.registeredPropertyManager = registeredPropertyManager;
            this.type = typeof(T);
            ForceStaticFieldInit(type);
            this.registeredPropertyList = GetRegisteredProperties(type, registeredPropertyManager);
        }

        internal static IReadOnlyList<IRegisteredProperty> GetRegisteredProperties(Type type, IRegisteredPropertyManager registeredPropertyManager)
        {
            List<IRegisteredProperty> result = new List<IRegisteredProperty>();

            // get inheritance hierarchy
            Type current = type;
            List<Type> hierarchy = new List<Type>();
            do
            {
                hierarchy.Add(current);
                current = current.BaseType;
            } while (current != null && !current.Equals(typeof(T)));

            // walk from top to bottom to build consolidated list
            for (int index = hierarchy.Count - 1; index >= 0; index--)
            {
                var source = registeredPropertyManager.GetRegisteredPropertiesForType(hierarchy[index]);
                //source.IsLocked = true; TODO??
                result.AddRange(source);
            }

            return result.AsReadOnly();
        }

        private static List<Type> hasBeenInit = new List<Type>();
        /// <summary>
        /// Forces initialization of the static fields declared
        /// by a type, and any of its base class types.
        /// </summary>
        /// <param name="type">Type of object to initialize.</param>
        private static void ForceStaticFieldInit(Type type)
        {
            if (!hasBeenInit.Contains(type))
            {
                var attr =
                  System.Reflection.BindingFlags.Static |
                  System.Reflection.BindingFlags.Public |
                  System.Reflection.BindingFlags.DeclaredOnly |
                  System.Reflection.BindingFlags.NonPublic;

                var t = type;

                while (t != null)
                {
                    var fields = t.GetFields(attr);
                    if (fields.Length > 0)
                        fields[0].GetValue(null);
                    t = t.BaseType;
                }
            }
        }

        protected virtual IRegisteredPropertyData<P> CreateRegisteredPropertyData<P>(string name, P value)
        {
            return new RegisteredPropertyData<P>(name, value);
        }

        public void Load<P>(IRegisteredProperty<P> registeredProperty, P newValue)
        {
            if (!fieldData.ContainsKey(registeredProperty.Index))
            {
                fieldData[registeredProperty.Index] = CreateRegisteredPropertyData(registeredProperty.Name, newValue);
            }
            else
            {
                var fd = fieldData[registeredProperty.Index] as IRegisteredPropertyData<P> ?? throw new PropertyTypeMismatchException($"FieldData is not {typeof(P).FullName}");
                fd.Value = newValue;
            }
        }

        public P Read<P>(IRegisteredProperty<P> registeredProperty)
        {
            if (!fieldData.TryGetValue(registeredProperty.Index, out var value))
            {
                return default(P);
            }

            IRegisteredPropertyData<P> fd = value as IRegisteredPropertyData<P> ?? throw new PropertyTypeMismatchException($"Property {registeredProperty.Name} is not type {typeof(P).FullName}");

            return fd.Value;
        }
    }


    [Serializable]
    public class PropertyTypeMismatchException : Exception
    {

        public PropertyTypeMismatchException() { }
        public PropertyTypeMismatchException(string message) : base(message) { }
        public PropertyTypeMismatchException(string message, Exception inner) : base(message, inner) { }
        protected PropertyTypeMismatchException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }


    [Serializable]
    public class PropertyNotFoundException : Exception
    {
        public PropertyNotFoundException() { }
        public PropertyNotFoundException(string message) : base(message) { }
        public PropertyNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected PropertyNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
