using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.Rules
{

    public interface IRuleResult
    {
        bool IsError { get; }

        IReadOnlyDictionary<string, string> PropertyErrorMessages { get; }

        IReadOnlyList<string> TargetErrorMessages { get; }

    }

    public class RuleResult : IRuleResult
    {

        protected Dictionary<string, string> PropertyErrorMessages { get; } = new Dictionary<string, string>();

        IReadOnlyDictionary<string, string> IRuleResult.PropertyErrorMessages => PropertyErrorMessages;

        protected List<string> TargetErrorMessages { get; } = new List<string>();

        IReadOnlyList<string> IRuleResult.TargetErrorMessages => TargetErrorMessages.AsReadOnly();

        public bool IsError { get { return PropertyErrorMessages.Any() || TargetErrorMessages.Any(); } }

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

        public static RuleResult TargetError(string message)
        {
            var result = new RuleResult();
            result.TargetErrorMessages.Add(message);
            return result;
        }

        internal void AddPropertyErrorMessage(string propertyName, string message)
        {
            PropertyErrorMessages.Add(propertyName, message);
        }

        internal void AddTargetErrorMessage(string message)
        {
            TargetErrorMessages.Add(message);
        }

    }

    public static class RuleResultExtensions
    {
        public static RuleResult AddPropertyError(this RuleResult rr, string propertyName, string message)
        {
            rr.AddPropertyErrorMessage(propertyName, message);
            return rr;
        }

        public static RuleResult AddTargetError(this RuleResult rr, string message)
        {
            rr.AddTargetErrorMessage(message);
            return rr;
        }

    }

}
