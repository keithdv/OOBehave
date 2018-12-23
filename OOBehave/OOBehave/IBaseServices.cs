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
    public interface IBaseServices<T>
    {
        IRegisteredPropertyDataManager<T> RegisteredPropertyDataManager { get; }
        IRegisteredOperationManager RegisteredOperationManager { get; }
    }

    public class BaseServices<T> : IBaseServices<T>
    {

        public BaseServices(IRegisteredPropertyDataManager<T> registeredPropertyDataManager,
            IRegisteredOperationManager registeredOperationManager)
        {
            this.RegisteredPropertyDataManager = registeredPropertyDataManager;
            this.RegisteredOperationManager = registeredOperationManager;
        }

        public IRegisteredPropertyDataManager<T> RegisteredPropertyDataManager { get; }

        public IRegisteredOperationManager RegisteredOperationManager { get; }
    }
}
