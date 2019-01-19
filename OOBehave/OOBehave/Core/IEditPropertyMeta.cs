using OOBehave.Core;

namespace OOBehave
{
    public interface IEditPropertyMeta : IValidatePropertyMeta
    {
        bool IsModified { get; }
    }


    public class EditPropertyMeta : ValidatePropertyMeta, IEditPropertyMeta
    {
        public EditPropertyMeta(IEditPropertyValue editPropertyValue) : base(editPropertyValue)
        {
            EditPropertyValue = editPropertyValue ?? throw new System.ArgumentNullException(nameof(EditPropertyValue));
        }

        public IEditPropertyValue EditPropertyValue { get; }

        public bool IsModified => EditPropertyValue.IsModified;
    }

}

