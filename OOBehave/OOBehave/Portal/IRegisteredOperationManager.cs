using System;
using System.Collections.Generic;
using System.Reflection;

namespace OOBehave.Portal
{
    public interface IRegisteredOperationManager
    {
        bool TypeRegistered<T>();
        MethodInfo MethodForOperation(Type targetType, Operation operation, Type criteriaType);
        IEnumerable<MethodInfo> MethodsForOperation(Type targetType, Operation operation);
        void RegisterOperation<T>(Operation operation, string methodName);
        void RegisterOperation<T>(Operation operation, MethodInfo method);
    }
}