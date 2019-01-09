using OOBehave.AuthorizationRules;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.Portal.Core
{
    public class PortalOperationManager<T> : IPortalOperationManager<T>
        where T : IPortalTarget
    {

        // TODO (?) make these depedencies that can be set to single instance??
        protected static IDictionary<PortalOperation, List<MethodInfo>> RegisteredOperations { get; } = new ConcurrentDictionary<PortalOperation, List<MethodInfo>>();
        protected static bool IsRegistered { get; set; }

        private IServiceScope Scope { get; }

        protected IAuthorizationRuleManager AuthorizationRuleManager { get; }


        public PortalOperationManager(IServiceScope scope)
        {
#if DEBUG
            if (typeof(T).IsInterface) { throw new Exception($"PortalOperationManager should be service type not interface. {typeof(T).FullName}"); }
#endif
            RegisterPortalOperations();
            Scope = scope;
            AuthorizationRuleManager = scope.Resolve<IAuthorizationRuleManager<T>>();
        }


        protected virtual void RegisterPortalOperations()
        {
            if (!IsRegistered)
            {
                var methods = typeof(T).GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic)
                    .Where(m => m.GetCustomAttributes<PortalOperationAttributeAttribute>() != null);

                foreach (var m in methods)
                {
                    var attributes = m.GetCustomAttributes<PortalOperationAttributeAttribute>();
                    foreach (var o in attributes)
                    {
                        RegisterOperation(o.Operation, m);
                    }
                }
                IsRegistered = true;
            }
        }

        public void RegisterOperation(PortalOperation operation, string methodName)
        {
            var method = typeof(T).GetMethod(methodName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance) ?? throw new Exception("No method found");
            RegisterOperation(operation, method);
        }

        public void RegisterOperation(PortalOperation operation, MethodInfo method)
        {

            var returnType = method.ReturnType;

            if (!(returnType == typeof(void) || returnType == typeof(Task)))
            {
                throw new OperationMethodException($"{method.Name} must be void or return Task");
            }

            if (!RegisteredOperations.TryGetValue(operation, out var methodList))
            {
                RegisteredOperations.Add(operation, methodList = new List<MethodInfo>());
            }

            methodList.Add(method);

        }

        public IEnumerable<MethodInfo> MethodsForOperation(PortalOperation operation)
        {
            if (!RegisteredOperations.TryGetValue(operation, out var methods))
            {
                return null;
            }

            return methods.AsReadOnly();
        }


        public MethodInfo MethodForOperation(PortalOperation operation, IEnumerable<Type> criteriaTypes)
        {
            var methods = MethodsForOperation(operation);
            MethodInfo matchingMethod = null;

            if (methods != null)
            {

                foreach (var m in methods)
                {
                    var parameters = m.GetParameters();

                    // No criteria
                    if (!criteriaTypes.Any() && !parameters.Any())
                    {
                        return m;
                    }
                    else if (criteriaTypes.Any() && parameters.Any() && parameters.Count() >= criteriaTypes.Count())
                    {

                        var match = true;
                        var critEnum = criteriaTypes.GetEnumerator();
                        var paramEnum = parameters.Cast<ParameterInfo>().Select(p => p.ParameterType).GetEnumerator();

                        paramEnum.MoveNext();
                        critEnum.MoveNext();

                        while (match && paramEnum.Current != null)
                        {

                            if (critEnum.Current != null && !paramEnum.Current.IsAssignableFrom(critEnum.Current))
                            {
                                match = false;
                            } else if(critEnum.Current == null && !Scope.IsRegistered(paramEnum.Current)) // For recognizing multiple positives for the same criteria
                            {
                                match = false;
                            }

                            paramEnum.MoveNext();
                            critEnum.MoveNext();

                        }

                        // At the end of the Crit list
                        // The parameter list can 
                        if (match)
                        {
                            if (matchingMethod != null) { throw new Exception($"More then one method for {operation.ToString()} with criteria [{string.Join(",", criteriaTypes)}]"); }

                            matchingMethod = m;
                        }

                    }
                }
            }
            return matchingMethod;
        }

        protected async Task CheckAccess(AuthorizeOperation operation)
        {
            await AuthorizationRuleManager.CheckAccess(operation);
        }

        protected async Task CheckAccess(AuthorizeOperation operation, params object[] criteria)
        {
            if (criteria == null) { throw new ArgumentNullException(nameof(criteria)); }

            await AuthorizationRuleManager.CheckAccess(operation, criteria);
        }

        public async Task<bool> TryCallOperation(IPortalTarget target, PortalOperation operation)
        {
            await CheckAccess(operation.ToAuthorizationOperation());

            var methods = MethodsForOperation(operation) ?? new List<MethodInfo>();

            using (await target.StopAllActions())
            {
                var invoked = false;

                foreach (var method in methods)
                {
                    var success = true;
                    var parameters = method.GetParameters().ToList();
                    var parameterValues = new object[parameters.Count()];

                    for (var i = 0; i < parameterValues.Length; i++)
                    {
                        var parameter = parameters[i];
                        if (!Scope.IsRegistered(parameter.ParameterType))
                        {
                            // Assume it's a criteria not a dependency
                            success = false;
                            break;
                        }
                    }

                    if (success)
                    {
                        // No parameters or all of the parameters are dependencies
                        for (var i = 0; i < parameterValues.Length; i++)
                        {
                            var parameter = parameters[i];
                            parameterValues[i] = Scope.Resolve(parameter.ParameterType);
                        }

                        invoked = true;

                        var result = method.Invoke(target, parameterValues);
                        if (method.ReturnType == typeof(Task))
                        {
                            await (Task)result;
                        }

                        PostOperation(target, operation);

                        break;
                    }
                }

                return invoked;
            }
        }
        public async Task<bool> TryCallOperation(IPortalTarget target, PortalOperation operation, params object[] criteria)
        {
            await CheckAccess(operation.ToAuthorizationOperation(), criteria);

            using (await target.StopAllActions())
            {
                var criteriaTypes = criteria.Select(x => x.GetType()).ToList();

                // This needs to be target.GetType() instead of a generic method
                // because T will be an interface but tager.GetType() will be the concrete
                var method = MethodForOperation(operation, criteriaTypes);

                if (method != null)
                {

                    var parameters = method.GetParameters().ToList();
                    var parameterValues = new object[parameters.Count()];

                    var criteriaE = criteria.GetEnumerator();

                    for (var i = 0; i < parameterValues.Length; i++)
                    {
                        if (criteriaE.MoveNext())
                        {
                            // Use up the criteria values first
                            // Assume MethodForOperation got the types right
                            parameterValues[i] = criteriaE.Current;
                        }
                        else
                        {
                            var parameter = parameters[i];
                            if (Scope.TryResolve(parameter.ParameterType, out var pv))
                            {
                                parameterValues[i] = pv;
                            }
                        }
                    }

                    var result = method.Invoke(target, parameterValues);

                    if (method.ReturnType == typeof(Task))
                    {
                        await (Task)result;
                    }

                    PostOperation(target, operation);

                    return true;
                }

                return false;
            }
        }

        protected virtual void PostOperation(IPortalTarget target, PortalOperation operation)
        {
            var editTarget = target as IPortalEditTarget;
            if (editTarget != null)
            {


                switch (operation)
                {
                    case PortalOperation.Create:
                        editTarget.MarkNew();
                        break;
                    case PortalOperation.CreateChild:
                        editTarget.MarkAsChild();
                        editTarget.MarkNew();
                        break;
                    case PortalOperation.Fetch:
                        break;
                    case PortalOperation.FetchChild:
                        editTarget.MarkAsChild();
                        break;
                    case PortalOperation.Delete:
                        break;
                    case PortalOperation.DeleteChild:
                        break;
                    case PortalOperation.Insert:
                    case PortalOperation.InsertChild:
                    case PortalOperation.Update:
                    case PortalOperation.UpdateChild:
                        editTarget.MarkUnmodified();
                        editTarget.MarkOld();
                        break;
                    default:
                        break;
                }
            }
        }

    }


    [Serializable]
    public class OperationMethodException : Exception
    {
        public OperationMethodException() { }
        public OperationMethodException(string message) : base(message) { }
        public OperationMethodException(string message, Exception inner) : base(message, inner) { }
        protected OperationMethodException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
