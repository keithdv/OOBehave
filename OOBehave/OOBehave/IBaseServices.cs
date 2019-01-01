using OOBehave.AuthorizationRules;
using OOBehave.Core;
using OOBehave.Portal;
using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave
{

    // DO NOT register this in the container
    // Absolutely needs to be type specific
    public interface IBaseServices
    {
        IPropertyValueManager PropertyValueManager { get; }

        IRegisteredPropertyManager RegisteredPropertyManager { get; }
    }

    /// <summary>
    /// Wrap the OOBehaveBase services into an interface so that 
    /// the inheriting classes don't need to list all services
    /// and services can be added
    /// </summary>
    public interface IBaseServices<T> : IBaseServices where T : IBase
    {

    }

    public class BaseServices<T> : IBaseServices<T>
        where T : Base
    {

        public BaseServices(IPropertyValueManager<T> registeredPropertyDataManager, IRegisteredPropertyManager<T> registeredPropertyManager)
        {
            PropertyValueManager = registeredPropertyDataManager;
            RegisteredPropertyManager = registeredPropertyManager;
        }

        public IPropertyValueManager PropertyValueManager { get; }
        public IRegisteredPropertyManager RegisteredPropertyManager { get; }
    }
}
