using OOBehave.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace OOBehave
{

    public interface IEditBase : IValidateBase, IEditMetaProperties
    {
        IEnumerable<string> ModifiedProperties { get; }
    }

    public interface IEditBase<T> : IEditBase, IValidateBase<T>
    {

    }

    public abstract class EditBase<T> : ValidateBase<T>, IOOBehaveObject<T>, IEditBase<T>
        where T : EditBase<T>
    {

        protected IEditPropertyValueManager<T> EditPropertyValueManager { get; }

        public EditBase(IEditableBaseServices<T> services) : base(services)
        {
            EditPropertyValueManager = services.EditPropertyValueManager;
        }


        public bool IsModified => EditPropertyValueManager.IsModified;
        public bool IsSelfModified => EditPropertyValueManager.IsSelfModified;
        public bool IsSavable => (IsModified || IsNew) && IsValid && !IsBusy;
        public bool IsNew { get; protected set; }
        public bool IsDeleted => throw new NotImplementedException();
        public IEnumerable<string> ModifiedProperties => EditPropertyValueManager.ModifiedProperties;

        protected override void MarkClean()
        {
            base.MarkClean();
            EditPropertyValueManager.MarkClean();
        }

        protected override void MarkNew()
        {
            base.MarkNew();
            IsNew = true;
        }

        protected override void MarkOld()
        {
            base.MarkOld();
            IsNew = false;
        }
    }



}
