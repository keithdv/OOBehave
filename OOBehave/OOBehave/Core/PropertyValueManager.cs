using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOBehave.Core
{
    public interface IPropertyValueManager<T>
    {
        void Load<P>(string name, P newValue);
        P Read<P>(string name);
    }

    public interface IPropertyValue
    {
        string Name { get; }

    }

    public interface IPropertyValue<T> : IPropertyValue
    {
        T Value { get; set; }
    }

    public class PropertyValue<T> : IPropertyValue<T>, IPropertyValue
    {
        public string Name { get; private set; }
        public virtual T Value { get; set; }

        public PropertyValue(string name, T value)
        {
            this.Name = name;
            this.Value = value;
        }
    }

    public class PropertyValueManager<T> : IPropertyValueManager<T>
    {

        protected IFactory Factory { get; }
        protected readonly IRegisteredPropertyManager<T> registeredPropertyManager;
        protected IDictionary<uint, IPropertyValue> fieldData = new ConcurrentDictionary<uint, IPropertyValue>();

        public PropertyValueManager(IRegisteredPropertyManager<T> registeredPropertyManager, IFactory factory)
        {
            this.registeredPropertyManager = registeredPropertyManager;
            Factory = factory;
        }

        protected virtual IPropertyValue<P> CreatePropertyValue<P>(string name, P value)
        {
            return Factory.CreatePropertyValue(name, value);
        }

        protected IRegisteredProperty<P> GetRegisteredProperty<P>(string name)
        {
            return registeredPropertyManager.RegisterProperty<P>(name);
        }

        public virtual void Load<P>(string name, P newValue)
        {
            Load(GetRegisteredProperty<P>(name), newValue);
        }

        public virtual void Load<P>(IRegisteredProperty registeredProperty, P newValue)
        {
            if (!fieldData.ContainsKey(registeredProperty.Index))
            {
                fieldData[registeredProperty.Index] = CreatePropertyValue(registeredProperty.Name, newValue);
            }
            else
            {
                var fd = fieldData[registeredProperty.Index] as IPropertyValue<P> ?? throw new PropertyTypeMismatchException($"FieldData is not {typeof(P).FullName}");
                fd.Value = newValue;
            }
        }

        public P Read<P>(string name)
        {
            return Read<P>(GetRegisteredProperty<P>(name));
        }

        public virtual P Read<P>(IRegisteredProperty<P> registeredProperty)
        {
            if (!fieldData.TryGetValue(registeredProperty.Index, out var value))
            {
                return default(P);
            }

            IPropertyValue<P> fd = value as IPropertyValue<P> ?? throw new PropertyTypeMismatchException($"Property {registeredProperty.Name} is not type {typeof(P).FullName}");

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
