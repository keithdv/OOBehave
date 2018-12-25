using OOBehave.Core;
using OOBehave.Portal;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace OOBehave
{

    public interface IEditBase : IValidateBase, IEditMetaProperties, IPortalEditTarget
    {
        IEnumerable<string> ModifiedProperties { get; }
        bool IsChild { get; }
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
        public bool IsSavable => IsModified && IsValid && !IsBusy && !IsChild;
        public bool IsNew { get; protected set; }
        public bool IsDeleted => throw new NotImplementedException();
        public IEnumerable<string> ModifiedProperties => EditPropertyValueManager.ModifiedProperties;
        public bool IsChild { get; protected set; }

        protected virtual void MarkAsChild()
        {
            IsChild = true;
        }

        void IPortalEditTarget.MarkAsChild()
        {
            MarkAsChild();
        }

        protected virtual void MarkUnmodified()
        {
            EditPropertyValueManager.MarkSelfUnmodified();
        }

        void IPortalEditTarget.MarkUnmodified()
        {
            MarkUnmodified();
        }

        protected virtual void MarkNew()
        {
            IsNew = true;
        }

        void IPortalEditTarget.MarkNew()
        {
            MarkNew();
        }

        protected virtual void MarkOld()
        {
            IsNew = false;
        }

        void IPortalEditTarget.MarkOld()
        {
            MarkOld();
        }
    }



}
