using System;
using System.Collections.Generic;
using System.Reflection;

namespace OOBehave.Portal
{

    public interface IRegisteredOperationManager
    {
        MethodInfo MethodForOperation(Operation operation, Type criteriaType);
        IEnumerable<MethodInfo> MethodsForOperation(Operation operation);
        void RegisterOperation(Operation operation, string methodName);
        void RegisterOperation(Operation operation, MethodInfo method);

    }
    public interface IRegisteredOperationManager<T> : IRegisteredOperationManager
    {
    }
}