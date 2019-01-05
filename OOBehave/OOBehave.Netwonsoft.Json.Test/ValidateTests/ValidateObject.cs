using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace OOBehave.Netwonsoft.Json.Test.ValidateTests
{
    public interface IValidateObject : IValidateBase
    {
        Guid ID { get; set; }
        string Name { get; set; }
        int RuleRunCount { get; }

        IValidateObject Child { get; set; }
        IEnumerable<IRule> Rules { get; }
    }

    public class ValidateObject : ValidateBase<ValidateObject>, IValidateObject
    {
        public ValidateObject(IValidateBaseServices<ValidateObject> services) : base(services)
        {
            RuleExecute.AddRule(nameof(Name), t =>
            {
                t.RuleRunCount++;
                if (t.Name == "Error") { return RuleResult.PropertyError(nameof(Name), "Error"); }
                return RuleResult.Empty();
            });
        }

        public int RuleRunCount { get => Getter<int>(); set => Setter(value); }
        public Guid ID { get => Getter<Guid>(); set => Setter(value); }
        public string Name { get => Getter<string>(); set => Setter(value); }
        public IValidateObject Child { get => Getter<IValidateObject>(); set => Setter(value); }

        public IEnumerable<IRule> Rules => RuleExecute.Rules;
    }

    public interface IValidateObjectList : IValidateListBase<IValidateObject>
    {
        Guid ID { get; set; }
        string Name { get; set; }
        int RuleRunCount { get; }
        IEnumerable<IRule> Rules { get; }
        void Add(IValidateObject obj);
    }

    public class ValidateObjectList : ValidateListBase<ValidateObjectList, IValidateObject>, IValidateObjectList
    {
        public ValidateObjectList(IValidateListBaseServices<ValidateObjectList, IValidateObject> services) : base(services)
        {
            RuleExecute.AddRule(nameof(Name), t =>
            {
                t.RuleRunCount++;
                if (t.Name == "Error") { return RuleResult.PropertyError(nameof(Name), "Error"); }
                return RuleResult.Empty();
            });
        }

        public int RuleRunCount { get => Getter<int>(); set => Setter(value); }
        public Guid ID { get => Getter<Guid>(); set => Setter(value); }
        public string Name { get => Getter<string>(); set => Setter(value); }
        public IEnumerable<IRule> Rules => RuleExecute.Rules;


    }
}
