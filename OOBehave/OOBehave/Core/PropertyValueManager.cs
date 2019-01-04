﻿using OOBehave.Attributes;
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
        void Set<P>(IRegisteredProperty<P> registeredProperty, P newValue);
        void Load<P>(IRegisteredProperty<P> registeredProperty, P newValue);
        P Read<P>(IRegisteredProperty<P> registeredProperty);
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

        protected override IPropertyValue CreatePropertyValue<PV>(string name, PV value)
        {
            return Factory.CreatePropertyValue(name, value);
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

        protected abstract P CreatePropertyValue<PV>(string name, PV value);

        public IRegisteredProperty<PV> GetRegisteredProperty<PV>(string name)
        {
            return registeredPropertyManager.GetOrRegisterProperty<PV>(name);
        }

        void ISetTarget.SetTarget(IBase target)
        {
            this.Target = (T)(target ?? throw new ArgumentNullException(nameof(target)));
        }

        public virtual void Set<PV>(string name, PV newValue)
        {
            Set(GetRegisteredProperty<PV>(name), newValue);
        }

        public virtual void Set<PV>(IRegisteredProperty<PV> registeredProperty, PV newValue)
        {
            if (!fieldData.TryGetValue(registeredProperty.Index, out var value))
            {
                // Default(P) so that it get's marked dirty
                // Maybe it would be better to use MarkSelfModified; you know; once I write that
                fieldData[registeredProperty.Index] = value = CreatePropertyValue(registeredProperty.Name, default(PV));
            }

            IPropertyValue<PV> fd = value as IPropertyValue<PV> ?? throw new PropertyTypeMismatchException($"Property {registeredProperty.Name} is not type {typeof(PV).FullName}");
            fd.Value = newValue;

            SetParent(newValue);
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

            fieldData[registeredProperty.Index] = CreatePropertyValue(registeredProperty.Name, newValue);

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

            IPropertyValue<PV> fd = value as IPropertyValue<PV> ?? throw new PropertyTypeMismatchException($"Property {registeredProperty.Name} is not type {typeof(P).FullName}");

            return fd.Value;
        }

        protected void SetParent<PV>(PV newValue)
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
