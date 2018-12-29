using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OOBehave.Rules
{


    public abstract class SharedTargetAsyncRule : TargetAsyncRule<IBase>
    {
        public SharedTargetAsyncRule() : base() { }

        public sealed override Task<IRuleResult> Execute(IBase target, CancellationToken token)
        {
            if (target == null) { throw new ArgumentNullException(nameof(target)); }

            Target = target as IPropertyAccess ?? throw new Exception($"To use {nameof(SharedTargetAsyncRule)} {target.GetType().FullName} must inherit from OOBehave.Base or OOBehave.ListBase");
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

    public abstract class SharedTargetRule : SharedTargetAsyncRule
    {

        public SharedTargetRule() : base() { }

        protected sealed override Task<IRuleResult> Execute(CancellationToken token)
        {
            return Task.FromResult(Execute());
        }

        protected abstract IRuleResult Execute();

    }
}
