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
        IPropertyValueManager<T> PropertyValueManager { get; }
    }

    public class BaseServices<T> : IBaseServices<T>
    {

        public BaseServices(IPropertyValueManager<T> registeredPropertyDataManager)
        {
            this.PropertyValueManager = registeredPropertyDataManager;
        }

        public IPropertyValueManager<T> PropertyValueManager { get; }

    }
}
