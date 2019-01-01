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
    public interface IValidateListBaseServices<T> : IListBaseServices<T>
        where T : IValidateBase
    {
        IValidatePropertyValueManager ValidatePropertyValueManager { get; }
        IRuleExecute RuleExecute { get; }

    }

    /// <summary>
    /// Wrap the OOBehaveBase services into an interface so that 
    /// the inheriting classes don't need to list all services
    /// and services can be added
    /// </summary>
    public interface IValidateListBaseServices<L, T> : IValidateListBaseServices<T>, IListBaseServices<L, T>
        where L : ValidateListBase<T>
        where T : IValidateBase
    {


    }

    public class ValidateListBaseServices<L, T> : ListBaseServices<L, T>, IValidateListBaseServices<L, T>
        where L : ValidateListBase<T>
        where T : IValidateBase
    {

        public ValidateListBaseServices(IValidatePropertyValueManager<L> registeredPropertyManager,
            IReceivePortalChild<T> portal,
            IRuleExecute<L> ruleExecute) : base(registeredPropertyManager, portal)
        {
            this.ValidatePropertyValueManager = registeredPropertyManager;
            RuleExecute = ruleExecute;
        }

        public IValidatePropertyValueManager ValidatePropertyValueManager { get; }
        public IRuleExecute RuleExecute { get; }

        IPropertyValueManager IListBaseServices<T>.PropertyValueManager
        {
            get { return ValidatePropertyValueManager; }
        }


    }
}
