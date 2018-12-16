using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.Rules
{
    public interface ICascadeRule<T> : IRule<T>
    {

        IReadOnlyList<IRegisteredProperty> TriggerProperties { get; }

    }

    public abstract class CascadeRule<T> : Rule<T>, ICascadeRule<T>
    {

        public CascadeRule(IRegisteredProperty inputProperty) : base()
        {
            TriggerProperties = new List<IRegisteredProperty>() { inputProperty }.AsReadOnly();
        }

        public CascadeRule(IEnumerable<IRegisteredProperty> triggerProperties) : base()
        {
            this.TriggerProperties = triggerProperties.ToList().AsReadOnly();
        }

        public IReadOnlyList<IRegisteredProperty> TriggerProperties { get; }


    }
}
