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
    {


        private IDictionary<PortalOperation, List<MethodInfo>> RegisteredOperations { get; } = new ConcurrentDictionary<PortalOperation, List<MethodInfo>>();

        private IServiceScope Scope { get; }

        public PortalOperationManager(IServiceScope scope)
        {
#if DEBUG
            if (typeof(T).IsInterface) { throw new Exception($"PortalOperationManager should be service type not interface. {typeof(T).FullName}"); }
#endif
            RegisterPortalOperations();
            Scope = scope;
        }


        // TODO: This should be handled by ObjectPortal
        protected virtual void RegisterPortalOperations()
        {
                var methods = typeof(T).GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic)
                    .Where(m => m.GetCustomAttribute<PortalOperationAttributeAttribute>() != null);

                foreach (var m in methods)
                {
                    var attribute = m.GetCustomAttribute<PortalOperationAttributeAttribute>();
                    RegisterOperation(attribute.Operation, m);
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


        public MethodInfo MethodForOperation(PortalOperation operation, Type criteriaType)
        {
            var methods = MethodsForOperation(operation);

            if (methods != null)
            {
                foreach (var m in methods)
                {
                    var parameters = m.GetParameters();
                    var hasCriteriaParameter = parameters.Where(p => p.ParameterType == criteriaType).FirstOrDefault();

                    if (hasCriteriaParameter != null)
                    {
                        return m;
                    }

                }
            }

            return null;
        }

        public async Task<bool> TryCallOperation(object target, PortalOperation operation)
        {
            var methods = MethodsForOperation(operation) ?? new List<MethodInfo>();

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
                        await(Task)result;
                    }

                    break;
                }
            }

            return invoked;

        }
        public async Task<bool> TryCallOperation(object target, object criteria, PortalOperation operation)
        {
            // This needs to be target.GetType() instead of a generic method
            // because T will be an interface but tager.GetType() will be the concrete
            var method = MethodForOperation(operation, criteria.GetType());

            if (method != null)
            {

                var parameters = method.GetParameters().ToList();
                var parameterValues = new object[parameters.Count()];

                for (var i = 0; i < parameterValues.Length; i++)
                {
                    var parameter = parameters[i];
                    if (parameter.ParameterType == criteria.GetType())
                    {
                        parameterValues[i] = criteria;
                    }
                    else
                    {
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

                return true;
            }

            return false;
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
