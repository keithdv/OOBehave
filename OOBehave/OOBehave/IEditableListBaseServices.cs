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
    public interface IEditListBaseServices<L, T> : IValidateListBaseServices<L, T>
        where T : IEditBase
    {
        IEditPropertyValueManager<L> EditPropertyValueManager { get; }
    }

    public class EditListBaseServices<L, T> : ValidateListBaseServices<L, T>, IEditListBaseServices<L, T>
        where T : IEditBase
    {

        public IEditPropertyValueManager<L> EditPropertyValueManager { get; }
        public EditListBaseServices(IEditPropertyValueManager<L> registeredPropertyManager,
                                        IReceivePortalChild<T> receivePortalChild,
                                        IFactory factory) : base(registeredPropertyManager, receivePortalChild, factory)
        {
            EditPropertyValueManager = registeredPropertyManager;
        }
    }
}
