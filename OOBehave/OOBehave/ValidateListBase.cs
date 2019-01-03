using OOBehave.Core;
using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OOBehave
{
    public interface IValidateListBase : IListBase, IValidateBase, IValidateMetaProperties
    {

    }

    public interface IValidateListBase<T> : IValidateListBase, IListBase<T>
    {

    }

    public abstract class ValidateListBase<T> : ListBase<T>, IValidateListBase<T>, INotifyPropertyChanged, IPropertyAccess
        where T : IValidateBase
    {
        protected IValidatePropertyValueManager ValidatePropertyValueManager => (IValidatePropertyValueManager)base.PropertyValueManager;

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use ValidatePropertyManager", true)]
        new IPropertyValueManager PropertyValueManager => base.PropertyValueManager;

        protected IRuleExecute RuleExecute { get; private set; }

        public ValidateListBase(IValidateListBaseServices<T> services) : base(services)
        {
            this.RuleExecute = services.RuleExecute;
            ((ISetTarget)this.RuleExecute).SetTarget(this);
        }

        public bool IsValid => RuleExecute.IsValid && ValidatePropertyValueManager.IsValid && !this.Any(c => !c.IsValid);
        public bool IsSelfValid => RuleExecute.IsValid;
        public bool IsBusy => RuleExecute.IsBusy || ValidatePropertyValueManager.IsBusy || this.Any(c => c.IsBusy);
        public bool IsSelfBusy => RuleExecute.IsBusy;

        protected override void Setter<P>(P value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            if (!IsStopped)
            {
                SetProperty(propertyName, value);
            }
            else
            {
                LoadProperty(propertyName, value);
            }
        }

        protected virtual void SetProperty<P>(string propertyName, P value)
        {
            ValidatePropertyValueManager.Set(propertyName, value);
            PropertyHasChanged(propertyName);
        }


        protected void PropertyHasChanged(string propertyName)
        {
            base.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
            CheckRules(propertyName);
        }

        protected virtual void CheckRules(string propertyName)
        {
            RuleExecute.CheckRulesForProperty(propertyName);
        }

        public virtual Task WaitForRules()
        {
            return Task.WhenAll(new Task[3] { RuleExecute.WaitForRules, ValidatePropertyValueManager.WaitForRules(), Task.WhenAll(this.Where(x => x.IsBusy).Select(x => x.WaitForRules())) });
        }

        public override async Task<IDisposable> StopAllActions()
        {
            var result = await base.StopAllActions();
            await WaitForRules();
            return result;
        }

        public IEnumerable<string> BrokenRuleMessages
        {
            get
            {
                return RuleExecute.Results.Where(x => x.IsError).SelectMany(x => x.PropertyErrorMessages).Select(x => x.Value);

            }
        }

        public IEnumerable<string> BrokenRulePropertyMessages(string propertyName)
        {
            return (RuleExecute.Results.Where(x => x.IsError).SelectMany(x => x.PropertyErrorMessages).Where(p => p.Key == propertyName).Select(p => p.Value));
        }

        void IPropertyAccess.SetProperty<P>(IRegisteredProperty<P> registeredProperty, P value)
        {
            ValidatePropertyValueManager.Set(registeredProperty, value);
        }

        public Task CheckAllSelfRules(CancellationToken token = new CancellationToken())
        {
            return RuleExecute.CheckAllRules(token);
        }

        public Task CheckAllRules(CancellationToken token = new CancellationToken())
        {
            return Task.WhenAll(RuleExecute.CheckAllRules(token), Task.WhenAll(this.Select(t => t.CheckAllRules(token))));
        }
    }
}
