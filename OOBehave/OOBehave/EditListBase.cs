﻿using OOBehave.Core;
using OOBehave.Portal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

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

        protected new IEditPropertyValueManager PropertyValueManager => (IEditPropertyValueManager)base.PropertyValueManager;

        protected new ISendReceivePortalChild<T> ItemPortal { get; }

        public EditListBase(IEditListBaseServices<T> services) : base(services)
        {
            this.ItemPortal = services.SendReceivePortalChild;
        }

        public bool IsModified => PropertyValueManager.IsModified || this.Any(c => c.IsModified) || IsDeleted;
        public bool IsSelfModified => PropertyValueManager.IsSelfModified || IsDeleted;
        public bool IsSavable => IsModified && IsValid && !IsBusy && !IsChild;
        public bool IsNew { get; protected set; }
        public bool IsDeleted { get; protected set; }
        public IEnumerable<string> ModifiedProperties => PropertyValueManager.ModifiedProperties;
        public bool IsChild { get; protected set; }
        protected List<T> DeletedList { get; } = new List<T>();


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

        protected async Task UpdateList()
        {
            foreach (var d in DeletedList)
            {
                await ItemPortal.UpdateChild(d);
            }

            foreach (var i in this.Where(i => i.IsModified).ToList())
            {
                await ItemPortal.UpdateChild(i);
            }
        }

        protected async Task UpdateList(object criteria)
        {
            foreach (var d in DeletedList)
            {
                await ItemPortal.UpdateChild(d, criteria);
            }

            foreach (var i in this.Where(i => i.IsModified).ToList())
            {
                await ItemPortal.UpdateChild(i, criteria);
            }
        }
    }



}
