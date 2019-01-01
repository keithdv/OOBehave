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

        IRuleExecute RuleExecute { get; }

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

        public ValidateBaseServices(IValidatePropertyValueManager<T> registeredPropertyValueManager, IRegisteredPropertyManager<T> registeredPropertyManager, IRuleExecute<T> ruleExecute)
            : base(registeredPropertyValueManager, registeredPropertyManager)
        {
            this.ValidatePropertyValueManager = registeredPropertyValueManager;
            RuleExecute = ruleExecute;

        }

        public IValidatePropertyValueManager ValidatePropertyValueManager { get; }
        public IRuleExecute RuleExecute { get; }

        IPropertyValueManager IBaseServices.PropertyValueManager
        {
            get { return ValidatePropertyValueManager; }
        }

    }
}
