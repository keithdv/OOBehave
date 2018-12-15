using OOBehave.Core;
using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave
{
    public interface IValidateBase : IBase, IValidateMetaProperties
    {

    }

    public interface IValidateBase<T> : IValidateBase, IBase<T>
    {

    }

    public class ValidateBase<T> : Base<T>, IValidateBase<T>
        where T : Base<T>
    {
        protected IRegisteredPropertyValidateDataManager<T> RegisteredPropertyValidateDataManager { get; }
        protected IRegisteredRuleManager RegisteredRuleManager { get; }

        public ValidateBase(IValidateBaseServices<T> services) : base(services)
        {
            this.RegisteredPropertyValidateDataManager = services.RegisteredPropertyValidateDataManager;
            this.RegisteredRuleManager = services.RuleManager;

            this.RegisteredRuleManager.RegisterRules<T>(RegisterRules);
        }

        /// <summary>
        /// Only gets called once per type
        /// </summary>
        /// <param name="rules"></param>
        protected virtual void RegisterRules(IRuleList rules)
        {
            // Default - No Rules
        }

        public bool IsValid => throw new NotImplementedException();

        public bool IsSelfValid => throw new NotImplementedException();

        public bool IsChild => throw new NotImplementedException();
    }
}
