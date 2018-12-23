using OOBehave.AuthorizationRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.Portal.Core
{
    /// <summary>
    /// Provide Authorization Check
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectPortalBase<T>
        where T : IBase
    {

        protected IServiceScope Scope { get; }
        protected IRegisteredOperationManager RegisteredOperationManager { get; }
        protected IRegisteredAuthorizationRuleManager RegisteredAuthorizationRuleManager { get; }
        public ObjectPortalBase(IServiceScope scope)
        {
            Scope = scope;

            // To find the static method this needs to be the concrete type
            var concreteType = scope.ConcreteType<T>() ?? throw new Exception($"Type {typeof(T).FullName} is not registered");
            RegisteredOperationManager = scope.Resolve(typeof(IRegisteredOperationManager<>).MakeGenericType(concreteType)) as IRegisteredOperationManager;
            RegisteredAuthorizationRuleManager = scope.Resolve(typeof(IRegisteredAuthorizationRuleManager<>).MakeGenericType(concreteType)) as IRegisteredAuthorizationRuleManager;
        }


        protected async Task CheckAccess(AuthorizeOperation operation)
        {
            if (RegisteredAuthorizationRuleManager.IsRegistered)
            {
                var methods = RegisteredAuthorizationRuleManager.AuthorizationRules(operation);
                var methodFound = false;

                foreach (var ruleMethod in methods)
                {
                    var method = ruleMethod.Method;

                    if (!method.GetParameters().Any())
                    {
                        // Only allow one; maybe take this out later
                        // AuthorizationRules should be stringent
                        if (methodFound)
                        {
                            throw new AuthorzationRulesMethodException($"More than one {operation.ToString()} method with no criteria found in {ruleMethod.AuthorizationRule.GetType().ToString()}");
                        }

                        methodFound = true;
                        IAuthorizationRuleResult ruleResult;
                        var methodResult = method.Invoke(ruleMethod.AuthorizationRule, new object[0]);
                        var methodResultAsync = methodResult as Task<IAuthorizationRuleResult>;

                        if (methodResultAsync != null)
                        {
                            await methodResultAsync;
                            ruleResult = ((IAuthorizationRuleResult)methodResultAsync.Result);
                        }
                        else
                        {
                            ruleResult = ((IAuthorizationRuleResult)methodResult);
                        }

                        if (!ruleResult.HasAccess)
                        {
                            throw new AccessDeniedException(ruleResult.Message);
                        }
                    }
                }

                if (!methodFound)
                {
                    throw new AccessDeniedException($"Missing authorization method for {operation.ToString()} with no criteria");
                }
            }

        }

        protected async Task CheckAccess(AuthorizeOperation operation, object criteria)
        {
            if (criteria == null) { throw new ArgumentNullException(nameof(criteria)); }

            if (RegisteredAuthorizationRuleManager.IsRegistered)
            {
                var methods = RegisteredAuthorizationRuleManager.AuthorizationRules(operation);
                var methodFound = false;

                foreach (var ruleMethod in methods)
                {
                    var method = ruleMethod.Method;
                    var parameter = method.GetParameters().SingleOrDefault();
                    if (parameter != null && parameter.ParameterType.IsAssignableFrom(criteria.GetType()))
                    {

                        // Only allow one; maybe take this out later
                        // AuthorizationRules should be stringent
                        if (methodFound)
                        {
                            throw new AuthorzationRulesMethodException($"More than one {operation.ToString()} method with no criteria found in {ruleMethod.AuthorizationRule.GetType().ToString()}");
                        }
                        methodFound = true;

                        IAuthorizationRuleResult ruleResult;
                        var methodResult = method.Invoke(ruleMethod.AuthorizationRule, new object[1] { criteria });
                        var methodResultAsync = methodResult as Task<IAuthorizationRuleResult>;

                        if (methodResultAsync != null)
                        {
                            await methodResultAsync;
                            ruleResult = ((IAuthorizationRuleResult)methodResultAsync.Result);
                        }
                        else
                        {
                            ruleResult = ((IAuthorizationRuleResult)methodResult);
                        }

                        if (!ruleResult.HasAccess)
                        {
                            throw new AccessDeniedException(ruleResult.Message);
                        }
                    }
                }

                if (!methodFound)
                {
                    throw new AccessDeniedException($"Missing authorization method for {operation.ToString()} with criteria {criteria.GetType().FullName}");
                }

            }
        }

    }


    [Serializable]
    public class AccessDeniedException : Exception
    {
        public AccessDeniedException() { }
        public AccessDeniedException(string message) : base(message) { }
        public AccessDeniedException(string message, Exception inner) : base(message, inner) { }
        protected AccessDeniedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
