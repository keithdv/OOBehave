using OOBehave.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave.Rules.Rules
{
    public interface IRequiredRule : IRule
    {

    }

    public class RequiredRule : RuleBase<IBase>, IRequiredRule
    {

        public RequiredRule(string propertyName) : base(propertyName) { }
        public RequiredRule(IRegisteredProperty registeredProperty) : base(registeredProperty) { }

        public override IRuleResult Execute(IBase target)
        {
            var value = ReadProperty(target, TriggerProperties[0]);

            bool isError = false;

            if (value is string s)
            {
                isError = string.IsNullOrWhiteSpace(s);
            }
            else if (value?.GetType().IsValueType ?? false)
            {
                isError = value == Activator.CreateInstance(value.GetType());
            }
            else
            {
                isError = value == null;
            }

            if (isError)
            {
                return RuleResult.PropertyError(TriggerProperties[0], $"{TriggerProperties[0]} is required.");
            }
            else
            {
                return RuleResult.Empty();
            }
        }

    }
}
