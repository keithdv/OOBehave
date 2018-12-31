using OOBehave.AuthorizationRules;
using OOBehave.Core;
using OOBehave.Portal;
using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave
{
    /// <summary>
    /// DO NOT REGISTER IN DI CONTAINER
    /// </summary>
    public interface IEditBaseServices : IValidateBaseServices
    {
        IEditPropertyValueManager EditPropertyValueManager { get; }

    }
    /// <summary>
    /// Wrap the OOBehaveBase services into an interface so that 
    /// the inheriting classes don't need to list all services
    /// and services can be added
    /// REGISTERED IN DI CONTAINER
    /// </summary>
    public interface IEditBaseServices<T> : IEditBaseServices, IValidateBaseServices<T>
        where T : IEditBase
    {
    }

    public class EditBaseServices<T> : ValidateBaseServices<T>, IEditBaseServices<T>
        where T : EditBase
    {

        public IEditPropertyValueManager EditPropertyValueManager { get; }
        public EditBaseServices(IEditPropertyValueManager<T> registeredPropertyValueManager, IRegisteredPropertyManager<T> registeredPropertyManager,
                                        IFactory factory) : base(registeredPropertyValueManager, registeredPropertyManager, factory)
        {
            EditPropertyValueManager = registeredPropertyValueManager;
        }
    }
}
