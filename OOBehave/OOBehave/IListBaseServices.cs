using OOBehave.AuthorizationRules;
using OOBehave.Core;
using OOBehave.Portal;
using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave
{
    public interface IListBaseServices<T>
        where T : IBase
    {
        IPropertyValueManager PropertyValueManager { get; }
        IReceivePortalChild<T> ReceivePortal { get; }
    }


    /// <summary>
    /// Wrap the OOBehaveBase services into an interface so that 
    /// the inheriting classes don't need to list all services
    /// and services can be added
    /// </summary>
    public interface IListBaseServices<L, T> : IListBaseServices<T>
        where L : ListBase<T>
        where T : IBase
    {
    }

    public class ListBaseServices<L, T> : IListBaseServices<L, T>
        where L : ListBase<T>
        where T : IBase
    {

        public ListBaseServices(IPropertyValueManager<L> registeredPropertyDataManager, IReceivePortalChild<T> receivePortal)
        {
            PropertyValueManager = registeredPropertyDataManager;
            ReceivePortal = receivePortal;
        }

        public IPropertyValueManager PropertyValueManager { get; }
        public IReceivePortalChild<T> ReceivePortal { get; }

    }
}
