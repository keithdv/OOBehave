using OOBehave.Attributes;
using OOBehave.Core;
using OOBehave.Portal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace OOBehave
{

    public abstract class EditBase<T> : ValidateBase<T>, IOOBehaveObject, IEditBase
        where T : EditBase<T>
    {
        [PortalDataMember]
        protected new IEditPropertyValueManager<T> PropertyValueManager => (IEditPropertyValueManager<T>)base.PropertyValueManager;

        public EditBase(IEditBaseServices<T> services) : base(services)
        {
            SendReceivePortal = services.SendReceivePortal;
        }

        public bool IsModified => PropertyValueManager.IsModified || IsDeleted || IsNew;
        public bool IsSelfModified => PropertyValueManager.IsSelfModified || IsDeleted;
        public bool IsSavable => IsModified && IsValid && !IsBusy && !IsChild;
        [PortalDataMember]
        public bool IsNew { get; protected set; }
        [PortalDataMember]
        public bool IsDeleted { get; protected set; }
        public IEnumerable<string> ModifiedProperties => PropertyValueManager.ModifiedProperties;
        [PortalDataMember]
        public bool IsChild { get; protected set; }
        protected ISendReceivePortal<T> SendReceivePortal { get; }

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
            // TODO : What if busy??
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

        void IPortalEditTarget.MarkDeleted()
        {
            MarkDeleted();
        }

        public void Delete()
        {
            MarkDeleted();
        }

        public virtual async Task Save()
        {
            if (!IsSavable)
            {
                if (IsChild)
                {
                    throw new Exception("Child objects cannot be saved");
                }
                if (!IsValid)
                {
                    throw new Exception("Object is not valid and cannot be saved.");
                }
                if (!IsModified)
                {
                    throw new Exception("Object has not been modified.");
                }
                if (IsBusy)
                {
                    throw new Exception("Object is busy and cannot be saved.");
                }
            }

            await SendReceivePortal.Update((T) this);

        }
    }



}
