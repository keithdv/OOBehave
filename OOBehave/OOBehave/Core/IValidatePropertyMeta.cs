using OOBehave.Core;

namespace OOBehave
{
    public interface IValidatePropertyMeta
    {
        bool IsValid { get; }
        bool IsBusy { get; }
        string ErrorMessage { get; }
        object Value { get; }
    }


    public class ValidatePropertyMeta : IValidatePropertyMeta
    {
        public ValidatePropertyMeta(IValidatePropertyValue validatePropertyValue)
        {
            ValidatePropertyValue = validatePropertyValue ?? throw new System.ArgumentNullException(nameof(validatePropertyValue));
        }

        public IValidatePropertyValue ValidatePropertyValue { get; }

        public bool IsValid => ValidatePropertyValue.IsValid;

        public bool IsBusy => ValidatePropertyValue.IsBusy;

        public string ErrorMessage => ValidatePropertyValue.ErrorMessage;

        public object Value => ValidatePropertyValue.Value;
    }

}

