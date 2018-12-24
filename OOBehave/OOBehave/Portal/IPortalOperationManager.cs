using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace OOBehave.Portal
{

    public interface IPortalOperationManager
    {
        void RegisterOperation(PortalOperation operation, string methodName);
        void RegisterOperation(PortalOperation operation, MethodInfo method);
        Task<bool> TryCallOperation(object target, PortalOperation operation);
        Task<bool> TryCallOperation(object target, object criteria, PortalOperation operation);
    }
    public interface IPortalOperationManager<T> : IPortalOperationManager
    {

    }
}