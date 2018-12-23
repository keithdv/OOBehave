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
    public interface IValidateBaseServices<T> : IBaseServices<T>
    {
        IRegisteredPropertyValidateDataManager<T> RegisteredPropertyValidateDataManager { get; }

        IRuleExecute<T> CreateRuleExecute(T target);

    }

    public class ValidateBaseServices<T> : BaseServices<T>, IValidateBaseServices<T>
    {

        private IFactory Factory { get; }
        public ValidateBaseServices(IRegisteredPropertyValidateDataManager<T> registeredPropertyManager,
            IFactory factory) : base(registeredPropertyManager)
        {
            this.RegisteredPropertyValidateDataManager = registeredPropertyManager;
            this.Factory = factory;
        }

        public IRegisteredPropertyValidateDataManager<T> RegisteredPropertyValidateDataManager { get; }

        IRegisteredPropertyDataManager<T> IBaseServices<T>.RegisteredPropertyDataManager
        {
            get { return RegisteredPropertyValidateDataManager; }
        }

        public IRuleExecute<T> CreateRuleExecute(T target)
        {
            return Factory.CreateRuleExecute(target);
        }

    }
}
