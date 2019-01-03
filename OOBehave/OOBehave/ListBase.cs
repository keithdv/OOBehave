using OOBehave.Attributes;
using OOBehave.AuthorizationRules;
using OOBehave.Core;
using OOBehave.Portal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace OOBehave
{
    public interface IListBase : IBase, IOOBehaveObject, IPortalTarget, IEnumerable, ICollection, IList
    {

    }

    public interface IListBase<T> : IListBase, ICollection<T>, IList<T>, IReadOnlyCollection<T>, IReadOnlyList<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        Task<T> CreateAdd();
        Task<T> CreateAdd(object criteria);
        new int Count { get; }
    }

    public abstract class ListBase<T> : ObservableCollection<T>, IOOBehaveObject, IListBase<T>, IPortalTarget, IPropertyAccess, ISetParent
        where T : IBase
    {

        protected IPropertyValueManager PropertyValueManager { get; private set; } // Private setter for Deserialization

        protected IReceivePortalChild<T> ItemPortal { get; }

        public ListBase(IListBaseServices<T> services)
        {
            PropertyValueManager = services.PropertyValueManager;
            ItemPortal = services.ReceivePortal;
        }

        public IBase Parent { get; protected set; }

        void ISetParent.SetParent(IBase parent)
        {
            Parent = parent;
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

        public async Task<T> CreateAdd()
        {
            var item = await ItemPortal.CreateChild();
            base.Add(item);
            return item;
        }

        public async Task<T> CreateAdd(object criteria)
        {
            var item = await ItemPortal.CreateChild(criteria);
            base.Add(item);
            return item;
        }

        P IPropertyAccess.ReadProperty<P>(IRegisteredProperty<P> registeredProperty)
        {
            return PropertyValueManager.Read(registeredProperty);
        }

        void IPropertyAccess.SetProperty<P>(IRegisteredProperty<P> registeredProperty, P value)
        {
            PropertyValueManager.Set(registeredProperty, value);
        }
    }

}
