using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.Core
{
    public interface IValidatePropertyValueManager<T> : IPropertyValueManager<T>
    {
        bool IsValid { get; }
        bool IsBusy { get; }

        Task WaitForRules();

    }

    public interface IValidatePropertyValue : IPropertyValue
    {
        bool IsValid { get; }
        bool IsBusy { get; }
        Task WaitForRules();
    }

    public interface IValidatePropertyValue<T> : IValidatePropertyValue, IPropertyValue<T>
    {

    }

    public class ValidatePropertyValue<T> : PropertyValue<T>, IValidatePropertyValue<T>
    {

        public virtual IValidateBase Child { get; protected set; }
        public ValidatePropertyValue(string name) : base(name) { }

        public ValidatePropertyValue(string name, T value) : base(name, value)
        {

        }

        public override T Value
        {
            get => base.Value;
            set
            {
                Child = value as IValidateBase;
                base.Value = value;
            }
        }

        public bool IsValid => (Child?.IsValid ?? true);
        public bool IsBusy => (Child?.IsBusy ?? false);

        public Task WaitForRules() { return Child?.WaitForRules() ?? Task.CompletedTask; }
    }

    public class ValidatePropertyValueManager<T> : PropertyValueManager<T>, IValidatePropertyValueManager<T>
    {
        public ValidatePropertyValueManager(IRegisteredPropertyManager<T> registeredPropertyManager, IFactory factory) : base(registeredPropertyManager, factory)
        {

        }

        public bool IsValid => !fieldData.Values.OfType<IValidatePropertyValue>().Any(_ => !_.IsValid);

        public bool IsBusy => fieldData.Values.OfType<IValidatePropertyValue>().Any(_ => _.IsBusy);

        public Task WaitForRules()
        {
            return Task.WhenAll(fieldData.Values.OfType<IValidatePropertyValue>().Select(x => x.WaitForRules()));
        }


        protected override IPropertyValue<P> CreatePropertyValue<P>(string name, P value)
        {
            return Factory.CreateValidatePropertyValue(name, value);
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
