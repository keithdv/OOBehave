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
    public interface IBaseServices<T> where T : IBase
    {
        IPropertyValueManager<T> PropertyValueManager { get; }
        IRegisteredPropertyManager<T> RegisteredPropertyManager { get; }
    }

    public class BaseServices<T> : IBaseServices<T>
        where T : IBase
    {

        public BaseServices(IPropertyValueManager<T> registeredPropertyDataManager, IRegisteredPropertyManager<T> registeredPropertyManager)
        {
            PropertyValueManager = registeredPropertyDataManager;
            RegisteredPropertyManager = registeredPropertyManager;
        }

        public IPropertyValueManager<T> PropertyValueManager { get; }
        public IRegisteredPropertyManager<T> RegisteredPropertyManager { get; }
    }
}
