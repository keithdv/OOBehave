using OOBehave.Core;
using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave
{
    public interface IValidateBase : IBase, IValidateMetaProperties
    {
        Task WaitForRules();
    }

    public interface IValidateBase<T> : IValidateBase, IBase<T>
    {

    }

    public abstract class ValidateBase<T> : Base<T>, IValidateBase<T>, INotifyPropertyChanged
        where T: ValidateBase<T>
    {
        protected IRegisteredPropertyValidateDataManager<T> RegisteredPropertyValidateDataManager { get; }
        protected IRegisteredRuleManager RegisteredRuleManager { get; }
        protected IRuleExecute<T> RuleExecute { get; }

        public ValidateBase(IValidateBaseServices<T> services) : base(services)
        {
            this.RegisteredPropertyValidateDataManager = services.RegisteredPropertyValidateDataManager;
            this.RegisteredRuleManager = services.RuleManager;

            this.RegisteredRuleManager.RegisterRules<T>(RegisterRules);
            // TODO - Why do I need to cast to T??
            this.RuleExecute = services.CreateRuleExecute((T) this, RegisteredRuleManager.GetRegisteredRules<T>());
        }

        /// <summary>
        /// Only gets called once per type
        /// </summary>
        /// <param name="rules"></param>
        protected virtual void RegisterRules(IRuleList<T> rules)
        {
            // Default - No Rules
        }

        public bool IsValid => RuleExecute.IsValid;

        public bool IsSelfValid => throw new NotImplementedException();

        public bool IsChild => throw new NotImplementedException();

        protected void SetProperty<P>(P value,[System.Runtime.CompilerServices.CallerMemberName]  string propertyName = "")
        {
            LoadProperty(value, propertyName);
            PropertyHasChanged(propertyName);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void PropertyHasChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            CheckRules(propertyName);
        }

        protected virtual void CheckRules(string propertyName)
        {
            RuleExecute.CheckRulesForProperty(propertyName);
        }

        public virtual Task WaitForRules()
        {
            return RuleExecute.WaitForRules;
        }
        
    }
}
