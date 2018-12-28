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
    public interface IValidateListBaseServices<L, T> : IListBaseServices<L, T>
        where L : ValidateListBase<L, T>
        where T : IValidateBase
    {
        IValidatePropertyValueManager<L> ValidatePropertyValueManager { get; }

        IRuleExecute<L> CreateRuleExecute(L target);

    }

    public class ValidateListBaseServices<L, T> : ListBaseServices<L, T>, IValidateListBaseServices<L, T>
        where L : ValidateListBase<L, T>
        where T : IValidateBase
    {

        private IFactory Factory { get; }
        public ValidateListBaseServices(IValidatePropertyValueManager<L> registeredPropertyManager,
            IReceivePortalChild<T> portal,
            IFactory factory) : base(registeredPropertyManager, portal)
        {
            this.ValidatePropertyValueManager = registeredPropertyManager;
            this.Factory = factory;
        }

        public IValidatePropertyValueManager<L> ValidatePropertyValueManager { get; }

        IPropertyValueManager<L> IListBaseServices<L, T>.PropertyValueManager
        {
            get { return ValidatePropertyValueManager; }
        }

        public IRuleExecute<L> CreateRuleExecute(L target)
        {
            return Factory.CreateRuleExecute(target);
        }

    }
}
