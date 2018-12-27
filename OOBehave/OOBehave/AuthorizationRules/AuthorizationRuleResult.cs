using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave.AuthorizationRules
{

    public interface IAuthorizationRuleResult
    {
        bool HasAccess { get; }
        string Message { get; }
    }

    public class AuthorizationRuleResult : IAuthorizationRuleResult
    {
        public bool HasAccess { get; }
        public string Message { get; set; }

        public AuthorizationRuleResult(bool hasAccess)
        {
            HasAccess = hasAccess;
        }


        public static IAuthorizationRuleResult AccessDenied(string message)
        {
            return new AuthorizationRuleResult(false) { Message = message };
        }

        public static IAuthorizationRuleResult AccessGranted()
        {
            return new AuthorizationRuleResult(true);
        }

    }
}
