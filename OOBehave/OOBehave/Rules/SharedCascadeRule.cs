using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OOBehave.Rules
{


    public abstract class SharedAsyncRule : AsyncRule<IBase>
    {
        public SharedAsyncRule() : base() { }
        public SharedAsyncRule(IEnumerable<IRegisteredProperty> triggerProperties) : base(triggerProperties.Select(x=>x.Name)) { }
        public SharedAsyncRule(params IRegisteredProperty[] triggerProperties) : this(triggerProperties.AsEnumerable()) { }

        public sealed override Task<IRuleResult> Execute(IBase target, CancellationToken token)
        {
            if (target == null) { throw new ArgumentNullException(nameof(target)); }

            Target = target as IPropertyAccess ?? throw new Exception($"To use {nameof(SharedAsyncRule)} {target.GetType().FullName} must inherit from OOBehave.Base");
            var result = Execute(token);
            Target = null;
            return result;
        }

        // Allows the rule to be SingleInstance
        private AsyncLocal<IPropertyAccess> targetAsyncLocal = new AsyncLocal<IPropertyAccess>();

        private IPropertyAccess Target { get => targetAsyncLocal.Value; set => targetAsyncLocal.Value = value; }

        protected abstract Task<IRuleResult> Execute(CancellationToken token);

        protected T ReadProperty<T>(IRegisteredProperty<T> registeredProperty)
        {
            return Target.ReadProperty<T>(registeredProperty);
        }

        protected void SetProperty<T>(IRegisteredProperty<T> registeredProperty, T value)
        {
            Target.SetProperty<T>(registeredProperty, value);
        }


    }

    public abstract class SharedRule : SharedAsyncRule
    {

        public SharedRule() : base() { }
        public SharedRule(IEnumerable<IRegisteredProperty> triggerProperties) : base(triggerProperties) { }
        public SharedRule(params IRegisteredProperty[] triggerProperties) : this(triggerProperties.AsEnumerable()) { }

        protected sealed override Task<IRuleResult> Execute(CancellationToken token)
        {
            return Task.FromResult(Execute());
        }

        protected abstract IRuleResult Execute();

    }
}
