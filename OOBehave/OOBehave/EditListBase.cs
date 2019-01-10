using OOBehave.Core;
using OOBehave.Portal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace OOBehave
{

    public interface IEditListBase : IValidateListBase, IEditBase, IEditMetaProperties, IPortalEditTarget
    {
    }

    public interface IEditListBase<I> : IValidateListBase<I>, IEditBase, IEditMetaProperties, IPortalEditTarget
        where I : IEditBase
    {
        new void RemoveAt(int index);

    }

    public abstract class EditListBase<T, I> : ValidateListBase<T, I>, IOOBehaveObject, IEditListBase<I>, IEditListBase
        where T : EditListBase<T, I>
        where I : IEditBase
    {

        protected new IEditPropertyValueManager<T> PropertyValueManager => (IEditPropertyValueManager<T>)base.PropertyValueManager;

        protected new ISendReceivePortalChild<I> ItemPortal { get; }
        public ISendReceivePortal<T> SendReceivePortal { get; }

        public EditListBase(IEditListBaseServices<T, I> services) : base(services)
        {
            this.ItemPortal = services.SendReceivePortalChild;
            this.SendReceivePortal = services.SendReceivePortal;
        }

        public bool IsModified => PropertyValueManager.IsModified || IsNew || this.Any(c => c.IsModified) || IsDeleted || DeletedList.Any();
        public bool IsSelfModified => PropertyValueManager.IsSelfModified || IsDeleted;
        public bool IsSavable => IsModified && IsValid && !IsBusy && !IsChild;
        public bool IsNew { get; protected set; }
        public bool IsDeleted { get; protected set; }
        public IEnumerable<string> ModifiedProperties => PropertyValueManager.ModifiedProperties;
        public bool IsChild { get; protected set; }
        protected List<I> DeletedList { get; } = new List<I>();


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
            // TODO
            // THis concept is a little blurry
            // I suppose I should delete all of my children??
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


        protected override void RemoveItem(int index)
        {
            var item = this[index];
            if (!item.IsNew)
            {
                item.Delete();
                DeletedList.Add(item);
            }

            base.RemoveItem(index);
        }


        protected async Task UpdateList()
        {
            foreach (var d in DeletedList)
            {
                await ItemPortal.UpdateChild(d);
            }

            DeletedList.Clear();

            foreach (var i in this.Where(i => i.IsModified).ToList())
            {
                await ItemPortal.UpdateChild(i);
            }
        }

        protected async Task UpdateList(params object[] criteria)
        {
            foreach (var d in DeletedList)
            {
                await ItemPortal.UpdateChild(d, criteria);
            }

            DeletedList.Clear();

            foreach (var i in this.Where(i => i.IsModified).ToList())
            {
                await ItemPortal.UpdateChild(i, criteria);
            }
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
            }

            await SendReceivePortal.Update((T)this);

        }

        [Update]
        [UpdateChild]
        protected virtual async Task Update()
        {
            if (IsSelfModified)
            {
                throw new Exception($"{typeof(T).FullName} is modified you must override and define Update().");
            }
            await UpdateList();
        }
    }



}
