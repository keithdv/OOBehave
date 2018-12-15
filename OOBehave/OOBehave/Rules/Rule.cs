using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.Rules
{

    public interface IRule
    {
        Task<IRuleResult> Execute(IRuleContext context);

        IReadOnlyList<IRegisteredProperty> InputProperties { get; }
    }

    public abstract class Rule : IRule
    {
        public Rule(IRegisteredProperty inputProperty)
        {
            InputProperties = new List<IRegisteredProperty>() { inputProperty }.AsReadOnly();
        }

        public Rule(ValueTuple<IRegisteredProperty, IRegisteredProperty> inputProperties)
        {
            this.InputProperties = new List<IRegisteredProperty>() { inputProperties.Item1, inputProperties.Item2 }.AsReadOnly();
        }

        public Rule(ValueTuple<IRegisteredProperty, IRegisteredProperty, IRegisteredProperty> inputProperties)
        {
            this.InputProperties = new List<IRegisteredProperty>() { inputProperties.Item1, inputProperties.Item2, inputProperties.Item3 }.AsReadOnly();
        }

        public Rule(IEnumerable<IRegisteredProperty> inputProperties)
        {
            this.InputProperties = inputProperties.ToList().AsReadOnly();
        }

        public IReadOnlyList<IRegisteredProperty> InputProperties { get; }

        public abstract Task<IRuleResult> Execute(IRuleContext context);

    }
}
