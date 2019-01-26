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

    public interface IReadOnlyListBase<I> : IBase, IOOBehaveObject, IPortalTarget, INotifyCollectionChanged, INotifyPropertyChanged, IReadOnlyCollection<I>, IReadOnlyList<I>
        where I : IBase
    {
        new int Count { get; }
    }


    public interface IListBase : IBase, IOOBehaveObject, IPortalTarget, INotifyCollectionChanged, INotifyPropertyChanged, IEnumerable, ICollection, IList
    {

    }

    public interface IListBase<I> : IBase, IOOBehaveObject, IPortalTarget, INotifyCollectionChanged, INotifyPropertyChanged, IEnumerable<I>, ICollection<I>, IList<I>
    {
        Task<I> CreateAdd();
        Task<I> CreateAdd(params object[] criteria);
    }

    public abstract class ListBase<T, I> : ObservableCollection<I>, IOOBehaveObject, IListBase<I>, IListBase, IReadOnlyListBase<I>, IPortalTarget, IPropertyAccess, ISetParent
        where T : ListBase<T, I>
        where I : IBase
    {

        protected IPropertyValueManager<T> PropertyValueManager { get; private set; } // Private setter for Deserialization

        protected IReceivePortalChild<I> ItemPortal { get; }

        public ListBase(IListBaseServices<T, I> services)
        {
            PropertyValueManager = services.PropertyValueManager;
            ItemPortal = services.ReceivePortal;
        }

        public IBase Parent { get; protected set; }

        void ISetParent.SetParent(IBase parent)
        {
            Parent = parent;
        }

        protected IRegisteredProperty<PV> GetRegisteredProperty<PV>(string name)
        {
            return PropertyValueManager.GetRegisteredProperty<PV>(name);
        }

        protected virtual P Getter<P>([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            return ReadProperty<P>(GetRegisteredProperty<P>(propertyName));
        }

        protected virtual void Setter<P>(P value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            LoadProperty(GetRegisteredProperty<P>(propertyName), value);
        }

        protected virtual P ReadProperty<P>(IRegisteredProperty<P> property)
        {
            return PropertyValueManager.ReadProperty<P>(property);
        }

        protected virtual void LoadProperty<P>(IRegisteredProperty<P> registeredProperty, P value)
        {
            PropertyValueManager.LoadProperty(registeredProperty, value);
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

        public async Task<I> CreateAdd()
        {
            // CreateChild cannot define a scope 
            // Don't want any fallback to a new scope because the requirement is uses the parent scope
            // But here there is none - so define it
            using (ItemPortal.UsingOperationScope())
            {
                var item = await ItemPortal.CreateChild().ConfigureAwait(false);
                base.Add(item);
                return item;
            }
        }

        public async Task<I> CreateAdd(params object[] criteria)
        {
            var item = await ItemPortal.CreateChild(criteria).ConfigureAwait(false);
            base.Add(item);
            return item;
        }

        P IPropertyAccess.ReadProperty<P>(IRegisteredProperty<P> registeredProperty)
        {
            return PropertyValueManager.ReadProperty(registeredProperty);
        }

        void IPropertyAccess.SetProperty<P>(IRegisteredProperty<P> registeredProperty, P value)
        {
            PropertyValueManager.LoadProperty(registeredProperty, value);
        }

        void IPropertyAccess.LoadProperty<P>(IRegisteredProperty<P> registeredProperty, P value)
        {
            PropertyValueManager.LoadProperty(registeredProperty, value);
        }

        IPropertyValue IPropertyAccess.ReadPropertyValue(string propertyName)
        {
            return PropertyValueManager.ReadProperty(propertyName);
        }

        IPropertyValue IPropertyAccess.ReadPropertyValue(IRegisteredProperty registeredProperty)
        {
            return PropertyValueManager.ReadProperty(registeredProperty);
        }

    }

}
