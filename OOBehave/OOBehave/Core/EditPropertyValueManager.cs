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

        IEnumerable<string> ModifiedProperties { get; }
        void MarkSelfUnmodified();
    }

    public interface IEditPropertyValueManager<T> : IEditPropertyValueManager, IValidatePropertyValueManager<T>
    {

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

    public class EditPropertyValue<T> : ValidatePropertyValue<T>, IEditPropertyValue<T>
    {

        protected IValuesDiffer ValuesDiffer { get; }
        public EditPropertyValue(IValuesDiffer valuesDiffer, string name, T value) : base(name)
        {
            this.ValuesDiffer = valuesDiffer;
            base.Value = value;
            EditChild = value as IEditBase;
        }

        public IEditBase EditChild { get; protected set; }

        public override T Value
        {
            get => base.Value;
            set
            {
                if (ValuesDiffer.Check(base.Value, value))
                {
                    base.Value = value;
                    EditChild = value as IEditBase;
                    IsSelfModified = true && EditChild == null; // Never consider ourself modified if OOBehave object
                }
            }
        }

        public bool IsModified => IsSelfModified || (EditChild?.IsModified ?? false);
        public bool IsSelfModified { get; private set; } = false;

        public void MarkSelfUnmodified()
        {
            IsSelfModified = false;
        }
    }


    public class EditPropertyValueManager<T> : ValidatePropertyValueManagerBase<T, IEditPropertyValue>, IEditPropertyValueManager<T>
    {
        public EditPropertyValueManager(IRegisteredPropertyManager<T> registeredPropertyManager, IFactory factory) : base(registeredPropertyManager, factory)
        {

        }

        protected override IEditPropertyValue CreatePropertyValue<PV>(string name, PV value)
        {
            return Factory.CreateEditPropertyValue(name, value);
        }

        public bool IsModified => fieldData.Values.Any(p => p.IsModified);
        public bool IsSelfModified => fieldData.Values.Any(p => p.IsSelfModified);

        public IEnumerable<string> ModifiedProperties => fieldData.Values.Where(f => f.IsModified).Select(f => f.Name);

        public void MarkSelfUnmodified()
        {
            foreach (var fd in fieldData.Values)
            {
                fd.MarkSelfUnmodified();
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
