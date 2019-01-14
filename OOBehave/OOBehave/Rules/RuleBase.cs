using OOBehave.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OOBehave.Rules
{

    public interface IRule
    {
        /// <summary>
        /// Must be unique for every rule across all types
        /// </summary>
        uint UniqueIndex { get; }
        IReadOnlyList<string> TriggerProperties { get; }

    }

    /// <summary>
    /// Contravariant - Allows RuleManager to call even when generic types are different
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRule<in T> : IRule
    {
        Task<IRuleResult> Execute(T target, CancellationToken token);
    }

    internal static class RuleIndexer
    {
        private static uint staticIndex = 0;
        internal static uint StaticIndex
        {
            get
            {
                staticIndex++;
                return staticIndex;
            }
        }
    }

    public abstract class AsyncRuleBase<T> : IRule<T>
    {


        protected AsyncRuleBase()
        {
            /// Must be unique for every rule across all types so Static counter is important
            UniqueIndex = RuleIndexer.StaticIndex;
        }

        public AsyncRuleBase(params string[] triggerProperties) : this(triggerProperties.AsEnumerable()) { }

        public AsyncRuleBase(IEnumerable<string> triggerProperties) : this()
        {
            TriggerProperties.AddRange(triggerProperties);
        }

        public AsyncRuleBase(params IRegisteredProperty[] triggerProperties) : this(triggerProperties.Select(t => t.Name).AsEnumerable()) { }

        public AsyncRuleBase(IEnumerable<IRegisteredProperty> triggerProperties) : this()
        {
            TriggerProperties.AddRange(triggerProperties.Select(t => t.Name));
        }

        public uint UniqueIndex { get; }

        IReadOnlyList<string> IRule.TriggerProperties => TriggerProperties.AsReadOnly();
        protected List<string> TriggerProperties { get; } = new List<string>();


        // TODO - Pass Cancellation Token and Cancel if we reach this 
        // rule again and it is currently running

        public abstract Task<IRuleResult> Execute(T target, CancellationToken token);

        private IPropertyAccess ToPropertyAccessor(T target)
        {
            return target as IPropertyAccess ?? throw new Exception("Target must inherit from Base<> to use ReadPropertyValue method");
        }

        /// <summary>
        /// Allows rule to get the meta properties (IsValid, IsModified)
        /// </summary>
        /// <param name="target"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected IPropertyValue ReadPropertyValue(T target, IRegisteredProperty registeredProperty)
        {
            return ToPropertyAccessor(target).ReadPropertyValue(registeredProperty);
        }

        /// <summary>
        /// Allows rule to get the meta properties (IsValid, IsModified)
        /// </summary>
        /// <param name="target"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected IPropertyValue ReadPropertyValue(T target, string propertyName)
        {
            return ToPropertyAccessor(target).ReadPropertyValue(propertyName);
        }

        protected object ReadProperty(T target, string propertyName)
        {
            return ReadPropertyValue(target, propertyName).Value;
        }

        protected P ReadProperty<P>(T target, IRegisteredProperty<P> registeredProperty)
        {
            return ToPropertyAccessor(target).ReadProperty(registeredProperty);
        }

        protected void SetProperty<P>(T target, IRegisteredProperty<P> registeredProperty, P value)
        {
            ToPropertyAccessor(target).SetProperty(registeredProperty, value);
        }

        protected void LoadProperty<P>(T target, IRegisteredProperty<P> registeredProperty, P value)
        {
            ToPropertyAccessor(target).LoadProperty(registeredProperty, value);
        }

    }


    public abstract class RuleBase<T> : AsyncRuleBase<T>
    {
        protected RuleBase() : base() { }

        public RuleBase(IEnumerable<string> triggerProperties) : base(triggerProperties) { }

        public RuleBase(params string[] triggerProperties) : this(triggerProperties.AsEnumerable()) { }
        public RuleBase(IEnumerable<IRegisteredProperty> triggerProperties) : base(triggerProperties) { }

        public RuleBase(params IRegisteredProperty[] triggerProperties) : this(triggerProperties.AsEnumerable()) { }

        public abstract IRuleResult Execute(T target);

        public sealed override Task<IRuleResult> Execute(T target, CancellationToken token)
        {
            return Task.FromResult(Execute(target));
        }


    }

    public class FluentRule<T> : RuleBase<T>
    {
        private Func<T, IRuleResult> ExecuteFunc { get; }
        public FluentRule(Func<T, IRuleResult> execute, params string[] triggerProperties) : base(triggerProperties)
        {
            this.ExecuteFunc = execute;
        }

        public override IRuleResult Execute(T target)
        {
            return ExecuteFunc(target);
        }
    }
}
