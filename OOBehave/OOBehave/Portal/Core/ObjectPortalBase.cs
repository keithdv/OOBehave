using OOBehave.AuthorizationRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OOBehave.Portal.Core
{

    public class ObjectPortalBase
    {
        protected static AsyncLocal<IServiceScope> dependencyScope { get; } = new AsyncLocal<IServiceScope>();

        protected IServiceScope DependencyScope
        {
            get { return dependencyScope.Value ?? throw new Exception("Must be called within a using(UsingDependencyScope) first."); }
        }

        protected static AsyncLocal<IServiceScope> targetScope { get; } = new AsyncLocal<IServiceScope>();

        protected IServiceScope TargetScope
        {
            get { return targetScope.Value ?? throw new Exception("Must call within a using(UsingDependencyScope) first."); }
        }

    }

    /// <summary>
    /// Provide Authorization Check
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectPortalBase<T> : ObjectPortalBase
        where T : IPortalTarget
    {

        public ObjectPortalBase(IServiceScope scope)
        {

            var type = typeof(T);

            if (type.IsInterface)
            {
                // To find the static method this needs to be the concrete type
                ConcreteType = scope.ConcreteType(type) ?? throw new Exception($"Type {type.FullName} is not registered");
            }
            else
            {
                ConcreteType = type;
            }
        }

        public Type ConcreteType { get; }

        public virtual IServiceScope UsingOperationScope()
        {
            return null;
        }

        protected virtual IPortalOperationManager OperationManager(IServiceScope scope)
        {
            return (IPortalOperationManager)scope.Resolve(typeof(IPortalOperationManager<>).MakeGenericType(ConcreteType));
        }

        protected virtual async Task CallOperationMethod(IServiceScope scope, T target, PortalOperation operation, bool throwException = true)
        {
            var success = await OperationManager(scope).TryCallOperation(target, operation).ConfigureAwait(false);

            if (!success && throwException)
            {
                throw new OperationMethodCallFailedException($"{operation.ToString()} method with no criteria not found on {target.GetType().FullName}.");
            }
        }


        protected virtual async Task CallOperationMethod(IServiceScope scope, T target, PortalOperation operation, object[] criteria, Type[] criteriaTypes)
        {
            if (target == null) { throw new ArgumentNullException(nameof(target)); }
            if (criteria == null) { throw new ArgumentNullException(nameof(criteria)); }

            var success = await OperationManager(scope).TryCallOperation(target, operation, criteria, criteriaTypes).ConfigureAwait(false);

            if (!success)
            {
                throw new OperationMethodCallFailedException($"{operation.ToString()} method on {target.GetType().FullName} with criteria [{string.Join(", ", criteriaTypes.Select(x => x.FullName))}] not found.");
            }
        }
    }

}
