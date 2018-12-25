﻿using OOBehave.AuthorizationRules;
using OOBehave.Core;
using OOBehave.Portal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace OOBehave
{
    public interface IBase : IOOBehaveObject, IPortalTarget
    {
        /// <summary>
        /// Stop events, rules and ismodified
        /// Only affects the Setter method
        /// Not SetProperty or LoadProperty
        /// </summary>
        bool IsStopped { get; }

        bool IsChild { get; }

    }

    public interface IBase<T> : IBase
    {

    }

    public abstract class Base<T> : IOOBehaveObject<T>, IBase<T>, IPortalTarget
        where T : Base<T>
    {

        protected IPropertyValueManager<T> PropertyValueManager { get; }
        public bool IsChild { get; protected set; }


        public Base(IBaseServices<T> services)
        {
            PropertyValueManager = services.PropertyValueManager;
        }

        protected virtual P Getter<P>([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            return ReadProperty<P>(propertyName);
        }

        protected virtual void Setter<P>(P value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            LoadProperty(propertyName, value);
        }

        protected virtual P ReadProperty<P>(string propertyName)
        {
            return PropertyValueManager.Read<P>(propertyName);
        }

        protected virtual void LoadProperty<P>(string propertyName, P value)
        {
            PropertyValueManager.Load(propertyName, value);
        }

        public bool IsStopped { get; protected set; }

        public virtual Task<IDisposable> StopAllActions()
        {
            if (IsStopped) { return Task.FromResult<IDisposable>(null); } // You are a nested using; You get nothing!!
            IsStopped = true;
            return Task.FromResult<IDisposable>(new Stopped(this));
        }

        public void StartAllActions()
        {
            if (IsStopped)
            {
                IsStopped = false;
            }
        }

        Task<IDisposable> IPortalTarget.StopAllActions()
        {
            return StopAllActions();
        }

        void IPortalTarget.StartAllActions()
        {
            StartAllActions();
        }

        protected virtual void MarkAsChild()
        {
            IsChild = true;
        }

        void IPortalTarget.MarkAsChild()
        {
            MarkAsChild();
        }

        protected virtual void MarkClean()
        {
            // This is an EditBase concept
        }

        void IPortalTarget.MarkClean()
        {
            MarkClean();
        }

        protected virtual void MarkNew()
        {
            // This is an EditBase concept
        }

        void IPortalTarget.MarkNew()
        {
            MarkNew();
        }

        protected virtual void MarkOld()
        {
            // This is an EditBase concept
        }

        void IPortalTarget.MarkOld()
        {
            MarkOld();
        }


        protected class Stopped : IDisposable
        {
            Base<T> Target { get; }
            public Stopped(Base<T> target)
            {
                this.Target = target;
            }

            public void Dispose()
            {
                Target.StartAllActions();
            }
        }

    }

}
