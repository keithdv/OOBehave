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
            PropertyIsModified = new EditPropertyMetaByName<bool>(this, p => p.IsModified);
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

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (!(e is MetaPropertyChangedEventArgs))
            {
                PropertyHasChanged(nameof(IsModified), true);
                PropertyHasChanged(nameof(IsSelfModified), true);
                PropertyHasChanged(nameof(PropertyIsModified), true);
            }
        }

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

            await SendReceivePortal.Update((T)this).ConfigureAwait(false);

        }


        public new IEditPropertyMeta this[string propertyName]
        {
            get
            {
                var pv = PropertyValueManager[propertyName] ?? throw new ArgumentNullException(propertyName);

                return new EditPropertyMeta(pv);
            }
        }


        public EditPropertyMetaByName<bool> PropertyIsModified { get; }
    }

    public class EditPropertyMetaByName<R>
    {
        public EditPropertyMetaByName(IEditBase target, Func<IEditPropertyMeta, R> toReturn)
        {
            Target = target;
            TranslateFunc = toReturn;
        }

        private IEditBase Target { get; }
        private Func<IEditPropertyMeta, R> TranslateFunc { get; }

        public R this[string propertyName]
        {
            get
            {
                return TranslateFunc(Target[propertyName]);
            }
        }
    }

}
