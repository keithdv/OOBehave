using OOBehave.AuthorizationRules;
using OOBehave.Core;
using OOBehave.Portal;
using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave
{
    /// <summary>
    /// Wrap the OOBehaveBase services into an interface so that 
    /// the inheriting classes don't need to list all services
    /// and services can be added
    /// </summary>
    public interface IListBaseServices<L, T>
        where T : IBase<T>
    {
        IPropertyValueManager<L> PropertyValueManager { get; }
        IReceivePortalChild<T> ReceivePortal { get; }
    }

    public class ListBaseServices<L, T> : IListBaseServices<L, T>
        where T : IBase<T>
    {

        public ListBaseServices(IPropertyValueManager<L> registeredPropertyDataManager, IReceivePortalChild<T> receivePortal)
        {
            PropertyValueManager = registeredPropertyDataManager;
            ReceivePortal = receivePortal;
        }

        public IPropertyValueManager<L> PropertyValueManager { get; }
        public IReceivePortalChild<T> ReceivePortal { get; }

    }
}
