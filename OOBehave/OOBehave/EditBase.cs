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
    }

    public abstract class EditBase : ValidateBase, IOOBehaveObject, IEditBase
    {

        protected IEditPropertyValueManager EditPropertyValueManager { get; }

        public EditBase(IEditBaseServices services) : base(services)
        {
            EditPropertyValueManager = services.EditPropertyValueManager;
        }


        public bool IsModified => EditPropertyValueManager.IsModified;
        public bool IsSelfModified => EditPropertyValueManager.IsSelfModified;
        public bool IsSavable => IsModified && IsValid && !IsBusy && !IsChild;
        [PortalDataMember]
        public bool IsNew { get; protected set; }
        public bool IsDeleted => throw new NotImplementedException();
        public IEnumerable<string> ModifiedProperties => EditPropertyValueManager.ModifiedProperties;
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
