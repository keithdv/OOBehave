using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOBehave.Core
{
    public interface IValidatePropertyValueManager<T> : IPropertyValueManager<T>
    {
        bool IsValid { get; }
    }

    internal interface IValidatePropertyValue : IPropertyValue
    {
        bool IsValid { get; }
    }

    internal interface IValidatePropertyValue<T> : IValidatePropertyValue, IPropertyValue<T>
    {

    }

    internal class RegisteredPropertyValidateChild<T> : PropertyValue<T>, IValidatePropertyValue<T>
    {

        private readonly IValidateBase child;
        public RegisteredPropertyValidateChild(string name, T value) : base(name, value)
        {
            child = value as IValidateBase ?? throw new RegisteredPropertyValidateChildDataWrongTypeException($"{typeof(T).FullName} does not implement IValidateBase");
        }

        public bool IsValid => child.IsValid;

    }

    internal class ValidatePropertyValue<T> : PropertyValue<T>, IValidatePropertyValue<T>
    {

        public ValidatePropertyValue(string name, T value) : base(name, value)
        {
        }

        public bool IsValid => true;

    }

    public class ValidatePropertyValueManager<T> : PropertyValueManager<T>, IValidatePropertyValueManager<T>
    {

        public ValidatePropertyValueManager(IRegisteredPropertyManager<T> registeredPropertyManager) : base(registeredPropertyManager)
        {
        }

        public bool IsValid
        {
            get
            {
                return !fieldData.Values.Cast<IValidatePropertyValue>().Any(_ => !_.IsValid);
            }
        }

        protected override IPropertyValue<P> CreatePropertyValue<P>(string name, P value)
        {
            var child = value as IValidateBase;
            if (child == null)
            {
                return new ValidatePropertyValue<P>(name, value);
            }
            else
            {
                return new RegisteredPropertyValidateChild<P>(name, value);
            }
        }
    }


    [Serializable]
    public class RegisteredPropertyValidateChildDataWrongTypeException : Exception
    {
        public RegisteredPropertyValidateChildDataWrongTypeException() { }
        public RegisteredPropertyValidateChildDataWrongTypeException(string message) : base(message) { }
        public RegisteredPropertyValidateChildDataWrongTypeException(string message, Exception inner) : base(message, inner) { }
        protected RegisteredPropertyValidateChildDataWrongTypeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

}
