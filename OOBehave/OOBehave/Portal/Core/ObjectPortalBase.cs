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
        protected IPortalOperationManager OperationManager { get; }
        protected IAuthorizationRuleManager AuthorizationRuleManager { get; }
        public ObjectPortalBase(IServiceScope scope)
        {
            Scope = scope;

            // To find the static method this needs to be the concrete type
            var concreteType = scope.ConcreteType<T>() ?? throw new Exception($"Type {typeof(T).FullName} is not registered");
            OperationManager = scope.Resolve(typeof(IPortalOperationManager<>).MakeGenericType(concreteType)) as IPortalOperationManager;
            AuthorizationRuleManager = scope.Resolve(typeof(IAuthorizationRuleManager<>).MakeGenericType(concreteType)) as IAuthorizationRuleManager;
        }


        protected async Task CheckAccess(AuthorizeOperation operation)
        {
            await AuthorizationRuleManager.CheckAccess(operation);
        }

        protected async Task CheckAccess(AuthorizeOperation operation, object criteria)
        {
            if (criteria == null) { throw new ArgumentNullException(nameof(criteria)); }

            await AuthorizationRuleManager.CheckAccess(operation, criteria);
        }

    }



}
