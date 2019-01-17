using OOBehave.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.Rules
{

    public interface IRuleResult
    {
        bool IsError { get; }

        IReadOnlyDictionary<string, string> PropertyErrorMessages { get; }

        IReadOnlyList<string> TriggerProperties { get; set; }
        string this[string propertyName] { get; }
    }

    [PortalDataContract]
    public class RuleResult : IRuleResult
    {
        [PortalDataMember]
        protected Dictionary<string, string> PropertyErrorMessages { get; } = new Dictionary<string, string>();

        IReadOnlyDictionary<string, string> IRuleResult.PropertyErrorMessages => new ReadOnlyDictionary<string, string>(PropertyErrorMessages);

        public bool IsError { get { return PropertyErrorMessages.Any(); } }

        [PortalDataMember]
        public IReadOnlyList<string> TriggerProperties { get; set; }

        string IRuleResult.this[string propertyName]
        {
            get
            {
                if (PropertyErrorMessages.TryGetValue(propertyName, out var result))
                {
                    return result;
                }
                return string.Empty;
            }
        }

        public static RuleResult Empty()
        {
            return new RuleResult();
        }

        public static RuleResult PropertyError(string propertyName, string message)
        {
            var result = new RuleResult();
            result.PropertyErrorMessages.Add(propertyName, message);
            return result;
        }

        internal void AddPropertyErrorMessage(string propertyName, string message)
        {
            PropertyErrorMessages.Add(propertyName, message);
        }

        [OnSerializing]
        public void OnSerializing(StreamingContext context)
        {
            // Readonly list cannot be serialized

            TriggerProperties = TriggerProperties?.ToList();
        }

    }

    public static class RuleResultExtensions
    {
        public static RuleResult AddPropertyError(this RuleResult rr, string propertyName, string message)
        {
            rr.AddPropertyErrorMessage(propertyName, message);
            return rr;
        }

    }

}
