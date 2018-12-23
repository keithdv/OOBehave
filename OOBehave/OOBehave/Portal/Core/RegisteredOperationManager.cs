using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.Portal.Core
{
    public class RegisteredOperationManager<T> : IRegisteredOperationManager<T>
    {


        private IDictionary<Operation, List<MethodInfo>> RegisteredOperations { get; } = new ConcurrentDictionary<Operation, List<MethodInfo>>();

        public RegisteredOperationManager()
        {
#if DEBUG
            if (typeof(T).IsInterface) { throw new Exception($"RegisteredOperationManager should be service type not interface. {typeof(T).FullName}"); }
#endif
            RegisterPortalOperations();
        }


        // TODO: This should be handled by ObjectPortal
        protected virtual void RegisterPortalOperations()
        {
                var methods = typeof(T).GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic)
                    .Where(m => m.GetCustomAttribute<OperationAttribute>() != null);

                foreach (var m in methods)
                {
                    var attribute = m.GetCustomAttribute<OperationAttribute>();
                    RegisterOperation(attribute.Operation, m);
                }
        }

        public void RegisterOperation(Operation operation, string methodName)
        {
            var method = typeof(T).GetMethod(methodName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance) ?? throw new Exception("No method found");
            RegisterOperation(operation, method);
        }

        public void RegisterOperation(Operation operation, MethodInfo method)
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

        public IEnumerable<MethodInfo> MethodsForOperation(Operation operation)
        {
            if (!RegisteredOperations.TryGetValue(operation, out var methods))
            {
                return null;
            }

            return methods.AsReadOnly();
        }


        public MethodInfo MethodForOperation(Operation operation, Type criteriaType)
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
