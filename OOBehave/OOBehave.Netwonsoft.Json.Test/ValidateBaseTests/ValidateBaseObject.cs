using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace OOBehave.Netwonsoft.Json.Test.ValidateBaseTests
{
    public interface IValidateBaseObject : IValidateBase
    {
        Guid ID { get; set; }
        string Name { get; set; }
        int RuleRunCount { get; }

        IValidateBaseObject Child { get; set; }
        IValidateBaseObject Parent { get; set; }
        IEnumerable<IRule> Rules { get; }
    }

    public class ValidateBaseObject : ValidateBase, IValidateBaseObject
    {
        public ValidateBaseObject(IValidateBaseServices<ValidateBaseObject> services) : base(services)
        {
            RuleExecute.AddRule<ValidateBaseObject>(nameof(Name), t =>
            {
                t.RuleRunCount++;
                if (t.Name == "Error") { return RuleResult.PropertyError(nameof(Name), "Error"); }
                return RuleResult.Empty();
            });
        }

        public int RuleRunCount { get => Getter<int>(); set => Setter(value); }
        public Guid ID { get => Getter<Guid>(); set => Setter(value); }
        public string Name { get => Getter<string>(); set => Setter(value); }
        public IValidateBaseObject Child { get => Getter<IValidateBaseObject>(); set => Setter(value); }
        public IValidateBaseObject Parent { get => Getter<IValidateBaseObject>(); set => Setter(value); }

        public IEnumerable<IRule> Rules => RuleExecute.Rules;
    }
}
