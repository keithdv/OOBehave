using OOBehave.AuthorizationRules;
using OOBehave.Core;
using OOBehave.Portal;
using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave
{
    public interface IEditListBaseServices<T> : IValidateListBaseServices<T>
    where T : IEditBase
    {
        IEditPropertyValueManager EditPropertyValueManager { get; }
    }

    /// <summary>
    /// Wrap the OOBehaveBase services into an interface so that 
    /// the inheriting classes don't need to list all services
    /// and services can be added
    /// </summary>
    public interface IEditListBaseServices<L, T> : IEditListBaseServices<T>, IValidateListBaseServices<L, T>
        where L : EditListBase<T>
        where T : IEditBase
    {
    }

    public class EditListBaseServices<L, T> : ValidateListBaseServices<L, T>, IEditListBaseServices<L, T>
        where L : EditListBase<T>
        where T : IEditBase
    {

        public IEditPropertyValueManager EditPropertyValueManager { get; }

        public EditListBaseServices(IEditPropertyValueManager<L> registeredPropertyManager,
                                        IReceivePortalChild<T> receivePortalChild,
                                        IRuleExecute<L> ruleExecute) 
            : base(registeredPropertyManager, receivePortalChild, ruleExecute)
        {
            EditPropertyValueManager = registeredPropertyManager;
        }
    }
}
