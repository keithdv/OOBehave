using OOBehave.AuthorizationRules;
using OOBehave.Core;
using OOBehave.Portal;
using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave
{

    public interface IValidateBaseServices : IBaseServices
    {
        IValidatePropertyValueManager ValidatePropertyValueManager { get; }

        IRuleExecute CreateRuleExecute(IValidateBase Target);

    }

    /// <summary>
    /// Wrap the OOBehaveBase services into an interface so that 
    /// the inheriting classes don't need to list all services
    /// and services can be added
    /// </summary>
    public interface IValidateBaseServices<T> : IValidateBaseServices, IBaseServices<T>
        where T : IValidateBase
    {


    }

    public class ValidateBaseServices<T> : BaseServices<T>, IValidateBaseServices<T>
        where T : ValidateBase
    {

        private IFactory Factory { get; }
        public ValidateBaseServices(IValidatePropertyValueManager<T> registeredPropertyValueManager, IRegisteredPropertyManager<T> registeredPropertyManager,
            IFactory factory) : base(registeredPropertyValueManager, registeredPropertyManager)
        {
            this.ValidatePropertyValueManager = registeredPropertyValueManager;
            this.Factory = factory;
        }

        public IValidatePropertyValueManager ValidatePropertyValueManager { get; }

        IPropertyValueManager IBaseServices.PropertyValueManager
        {
            get { return ValidatePropertyValueManager; }
        }

        public IRuleExecute CreateRuleExecute(IValidateBase target)
        {
            // This is the one catch not have generic base classes
            // Classes receive IRuleExecute instead of IRuleExecut<T>
            return Factory.CreateRuleExecute(target as T ?? throw new Exception($"Unexpected: Cannot case {target.GetType().FullName} to {typeof(T).FullName}"));
        }

    }
}
