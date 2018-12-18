using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOBehave.Core
{
    public interface IRegisteredPropertyDataManager<T>
    {
        void Load<P>(string name, P newValue);
        P Read<P>(string name);
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

        protected readonly IRegisteredPropertyManager registeredPropertyManager;
        protected IDictionary<uint, IRegisteredPropertyData> fieldData = new ConcurrentDictionary<uint, IRegisteredPropertyData>();

        public RegisteredPropertyDataManager(IRegisteredPropertyManager registeredPropertyManager)
        {
            this.registeredPropertyManager = registeredPropertyManager;
        }

        protected virtual IRegisteredPropertyData<P> CreateRegisteredPropertyData<P>(string name, P value)
        {
            return new RegisteredPropertyData<P>(name, value);
        }

        private IRegisteredProperty<P> GetRegisteredProperty<P>(string name)
        {
            return registeredPropertyManager.RegisterProperty<T, P>(name);
        }

        public void Load<P>(string name, P newValue)
        {
            Load(GetRegisteredProperty<P>(name), newValue);
        }

        public void Load<P>(IRegisteredProperty registeredProperty, P newValue)
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

        public P Read<P>(string name)
        {
            return Read<P>(GetRegisteredProperty<P>(name));
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
