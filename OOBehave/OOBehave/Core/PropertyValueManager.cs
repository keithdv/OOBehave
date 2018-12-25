﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOBehave.Core
{
    public interface IPropertyValueManager<T>
    {
        void Set<P>(string name, P newValue);
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
        public string Name { get; }
        public virtual T Value { get; set; }

        public PropertyValue(string name)
        {
            this.Name = name;
        }

        public PropertyValue(string name, T value)
        {
            this.Name = name;
            this.Value = value;
        }
    }

    public class PropertyValueManager<T> : PropertyValueManagerBase<T, IPropertyValue>
    {
        public PropertyValueManager(IRegisteredPropertyManager<T> registeredPropertyManager, IFactory factory) : base(registeredPropertyManager, factory)
        {

        }

        protected override IPropertyValue CreatePropertyValue<PV>(string name, PV value)
        {
            return Factory.CreatePropertyValue(name, value);
        }
    }

    public abstract class PropertyValueManagerBase<T, P> : IPropertyValueManager<T>
        where P : IPropertyValue
    {

        protected IFactory Factory { get; }
        protected readonly IRegisteredPropertyManager<T> registeredPropertyManager;
        protected IDictionary<uint, P> fieldData = new ConcurrentDictionary<uint, P>();

        public PropertyValueManagerBase(IRegisteredPropertyManager<T> registeredPropertyManager, IFactory factory)
        {
            this.registeredPropertyManager = registeredPropertyManager;
            Factory = factory;
        }

        protected abstract P CreatePropertyValue<PV>(string name, PV value);
        
        protected IRegisteredProperty<PV> GetRegisteredProperty<PV>(string name)
        {
            return registeredPropertyManager.RegisterProperty<PV>(name);
        }



        public virtual void Set<PV>(string name, PV newValue)
        {
            Set(GetRegisteredProperty<PV>(name), newValue);
        }

        public virtual void Set<PV>(IRegisteredProperty registeredProperty, PV newValue)
        {
            if (!fieldData.TryGetValue(registeredProperty.Index, out var value))
            {
                // Default(P) so that it get's marked dirty
                // Maybe it would be better to use MarkSelfModified; you know; once I write that
                fieldData[registeredProperty.Index] = value = CreatePropertyValue(registeredProperty.Name, default(PV));
            }

            IPropertyValue<PV> fd = value as IPropertyValue<PV> ?? throw new PropertyTypeMismatchException($"Property {registeredProperty.Name} is not type {typeof(P).FullName}");
            fd.Value = newValue;

        }

        public virtual void Load<PV>(string name, PV newValue)
        {
            Load(GetRegisteredProperty<PV>(name), newValue);
        }

        public virtual void Load<PV>(IRegisteredProperty registeredProperty, PV newValue)
        {
            if (!fieldData.ContainsKey(registeredProperty.Index))
            {
                // TODO Destroy and Delink to old value
            }

            fieldData[registeredProperty.Index] = CreatePropertyValue(registeredProperty.Name, newValue);

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
