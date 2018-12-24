using OOBehave.Core;
using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave
{
    public interface IValidateBase : IBase, IValidateMetaProperties
    {
        Task WaitForRules();
        IEnumerable<string> BrokenRuleMessages { get; }
        IEnumerable<string> BrokenRulePropertyMessages(string propertyName);
    }

    public interface IValidateBase<T> : IValidateBase, IBase<T>
    {

    }

    public abstract class ValidateBase<T> : Base<T>, IValidateBase<T>, INotifyPropertyChanged
        where T : ValidateBase<T>
    {
        protected IValidatePropertyValueManager<T> ValidatePropertyValueManager { get; }

        protected IRuleExecute<T> RuleExecute { get; }

        public ValidateBase(IValidateBaseServices<T> services) : base(services)
        {
            this.ValidatePropertyValueManager = services.ValidatePropertyValueManager;

            // TODO - Why do I need to cast to T??
            this.RuleExecute = services.CreateRuleExecute((T)this);
        }

        public bool IsValid => RuleExecute.IsValid && ValidatePropertyValueManager.IsValid;

        public bool IsSelfValid => RuleExecute.IsValid;

        public bool IsChild => throw new NotImplementedException();

        protected void SetProperty<P>(P value, [System.Runtime.CompilerServices.CallerMemberName]  string propertyName = "")
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

        public IEnumerable<string> BrokenRuleMessages
        {
            get
            {
                return (RuleExecute.Results.Where(x => x.IsError).SelectMany(x => x.PropertyErrorMessages).Select(x => x.Value)
                                .Union(RuleExecute.Results.Where(x => x.IsError).SelectMany(x => x.TargetErrorMessages)));

            }
        }

        public IEnumerable<string> BrokenRulePropertyMessages(string propertyName)
        {
            return (RuleExecute.Results.Where(x => x.IsError).SelectMany(x => x.PropertyErrorMessages).Where(p => p.Key == propertyName).Select(p => p.Value));
        }

    }
}
