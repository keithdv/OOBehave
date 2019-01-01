using OOBehave.Core;
using OOBehave.Portal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace OOBehave
{

    public interface IEditListBase : IValidateListBase, IEditBase, IEditMetaProperties, IPortalEditTarget
    {

    }

    public interface IEditListBase<T> : IEditListBase, IValidateListBase<T>
        where T : IEditBase
    {

    }

    public abstract class EditListBase<T> : ValidateListBase<T>, IOOBehaveObject, IEditListBase<T>
        where T : IEditBase
    {

        protected IEditPropertyValueManager EditPropertyValueManager => (IEditPropertyValueManager)base.PropertyValueManager;

        public EditListBase(IEditListBaseServices<T> services) : base(services)
        {
            
        }

        public bool IsModified => EditPropertyValueManager.IsModified || this.Any(c => c.IsModified);
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
