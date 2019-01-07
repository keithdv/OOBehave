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
    public interface IEditListBaseServices<T, I> : IValidateListBaseServices<T, I>
        where T : EditListBase<T, I>
        where I : IEditBase
    {
        IEditPropertyValueManager<T> EditPropertyValueManager { get; }
        ISendReceivePortalChild<I> SendReceivePortalChild { get; }
        ISendReceivePortal<T> SendReceivePortal { get; }
    }

    public class EditListBaseServices<T, I> : ValidateListBaseServices<T, I>, IEditListBaseServices<T, I>
        where T : EditListBase<T, I>
        where I : IEditBase
    {

        public IEditPropertyValueManager<T> EditPropertyValueManager { get; }
        public ISendReceivePortalChild<I> SendReceivePortalChild { get; }
        public ISendReceivePortal<T> SendReceivePortal { get; }

        public EditListBaseServices(IEditPropertyValueManager<T> registeredPropertyManager,
                                        ISendReceivePortalChild<I> sendReceivePortalChild,
                                        ISendReceivePortal<T> sendReceivePortal,
                                        IRuleExecute<T> ruleExecute)
            : base(registeredPropertyManager, sendReceivePortalChild, ruleExecute)
        {
            EditPropertyValueManager = registeredPropertyManager;
            SendReceivePortalChild = sendReceivePortalChild;
            SendReceivePortal = sendReceivePortal;
        }
    }
}
