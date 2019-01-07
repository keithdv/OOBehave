﻿using OOBehave.Attributes;
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
    public interface IValidateBase : IBase, IValidateMetaProperties
    {
        Task WaitForRules();
        IEnumerable<string> BrokenRuleMessages { get; }
        IEnumerable<string> BrokenRulePropertyMessages(string propertyName);
        Task CheckAllRules(CancellationToken token = new CancellationToken());
        Task CheckAllSelfRules(CancellationToken token = new CancellationToken());
    }

    [PortalDataContract]
    public abstract class ValidateBase<T> : Base<T>, IValidateBase, INotifyPropertyChanged, IPropertyAccess
        where T : ValidateBase<T>
    {

        [PortalDataMember]
        protected new IValidatePropertyValueManager<T> PropertyValueManager => (IValidatePropertyValueManager<T>)base.PropertyValueManager;

        [PortalDataMember]
        protected IRuleExecute<T> RuleExecute { get; }

        public ValidateBase(IValidateBaseServices<T> services) : base(services)
        {
            this.RuleExecute = services.RuleExecute;
            ((ISetTarget)this.RuleExecute).SetTarget(this);
        }

        public bool IsValid => RuleExecute.IsValid && PropertyValueManager.IsValid;

        public bool IsSelfValid => RuleExecute.IsValid;

        public bool IsSelfBusy => RuleExecute.IsBusy;

        public bool IsBusy => RuleExecute.IsBusy || PropertyValueManager.IsBusy;

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
            if(PropertyValueManager.Set(GetRegisteredProperty<P>(propertyName), value))
            {
                PropertyHasChanged(propertyName);
            }
        }

        void IPropertyAccess.SetProperty<P>(IRegisteredProperty<P> registeredProperty, P value)
        {
            PropertyValueManager.Set(registeredProperty, value);
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
            return Task.WhenAll(new Task[2] { RuleExecute.WaitForRules, PropertyValueManager.WaitForRules() });
        }

        /// <summary>
        /// Permantatly mark invalid
        /// Note: not associated with any specific property
        /// </summary>
        /// <param name="message"></param>
        protected virtual void MarkInvalid(string message)
        {
            RuleExecute.MarkInvalid(message);
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
                return (RuleExecute.Results.Where(x => x.IsError).SelectMany(x => x.PropertyErrorMessages).Select(x => x.Value));

            }
        }

        public IEnumerable<string> BrokenRulePropertyMessages(string propertyName)
        {
            return (RuleExecute.Results.Where(x => x.IsError).SelectMany(x => x.PropertyErrorMessages).Where(p => p.Key == propertyName).Select(p => p.Value));
        }

        public Task CheckAllSelfRules(CancellationToken token = new CancellationToken())
        {
            return RuleExecute.CheckAllRules();
        }

        public Task CheckAllRules(CancellationToken token = new CancellationToken())
        {
            return Task.WhenAll(RuleExecute.CheckAllRules(token), PropertyValueManager.CheckAllRules(token));
        }

    }
}
