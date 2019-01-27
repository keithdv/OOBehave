﻿using OOBehave.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.Core
{
    public interface IEditPropertyValueManager : IValidatePropertyValueManager
    {
        bool IsModified { get; }
        bool IsSelfModified { get; }

        IReadOnlyList<string> ModifiedProperties { get; }
        void MarkSelfUnmodified();
    }

    public interface IEditPropertyValueManager<T> : IEditPropertyValueManager, IValidatePropertyValueManager<T>
    {
        new IEditPropertyValue this[string propertyName] { get; }
    }

    public interface IEditPropertyValue : IValidatePropertyValue
    {
        bool IsModified { get; }
        bool IsSelfModified { get; }
        void MarkSelfUnmodified();
    }

    public interface IEditPropertyValue<T> : IEditPropertyValue, IValidatePropertyValue<T>
    {

    }

    [PortalDataContract]
    public class EditPropertyValue<T> : ValidatePropertyValue<T>, IEditPropertyValue<T>
    {


        private bool initialValue = true;
        public EditPropertyValue(string name, T value) : base(name, value)
        {
            EditChild = value as IEditBase;
            initialValue = false;
        }

        public IEditBase EditChild { get; protected set; }

        protected override void OnValueChanged(T newValue)
        {
            base.OnValueChanged(newValue);
            EditChild = newValue as IEditBase;
            if (!initialValue)
            {
                IsSelfModified = true && EditChild == null; // Never consider ourself modified if OOBehave object
            }
        }

        public bool IsModified => IsSelfModified || (EditChild?.IsModified ?? false);

        [PortalDataMember]
        public bool IsSelfModified { get; private set; } = false;

        public void MarkSelfUnmodified()
        {
            IsSelfModified = false;
        }
    }

    public delegate IEditPropertyValue CreateEditPropertyValue(IRegisteredProperty property, object value);

    public class EditPropertyValueManager<T> : ValidatePropertyValueManagerBase<T, IEditPropertyValue>, IEditPropertyValueManager<T>
        where T : IBase
    {
        public EditPropertyValueManager(IRegisteredPropertyManager<T> registeredPropertyManager, IValuesDiffer valuesDiffer, CreateEditPropertyValue createEditPropertyValue) : base(registeredPropertyManager, valuesDiffer)
        {
            CreateEditPropertyValue = createEditPropertyValue;
        }

        protected override IEditPropertyValue CreatePropertyValue(IRegisteredProperty registeredProperty, object value)
        {
            return CreateEditPropertyValue(registeredProperty, value);
        }

        public bool IsModified => fieldData.Values.Any(p => p.IsModified);
        public bool IsSelfModified => fieldData.Values.Any(p => p.IsSelfModified);

        public IReadOnlyList<string> ModifiedProperties => fieldData.Values.Where(f => f.IsModified).Select(f => f.Name).ToList().AsReadOnly();

        public CreateEditPropertyValue CreateEditPropertyValue { get; }

        public void MarkSelfUnmodified()
        {
            foreach (var fd in fieldData.Values)
            {
                fd.MarkSelfUnmodified();
            }
        }

        public new IEditPropertyValue this[string propertyName]
        {
            get
            {
                return base[propertyName] as IEditPropertyValue;
            }
        }
    }


    [Serializable]
    public class RegisteredPropertyEditChildDataWrongTypeException : Exception
    {
        public RegisteredPropertyEditChildDataWrongTypeException() { }
        public RegisteredPropertyEditChildDataWrongTypeException(string message) : base(message) { }
        public RegisteredPropertyEditChildDataWrongTypeException(string message, Exception inner) : base(message, inner) { }
        protected RegisteredPropertyEditChildDataWrongTypeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

}
