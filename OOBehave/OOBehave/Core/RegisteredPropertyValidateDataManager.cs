using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOBehave.Core
{
    public interface IRegisteredPropertyValidateDataManager<T> : IRegisteredPropertyDataManager<T>
    {

    }

    internal interface IRegisteredPropertyValidateData : IRegisteredPropertyData, IValidateMetaProperties
    {

    }

    internal interface IRegisteredPropertyValidateData<T> : IRegisteredPropertyValidateData, IRegisteredPropertyData<T>
    {

    }

    internal class RegisteredPropertyValidateChild<T> : RegisteredPropertyData<T>, IRegisteredPropertyValidateData<T>
    {

        private readonly IValidateBase child;
        public RegisteredPropertyValidateChild(string name, T value) : base(name, value)
        {
            child = value as IValidateBase ?? throw new RegisteredPropertyValidateChildDataWrongTypeException($"{typeof(T).FullName} does not implement IValidateBase");
        }

        public bool IsValid => child.IsValid;

        public bool IsSelfValid => child.IsValid;

        public bool IsChild => child.IsChild;

    }

    internal class RegisteredPropertyValidateData<T> : RegisteredPropertyData<T>, IRegisteredPropertyValidateData<T>
    {

        public RegisteredPropertyValidateData(string name, T value) : base(name, value)
        {
        }

        public bool IsValid => true;

        public bool IsSelfValid => true;

        public bool IsChild => true;

    }

    public class RegisteredPropertyValidateDataManager<T> : RegisteredPropertyDataManager<T>, IRegisteredPropertyValidateDataManager<T>
    {

        public RegisteredPropertyValidateDataManager(IRegisteredPropertyManager registeredPropertyManager) : base(registeredPropertyManager)
        {
        }

        protected override IRegisteredPropertyData<P> CreateRegisteredPropertyData<P>(string name, P value)
        {
            var child = value as IValidateBase;
            if (child == null)
            {
                return new RegisteredPropertyValidateData<P>(name, value);
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
