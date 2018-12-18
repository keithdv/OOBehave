using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OOBehave.Rules
{
    public interface ICascadeRule<T> : IRule<T>
    {

        IReadOnlyList<IRegisteredProperty> TriggerProperties { get; }

    }

    public abstract class CascadeAsyncRule<T> : Rule<T>, ICascadeRule<T>
    {

        public CascadeAsyncRule(IRegisteredProperty triggerProperty) : base()
        {
            TriggerProperties = new List<IRegisteredProperty>() { triggerProperty }.AsReadOnly();
        }

        public CascadeAsyncRule(IEnumerable<IRegisteredProperty> triggerProperties) : base()
        {
            this.TriggerProperties = triggerProperties.ToList().AsReadOnly();
        }

        public IReadOnlyList<IRegisteredProperty> TriggerProperties { get; }


    }

    public abstract class CascadeRule<T> : CascadeAsyncRule<T>
    {
        public CascadeRule(IRegisteredProperty triggerProperty) : base(triggerProperty)
        {

        }

        public CascadeRule(IEnumerable<IRegisteredProperty> triggerProperties) : base(triggerProperties)
        {

        }

        public abstract IRuleResult Execute(T target);

        public sealed override Task<IRuleResult> Execute(T target, CancellationToken token)
        {
            return Task.FromResult(Execute(target));
        }

    }
}
