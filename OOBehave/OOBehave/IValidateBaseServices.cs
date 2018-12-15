using OOBehave.Core;
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
    public interface IValidateBaseServices<T> : IBaseServices<T>
    {
        IRegisteredPropertyValidateDataManager<T> RegisteredPropertyValidateDataManager { get; }
        IRegisteredRuleManager RuleManager { get; }

    }

    public class ValidateBaseServices<T> : BaseServices<T>, IValidateBaseServices<T>
    {

        public ValidateBaseServices(IRegisteredPropertyValidateDataManager<T> registeredPropertyManager, IRegisteredRuleManager ruleManager) : base(registeredPropertyManager)
        {
            this.RegisteredPropertyValidateDataManager = registeredPropertyManager;
            this.RuleManager = ruleManager;
        }

        public IRegisteredPropertyValidateDataManager<T> RegisteredPropertyValidateDataManager { get; }

        IRegisteredPropertyDataManager<T> IBaseServices<T>.RegisteredPropertyDataManager
        {
            get { return RegisteredPropertyValidateDataManager; }
        }

        public IRegisteredRuleManager RuleManager { get; }


    }
}
