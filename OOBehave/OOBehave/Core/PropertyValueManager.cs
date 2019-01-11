using OOBehave.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace OOBehave.Core
{

    /// <summary>
    /// DO NOT REGISTER IN THE CONTAINER
    /// </summary>
    public interface IPropertyValueManager
    {
        IRegisteredProperty<PV> GetRegisteredProperty<PV>(string name);
        void Load<P>(IRegisteredProperty<P> registeredProperty, P newValue);
        P Read<P>(IRegisteredProperty<P> registeredProperty);

        // This isn't possible without some nasty reflection or static backing fields
        // If the property is being loaded for the first time you need the type
        //void Load(IRegisteredProperty registeredProperty, object newValue);
        object Read(IRegisteredProperty registeredProperty);
    }


    /// <summary>
    /// This is what is registered from the container so that it is Type specific
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPropertyValueManager<T> : IPropertyValueManager
    {

    }

    public interface IPropertyValue
    {
        string Name { get; }

    }

    public interface IPropertyValue<T> : IPropertyValue
    {
        T Value { get; set; }
    }

    [PortalDataContract]
    public class PropertyValue<T> : IPropertyValue<T>, IPropertyValue
    {
        [PortalDataMember]
        public string Name { get; protected set; } // Setter for Deserialization of Edit

        [PortalDataMember]
        public virtual T Value { get; set; }

        protected PropertyValue() { } // For EditPropertyValue Deserialization

        public PropertyValue(string name, T value)
        {
            this.Name = name;
            this.Value = value;
        }
    }

    public class PropertyValueManager<T> : PropertyValueManagerBase<T, IPropertyValue>
        where T : IBase
    {
        public PropertyValueManager(IRegisteredPropertyManager<T> registeredPropertyManager, IFactory factory) : base(registeredPropertyManager, factory)
        {

        }

        protected override IPropertyValue CreatePropertyValue<PV>(IRegisteredProperty<PV> registeredProperty, PV value)
        {
            return Factory.CreatePropertyValue(registeredProperty, value);
        }
    }

    [PortalDataContract]
    public abstract class PropertyValueManagerBase<T, P> : IPropertyValueManager<T>, ISetTarget
        where T : IBase
        where P : IPropertyValue
    {
        protected T Target { get; set; }

        protected IFactory Factory { get; }
        protected readonly IRegisteredPropertyManager<T> registeredPropertyManager;

        [PortalDataMember]
        protected IDictionary<uint, P> fieldData = new ConcurrentDictionary<uint, P>();

        public PropertyValueManagerBase(IRegisteredPropertyManager<T> registeredPropertyManager, IFactory factory)
        {
            this.registeredPropertyManager = registeredPropertyManager;
            Factory = factory;
        }

        protected abstract P CreatePropertyValue<PV>(IRegisteredProperty<PV> registeredProperty, PV value);

        public IRegisteredProperty<PV> GetRegisteredProperty<PV>(string name)
        {
            return registeredPropertyManager.GetRegisteredProperty<PV>(name);
        }

        void ISetTarget.SetTarget(IBase target)
        {
            this.Target = (T)(target ?? throw new ArgumentNullException(nameof(target)));
        }


        public virtual void Load<PV>(string name, PV newValue)
        {
            Load(GetRegisteredProperty<PV>(name), newValue);
        }

        public virtual void Load<PV>(IRegisteredProperty<PV> registeredProperty, PV newValue)
        {
            if (!fieldData.ContainsKey(registeredProperty.Index))
            {
                // TODO Destroy and Delink to old value
            }

            fieldData[registeredProperty.Index] = CreatePropertyValue(registeredProperty, newValue);

            SetParent(newValue);
        }

        public PV Read<PV>(string name)
        {
            return Read<PV>(GetRegisteredProperty<PV>(name));
        }

        public virtual PV Read<PV>(IRegisteredProperty<PV> registeredProperty)
        {
            if (!fieldData.TryGetValue(registeredProperty.Index, out var value))
            {
                return default(PV);
            }

            IPropertyValue<PV> fd = value as IPropertyValue<PV> ?? throw new PropertyTypeMismatchException($"Property {registeredProperty.Name} is not type {typeof(PV).FullName}");

            return fd.Value;
        }

        public virtual object Read(IRegisteredProperty registeredProperty)
        {
            if (fieldData.TryGetValue(registeredProperty.Index, out var fd))
            {
                return fd;
            }

            return null;

        }

        protected void SetParent(object newValue)
        {
            if (newValue is ISetParent x)
            {
                if (Target == null) { throw new ArgumentNullException(nameof(Target)); }
                x.SetParent(Target);
            }
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
