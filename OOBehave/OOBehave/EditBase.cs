using OOBehave.Attributes;
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

        void Delete();
    }

    public abstract class EditBase : ValidateBase, IOOBehaveObject, IEditBase
    {
        [PortalDataMember]
        protected new IEditPropertyValueManager PropertyValueManager => (IEditPropertyValueManager) base.PropertyValueManager;

        public EditBase(IEditBaseServices services) : base(services)
        {
        }

        public bool IsModified => PropertyValueManager.IsModified || IsDeleted;
        public bool IsSelfModified => PropertyValueManager.IsSelfModified || IsDeleted;
        public bool IsSavable => IsModified && IsValid && !IsBusy && !IsChild;
        [PortalDataMember]
        public bool IsNew { get; protected set; }
        [PortalDataMember]
        public bool IsDeleted { get; protected set; }
        public IEnumerable<string> ModifiedProperties => PropertyValueManager.ModifiedProperties;
        [PortalDataMember]
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
            PropertyValueManager.MarkSelfUnmodified();
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

        protected virtual void MarkDeleted()
        {
            IsDeleted = true;
        }

        public void Delete()
        {
            MarkDeleted();
        }


    }



}
