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

    public interface IValidateListBase<I> : IListBase<I>, IValidateBase, IValidateMetaProperties
    {

    }

    public abstract class ValidateListBase<T, I> : ListBase<T, I>, IValidateListBase<I>, IValidateListBase, INotifyPropertyChanged, IPropertyAccess, IRuleAccess
        where T : ValidateListBase<T, I>
        where I : IValidateBase
    {
        protected new IValidatePropertyValueManager<T> PropertyValueManager => (IValidatePropertyValueManager<T>)base.PropertyValueManager;

        protected IRuleManager<T> RuleManager { get; private set; }

        public ValidateListBase(IValidateListBaseServices<T, I> services) : base(services)
        {
            this.RuleManager = services.RuleManager;
            this.RuleManager.PropertyChanged += RuleManager_PropertyChanged;
            ((ISetTarget)this.RuleManager).SetTarget(this);

            PropertyIsBusy = new ValidatePropertyMetaByName<bool>(this, (p) => p.IsBusy);
            PropertyIsValid = new ValidatePropertyMetaByName<bool>(this, (p) => p.IsValid);
            PropertyErrorMessage = new ValidatePropertyMetaByName<string>(this, (p) => p.ErrorMessage);

        }

        public bool IsValid => RuleManager.IsValid && PropertyValueManager.IsValid && !this.Any(c => !c.IsValid);
        public bool IsSelfValid => RuleManager.IsValid;
        public bool IsBusy => RuleManager.IsBusy || PropertyValueManager.IsBusy || this.Any(c => c.IsBusy);
        public bool IsSelfBusy => RuleManager.IsBusy;

        protected override void Setter<P>(P value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            if (!IsStopped)
            {
                SetProperty(propertyName, value);
            }
            else
            {
                LoadProperty(GetRegisteredProperty<P>(propertyName), value);
            }
        }

        protected virtual void SetProperty<P>(string propertyName, P value)
        {
            if (PropertyValueManager.SetProperty(GetRegisteredProperty<P>(propertyName), value))
            {
                CheckRules(propertyName);
                PropertyHasChanged(propertyName);
            }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.PropertyName == nameof(IsValid))
            {
                PropertyHasChanged(nameof(PropertyIsValid));
                PropertyHasChanged(nameof(PropertyErrorMessage));
            }
            else if (e.PropertyName == nameof(IsBusy))
            {
                PropertyHasChanged(nameof(PropertyIsBusy));
            }

        }
        protected void PropertyHasChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        private void RuleManager_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyHasChanged(e.PropertyName);
        }

        protected virtual void CheckRules(string propertyName)
        {
            RuleManager.CheckRulesForProperty(propertyName);
        }

        public virtual Task WaitForRules()
        {
            return Task.WhenAll(new Task[3] { RuleManager.WaitForRules, PropertyValueManager.WaitForRules(), Task.WhenAll(this.Where(x => x.IsBusy).Select(x => x.WaitForRules())) });
        }

        /// <summary>
        /// Permantatly mark invalid
        /// Note: not associated with any specific property
        /// </summary>
        /// <param name="message"></param>
        protected virtual void MarkInvalid(string message)
        {
            RuleManager.MarkInvalid(message);
        }


        public override async Task<IDisposable> StopAllActions()
        {
            var result = await base.StopAllActions().ConfigureAwait(false);
            await WaitForRules().ConfigureAwait(false);
            return result;
        }

        IRuleResultReadOnlyList IValidateBase.RuleResultList => RuleManager.Results;
        IReadOnlyList<string> IValidateBase.BrokenRuleMessages => RuleManager.Results.Where(x => x.IsError).SelectMany(x => x.PropertyErrorMessages).Select(x => x.Value).ToList().AsReadOnly();


        void IPropertyAccess.SetProperty<P>(IRegisteredProperty<P> registeredProperty, P value)
        {
            PropertyValueManager.SetProperty(registeredProperty, value);
        }

        public Task CheckAllSelfRules(CancellationToken token = new CancellationToken())
        {
            return RuleManager.CheckAllRules(token);
        }

        public Task CheckAllRules(CancellationToken token = new CancellationToken())
        {
            return Task.WhenAll(RuleManager.CheckAllRules(token), Task.WhenAll(this.Select(t => t.CheckAllRules(token))));
        }

        IValidatePropertyValueInternal IRuleAccess.this[string propertyName]
        {
            get
            {
                return this.PropertyValueManager[propertyName] as IValidatePropertyValueInternal;
            }
        }

        public IValidatePropertyMeta this[string propertyName]
        {
            get
            {
                return new ValidatePropertyMeta(PropertyValueManager[propertyName]);
            }
        }


        // For XAML Binding
        public ValidatePropertyMetaByName<bool> PropertyIsBusy { get; }
        public ValidatePropertyMetaByName<bool> PropertyIsValid { get; }
        public ValidatePropertyMetaByName<string> PropertyErrorMessage { get; }

    }
}
