using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave.AuthorizationRules
{

    /// <summary>
    /// Place on a static method with parameter IRegisteredAuthorizationRuleManager(T)
    /// to define Authorization Rules for the object
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class AuthorizationRulesAttribute : Attribute
    {
        public AuthorizationRulesAttribute()
        {
        }
    }
}
