using OOBehave.Attributes;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace OOBehave.Rules
{

    public interface IRuleResultReadOnlyList : IReadOnlyList<IRuleResult>
    {
        bool IsError { get; }
        IRuleResult this[string propertyName] { get; }

        IReadOnlyList<IRuleResult> Results(string propertyName);
    }

    public class RuleResultReadOnlyList : IRuleResultReadOnlyList
    {
        public RuleResultReadOnlyList(ICollection<IRuleResult> ruleResults)
        {
            RuleResultList = ruleResults;
        }
        bool IRuleResultReadOnlyList.IsError => this.Any(r => r.IsError);

        private ICollection<IRuleResult> RuleResultList { get; }

        int IReadOnlyCollection<IRuleResult>.Count => RuleResultList.Count;

        IRuleResult IReadOnlyList<IRuleResult>.this[int index] => throw new NotImplementedException();

        IRuleResult IRuleResultReadOnlyList.this[string propertyName]
        {
            get
            {
                return RuleResultList.FirstOrDefault(r => r.PropertyErrorMessages[propertyName] != null);
            }
        }

        IReadOnlyList<IRuleResult> IRuleResultReadOnlyList.Results(string propertyName)
        {
            return RuleResultList.Where(r => r.PropertyErrorMessages.ContainsKey(propertyName)).ToList().AsReadOnly();
        }

        IEnumerator<IRuleResult> IEnumerable<IRuleResult>.GetEnumerator()
        {
            return RuleResultList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return RuleResultList.GetEnumerator();
        }
    }

    public interface IRuleResultList : IDictionary<int, IRuleResult>
    {
        IRuleResult this[string propertyName] { get; }
        void SetKeysToNegative();

        IRuleResultReadOnlyList RuleResultList { get; }

        IRuleResult OverrideResult { get; set; }
    }

    public class RuleResultList : ConcurrentDictionary<int, IRuleResult>, IRuleResultList
    {
        private IDictionary<int, IRuleResult> dict => (IDictionary<int, IRuleResult>)this;

        IRuleResultReadOnlyList IRuleResultList.RuleResultList => new RuleResultReadOnlyList(this.Values);

        public IRuleResult this[string propertyName]
        {
            get
            {
                return this.Values.FirstOrDefault(r => r.PropertyErrorMessages[propertyName] != null);
            }
        }

        public void SetKeysToNegative()
        {
            foreach (var kvp in this)
            {
                if (kvp.Key > 0)
                {
                    dict.Add(kvp.Key * -1, kvp.Value);
                    dict.Remove(kvp.Key);
                }
            }
        }

        private IRuleResult overrideResult;

        public IRuleResult OverrideResult
        {

            get { return overrideResult; }
            set
            {
                overrideResult = value;
                if (value != null) // Deserialization sends thru a null value
                {
                    this.Clear();
                    dict.Add(0, overrideResult);
                }
            }
        }

    }
}
