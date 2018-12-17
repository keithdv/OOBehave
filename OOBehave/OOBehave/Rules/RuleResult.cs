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

        IDictionary<IRegisteredProperty, string> PropertyErrorMessages { get; }

        IList<string> TargetErrorMessages { get; }

    }

    public class RuleResult : IRuleResult
    {

        public IDictionary<IRegisteredProperty, string> PropertyErrorMessages { get; } = new Dictionary<IRegisteredProperty, string>();

        public IList<string> TargetErrorMessages { get; } = new List<string>();

        public bool IsError { get { return PropertyErrorMessages.Any() || TargetErrorMessages.Any(); } }

        public static IRuleResult Empty()
        {
            return (IRuleResult)new RuleResult();
        }

        public static IRuleResult PropertyError(IRegisteredProperty property, string message)
        {
            var result = new RuleResult();
            result.PropertyErrorMessages.Add(property, message);
            return (IRuleResult)result;
        }

        public static IRuleResult TargetError(string message)
        {
            var result = new RuleResult();
            result.TargetErrorMessages.Add(message);
            return (IRuleResult)result;
        }

    }

    public static class RuleResultExtensions
    {

    }
}
