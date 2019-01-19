using OOBehave.Attributes;
using OOBehave.Core;
using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OOBehave
{

    internal interface IRuleAccess
    {
        IValidatePropertyValueInternal this[string propertyName] { get; }
    }

    public interface IValidateBase : IBase, IValidateMetaProperties, INotifyPropertyChanged
    {
        Task WaitForRules();
        Task CheckAllRules(CancellationToken token = new CancellationToken());
        Task CheckAllSelfRules(CancellationToken token = new CancellationToken());
        IRuleResultReadOnlyList RuleResultList { get; }

        IEnumerable<string> BrokenRuleMessages { get; }

        IValidatePropertyMeta this[string propertyName] { get; }

        ValidatePropertyMetaByName<bool> PropertyIsBusy { get; }
        ValidatePropertyMetaByName<bool> PropertyIsValid { get; }
        ValidatePropertyMetaByName<string> PropertyErrorMessage { get; }
    }


    [PortalDataContract]
    public abstract class ValidateBase<T> : Base<T>, IValidateBase, INotifyPropertyChanged, IPropertyAccess, IDataErrorInfo, IRuleAccess
        where T : ValidateBase<T>
    {

        [PortalDataMember]
        protected new IValidatePropertyValueManager<T> PropertyValueManager => (IValidatePropertyValueManager<T>)base.PropertyValueManager;

        [PortalDataMember]
        protected IRuleManager<T> RuleManager { get; }

        public ValidateBase(IValidateBaseServices<T> services) : base(services)
        {
            this.RuleManager = services.RuleManager;
            this.RuleManager.PropertyChanged += RuleManager_PropertyChanged;
            ((ISetTarget)this.RuleManager).SetTarget(this);

            PropertyIsBusy = new ValidatePropertyMetaByName<bool>(this, (p) => p.IsBusy);
            PropertyIsValid = new ValidatePropertyMetaByName<bool>(this, (p) => p.IsValid);
            PropertyErrorMessage = new ValidatePropertyMetaByName<string>(this, (p) => p.ErrorMessage);

        }


        public bool IsValid => RuleManager.IsValid && PropertyValueManager.IsValid;

        public bool IsSelfValid => RuleManager.IsValid;

        public bool IsSelfBusy => RuleManager.IsBusy;

        public bool IsBusy => RuleManager.IsBusy || PropertyValueManager.IsBusy;

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

        void IPropertyAccess.SetProperty<P>(IRegisteredProperty<P> registeredProperty, P value)
        {
            PropertyValueManager.SetProperty(registeredProperty, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IsValid))
            {
                PropertyHasChanged(nameof(IsSelfValid), true);
                PropertyHasChanged(nameof(PropertyIsValid), true);
                PropertyHasChanged(nameof(PropertyErrorMessage), true);
            }
            else if (e.PropertyName == nameof(IsBusy))
            {
                PropertyHasChanged(nameof(PropertyIsBusy), true);
            }
        }

        protected void PropertyHasChanged(string propertyName, bool isMeta = false)
        {
            PropertyChangedEventArgs e = isMeta ? new MetaPropertyChangedEventArgs(propertyName) : new PropertyChangedEventArgs(propertyName);
            PropertyChanged?.Invoke(this, e);
            OnPropertyChanged(e);
        }

        private void RuleManager_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyHasChanged(e.PropertyName, true);
        }

        protected virtual void CheckRules(string propertyName)
        {
            RuleManager.CheckRulesForProperty(propertyName);
        }

        public virtual Task WaitForRules()
        {
            return Task.WhenAll(new Task[2] { RuleManager.WaitForRules, PropertyValueManager.WaitForRules() });
        }

        public IRuleResultReadOnlyList RuleResultList => RuleManager.Results;

        public IEnumerable<string> BrokenRuleMessages => RuleManager.Results.Where(x => x.IsError).SelectMany(x => x.PropertyErrorMessages).Select(x => x.Value);

        string IDataErrorInfo.Error
        {
            get
            {
                if (!IsSelfValid)
                {
                    if (RuleManager.OverrideResult != null)
                    {
                        return RuleManager.OverrideResult.PropertyErrorMessages.First().Value;
                    }
                    else
                    {
                        return RuleManager.Results.FirstOrDefault()?.PropertyErrorMessages.First().Value ?? string.Empty;
                    }
                }
                return string.Empty;
            }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (!IsSelfValid)
                {
                    return RuleManager[columnName].FirstOrDefault() ?? string.Empty;
                }
                return string.Empty;
            }
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

        public Task CheckAllSelfRules(CancellationToken token = new CancellationToken())
        {
            return RuleManager.CheckAllRules();
        }

        public Task CheckAllRules(CancellationToken token = new CancellationToken())
        {
            return Task.WhenAll(RuleManager.CheckAllRules(token), PropertyValueManager.CheckAllRules(token));
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
                var pv = PropertyValueManager[propertyName];

                return pv != null ? new ValidatePropertyMeta(pv) : null;
            }
        }


        // For XAML Binding
        public ValidatePropertyMetaByName<bool> PropertyIsBusy { get; }
        public ValidatePropertyMetaByName<bool> PropertyIsValid { get; }
        public ValidatePropertyMetaByName<string> PropertyErrorMessage { get; }


    }

    public class ValidatePropertyMetaByName<R>
    {
        public ValidatePropertyMetaByName(IValidateBase target, Func<IValidatePropertyMeta, R> toReturn)
        {
            Target = target;
            TranslateFunc = toReturn;
        }

        private IValidateBase Target { get; }
        private Func<IValidatePropertyMeta, R> TranslateFunc { get; }

        public R this[string propertyName]
        {
            get
            {
                return TranslateFunc(Target[propertyName]);
            }
        }
    }
}
