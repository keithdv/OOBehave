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
        IRuleExecute CreateRuleExecute(IValidateBase target);

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

        private IFactory Factory { get; }
        public ValidateListBaseServices(IValidatePropertyValueManager<L> registeredPropertyManager,
            IReceivePortalChild<T> portal,
            IFactory factory) : base(registeredPropertyManager, portal)
        {
            this.ValidatePropertyValueManager = registeredPropertyManager;
            this.Factory = factory;
        }

        public IValidatePropertyValueManager ValidatePropertyValueManager { get; }

        IPropertyValueManager IListBaseServices<T>.PropertyValueManager
        {
            get { return ValidatePropertyValueManager; }
        }

        public IRuleExecute CreateRuleExecute(IValidateBase target)
        {
            // This is the one catch not have generic base classes
            // Classes receive IRuleExecute instead of IRuleExecut<T>
            return Factory.CreateRuleExecute(target as L ?? throw new Exception($"Unexpected: Cannot cast {target.GetType().FullName} to {typeof(T).FullName}"));
        }

    }
}
