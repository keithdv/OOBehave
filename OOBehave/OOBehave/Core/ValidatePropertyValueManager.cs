using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OOBehave.Core
{
    public interface IValidatePropertyValueManager : IPropertyValueManager
    {
        bool IsValid { get; }
        bool IsBusy { get; }
        Task CheckAllRules(CancellationToken token);
        Task WaitForRules();
        bool SetProperty<P>(IRegisteredProperty<P> registeredProperty, P newValue);

    }

    public interface IValidatePropertyValueManager<T> : IValidatePropertyValueManager, IPropertyValueManager<T>
    {
        new IValidatePropertyValue this[string propertyName] { get; }
    }

    internal interface IValidatePropertyValueInternal
    {
        bool IsValid { get; set; }
        bool IsBusy { get; set; }
        string ErrorMessage { get; set; }

    }

    public interface IValidatePropertyMeta
    {
        bool IsValid { get; }
        bool IsBusy { get; }
        string ErrorMessage { get; }
        object Value { get; }
    }

    public interface IValidatePropertyValue : IPropertyValue
    {
        bool IsValid { get; }
        bool IsBusy { get; }
        string ErrorMessage { get; }
        Task CheckAllRules(CancellationToken token);
        Task WaitForRules();
    }

    public interface IValidatePropertyValue<T> : IValidatePropertyValue, IPropertyValue<T>
    {

    }

    public class ValidatePropertyValue<T> : PropertyValue<T>, IValidatePropertyValue<T>, IValidatePropertyValueInternal
    {

        public virtual IValidateBase Child { get; protected set; }

        public ValidatePropertyValue(string name, T value) : base(name, value)
        {
            Child = value as IValidateBase;
        }

        public override T Value
        {
            get => base.Value;
            set
            {
                OnValueChanging(base.Value, value);
                base.Value = value;
                OnValueChanged(base.Value);
            }
        }

        /// <summary>
        /// Before any checks on if the value actually changed
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected virtual void OnValueChanging(T oldValue, T newValue)
        {

        }
        protected virtual void OnValueChanged(T newValue)
        {
            Child = newValue as IValidateBase;
        }

        private bool isValid = true;

        public bool IsValid
        {
            get { return (Child?.IsValid) ?? isValid; }
            set { isValid = value; }
        }

        private bool isBusy = false;

        public bool IsBusy
        {
            get { return (Child?.IsBusy) ?? isBusy; }
            set { isBusy = value; }
        }

        public string ErrorMessage { get; set; }

        public Task WaitForRules() { return Child?.WaitForRules() ?? Task.CompletedTask; }
        public Task CheckAllRules(CancellationToken token) { return Child?.CheckAllRules(token); }

    }

    public class ValidatePropertyValueManager<T> : ValidatePropertyValueManagerBase<T, IValidatePropertyValue>
        where T : IBase
    {
        public ValidatePropertyValueManager(IRegisteredPropertyManager<T> registeredPropertyManager, IFactory factory, IValuesDiffer valuesDiffer) : base(registeredPropertyManager, factory, valuesDiffer)
        {

        }

        protected override IValidatePropertyValue CreatePropertyValue<PV>(IRegisteredProperty<PV> registeredProperty, PV value)
        {
            return Factory.CreateValidatePropertyValue(registeredProperty, value);
        }
    }

    public abstract class ValidatePropertyValueManagerBase<T, P> : PropertyValueManagerBase<T, P>, IValidatePropertyValueManager<T>
        where T : IBase
        where P : IValidatePropertyValue
    {
        public ValidatePropertyValueManagerBase(IRegisteredPropertyManager<T> registeredPropertyManager, IFactory factory, IValuesDiffer valuesDiffer) : base(registeredPropertyManager, factory)
        {
            ValuesDiffer = valuesDiffer;
        }

        public bool IsValid => !fieldData.Values.Any(_ => !_.IsValid);

        public bool IsBusy => fieldData.Values.Any(_ => _.IsBusy);

        public IValuesDiffer ValuesDiffer { get; }

        public Task WaitForRules()
        {
            return Task.WhenAll(fieldData.Values.Select(x => x.WaitForRules()));
        }

        public new IValidatePropertyValue this[string propertyName]
        {
            get
            {
                return base[propertyName] as IValidatePropertyValue;
            }
        }

        public Task CheckAllRules(CancellationToken token)
        {
            var tasks = fieldData.Values.Select(x => x.CheckAllRules(token)).ToList();
            return Task.WhenAll(tasks.Where(t => t != null));
        }

        public virtual bool SetProperty<PV>(string name, PV newValue)
        {
            return SetProperty(GetRegisteredProperty<PV>(name), newValue);
        }

        public virtual bool SetProperty<PV>(IRegisteredProperty<PV> registeredProperty, PV newValue)
        {
            if (!fieldData.TryGetValue(registeredProperty.Index, out var value))
            {
                // Default(P) so that it get's marked dirty
                // Maybe it would be better to use MarkSelfModified; you know; once I write that
                fieldData[registeredProperty.Index] = value = CreatePropertyValue(registeredProperty, default(PV));
            }

            IPropertyValue<PV> fd = value as IPropertyValue<PV> ?? throw new PropertyTypeMismatchException($"Property {registeredProperty.Name} is not type {typeof(PV).FullName}");

            if (ValuesDiffer.Check(fd.Value, newValue))
            {
                fd.Value = newValue;
                SetParent(newValue);
                return true;
            }
            else
            {
                return false;
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

