using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.Portal.Core
{
    public class RegisteredOperationManager : IRegisteredOperationManager
    {

        private Dictionary<Type, Dictionary<Operation, List<MethodInfo>>> AllRegisteredOperations = new Dictionary<Type, Dictionary<Operation, List<MethodInfo>>>();


        public bool TypeRegistered<T>()
        {
            return AllRegisteredOperations.ContainsKey(typeof(T));
        }

        public void RegisterOperation<T>(Operation operation, string methodName)
        {
            var method = typeof(T).GetMethod(methodName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance) ?? throw new Exception("No method found");
            RegisterOperation<T>(operation, method);
        }

        public void RegisterOperation<T>(Operation operation, MethodInfo method)
        {

            var returnType = method.ReturnType;

            if (!(returnType == typeof(void) || returnType == typeof(Task)))
            {
                throw new OperationMethodException($"{method.Name} must be void or return Task");
            }

            if (!AllRegisteredOperations.TryGetValue(typeof(T), out var methodDict))
            {
                AllRegisteredOperations.Add(typeof(T), methodDict = new Dictionary<Operation, List<MethodInfo>>());
            }

            if (!methodDict.TryGetValue(operation, out var methodList))
            {
                methodDict.Add(operation, methodList = new List<MethodInfo>());
            }

            methodList.Add(method);

        }

        public IEnumerable<MethodInfo> MethodsForOperation(Type targetType, Operation operation)
        {
            if (!AllRegisteredOperations.TryGetValue(targetType, out var methodDict))
            {
                return null;
            }

            if (!methodDict.TryGetValue(operation, out var methods))
            {
                return null;
            }

            return methods.AsReadOnly();
        }


        public MethodInfo MethodForOperation(Type targetType, Operation operation, Type criteriaType)
        {
            var methods = MethodsForOperation(targetType, operation);

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
