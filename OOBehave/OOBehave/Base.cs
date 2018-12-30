using OOBehave.AuthorizationRules;
using OOBehave.Core;
using OOBehave.Portal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace OOBehave
{

    internal interface IPropertyAccess
    {
        P ReadProperty<P>(IRegisteredProperty<P> registeredProperty);
        void SetProperty<P>(IRegisteredProperty<P> registeredProperty, P value);
    }

    public interface IBase : IOOBehaveObject, IPortalTarget
    {
        /// <summary>
        /// Stop events, rules and ismodified
        /// Only affects the Setter method
        /// Not SetProperty or LoadProperty
        /// </summary>
        bool IsStopped { get; }

    }

    [DataContract]
    public abstract class Base : IOOBehaveObject, IBase, IPortalTarget, IPropertyAccess
    {

        [DataMember]
        protected IPropertyValueManager PropertyValueManager { get; }



        public Base(IBaseServices services)
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
            return Task.FromResult<IDisposable>(new Core.Stopped(this));
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

        P IPropertyAccess.ReadProperty<P>(IRegisteredProperty<P> registeredProperty)
        {
            return PropertyValueManager.Read(registeredProperty);
        }

        void IPropertyAccess.SetProperty<P>(IRegisteredProperty<P> registeredProperty, P value)
        {
            PropertyValueManager.Load(registeredProperty, value);
        }

        [OnSerializing]
        private void OnSerializing(StreamingContext context)
        {

        }

        [OnDeserializing]
        private void OnDeserializing(StreamingContext context)
        {

        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {

        }
    }

}
