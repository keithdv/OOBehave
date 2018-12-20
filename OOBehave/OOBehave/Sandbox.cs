using OOBehave.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OOBehave
{
    public interface IDal { }

    public enum Operation
    {
        Create, CreateChild,
        Fetch, FetchChild,
        Update, UpdateChild,
        Insert, InsertChild
    }

    public static class RegisteredOperations
    {

        public static Dictionary<Type, Dictionary<Operation, List<MethodInfo>>> AllRegisteredOperations = new Dictionary<Type, Dictionary<Operation, List<MethodInfo>>>();
        public static void RegisterOperation<T>(Operation operation, string methodName)
        {
            var method = typeof(T).GetMethod(methodName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance) ?? throw new Exception("No method found");


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

        public static List<MethodInfo> MethodsForOperation<T>(Operation operation)
        {
            if (!AllRegisteredOperations.TryGetValue(typeof(T), out var methodDict))
            {
                return null;
            }

            if (!methodDict.TryGetValue(operation, out var methods))
            {
                return null;
            }

            return methods;
        }

        public static MethodInfo MethodForOperation<T, C>(Operation operation)
        {
            var methods = MethodsForOperation<T>(operation) ?? throw new ArgumentNullException("methods");

            foreach (var m in methods)
            {
                var parameters = m.GetParameters();
                var hasCriteriaParameter = parameters.Where(p => p.ParameterType == typeof(C)).FirstOrDefault();

                if (hasCriteriaParameter != null)
                {
                    return m;
                }

            }

            return null;
        }

    }

    public class ObjectPortal
    {

        private IServiceScope scope;
        public ObjectPortal(IServiceScope scope)
        {
            this.scope = scope;
        }

        public void UpdateChild<T>(T child)
        {
            var methods = RegisteredOperations.MethodsForOperation<T>(Operation.UpdateChild) ?? throw new Exception("Method not found");
            var invoked = false;

            foreach (var method in methods)
            {
                var success = true;
                var parameters = method.GetParameters().ToList();
                var parameterValues = new object[parameters.Count()];

                for (var i = 0; i < parameterValues.Length; i++)
                {
                    var parameter = parameters[i];
                    if (!scope.IsRegistered(parameter.ParameterType))
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
                        parameterValues[i] = scope.Resolve(parameter.ParameterType);
                    }

                    invoked = true;
                    method.Invoke(child, parameterValues);
                    break;
                }
            }

            if(!invoked)
            {
                throw new Exception("Method not found");
            }
        }

        public void UpdateChild<T, C>(T child, C criteria)
        {
            var method = RegisteredOperations.MethodForOperation<T, C>(Operation.UpdateChild) ?? throw new Exception("Method not found");

            var parameters = method.GetParameters().ToList();
            var parameterValues = new object[parameters.Count()];

            for (var i = 0; i < parameterValues.Length; i++)
            {
                var parameter = parameters[i];
                if (parameter.ParameterType == typeof(C))
                {
                    parameterValues[i] = criteria;
                }
                else
                {
                    if (scope.TryResolve(parameter.ParameterType, out var pv))
                    {
                        parameterValues[i] = pv;
                    }
                }
            }

            method.Invoke(child, parameterValues);

        }
    }


}
