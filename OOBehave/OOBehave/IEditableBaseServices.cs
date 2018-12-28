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
    /// Wrap the OOBehaveBase services into an interface so that 
    /// the inheriting classes don't need to list all services
    /// and services can be added
    /// </summary>
    public interface IEditBaseServices<T> : IValidateBaseServices<T>
    {
        IEditPropertyValueManager<T> EditPropertyValueManager { get; }
    }

    public class EditBaseServices<T> : ValidateBaseServices<T>, IEditBaseServices<T>
    {

        public IEditPropertyValueManager<T> EditPropertyValueManager { get; }
        public EditBaseServices(IEditPropertyValueManager<T> registeredPropertyManager,
                                        IFactory factory) : base(registeredPropertyManager, factory)
        {
            EditPropertyValueManager = registeredPropertyManager;
        }
    }
}
