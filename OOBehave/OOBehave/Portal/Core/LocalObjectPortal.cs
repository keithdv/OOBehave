using OOBehave.AuthorizationRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.Portal.Core
{


    public class LocalReceivePortal<T> : ObjectPortalBase<T>, IReceivePortal<T>
        where T : IBase
    {

        public LocalReceivePortal(IServiceScope scope)
            : base(scope)
        {
        }

        public async Task<T> Create()
        {
            return await CallOperationMethod(Operation.Create, false);
        }

        public async Task<T> Create(object criteria)
        {
            return await CallOperationMethod(criteria, Operation.Create);
        }

        public async Task<T> CreateChild()
        {
            return await CallOperationMethod(Operation.CreateChild, false);
        }

        public async Task<T> CreateChild(object criteria)
        {
            return await CallOperationMethod(criteria, Operation.CreateChild);
        }

        public async Task<T> Fetch()
        {
            return await CallOperationMethod(Operation.Fetch);
        }

        public async Task<T> Fetch(object criteria)
        {
            return await CallOperationMethod(criteria, Operation.Fetch);
        }

        public async Task<T> FetchChild()
        {
            return await CallOperationMethod(Operation.FetchChild);
        }

        public async Task<T> FetchChild(object criteria)
        {
            return await CallOperationMethod(criteria, Operation.FetchChild);
        }

        protected async Task<T> CallOperationMethod(Operation operation, bool throwException = false)
        {
            return await CallOperationMethod(Scope.Resolve<T>(), operation, throwException);
        }

        protected async Task<T> CallOperationMethod(T target, Operation operation, bool throwException = false)
        {
            await CheckAccess(operation.ToAuthorizationOperation());

            var methods = RegisteredOperationManager.MethodsForOperation(operation) ?? new List<MethodInfo>();

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

                    break;
                }
            }

            if (!invoked && throwException)
            {
                throw new OperationMethodCallFailedException($"{operation.ToString()} method with no criteria not found on {target.GetType().FullName}.");
            }

            return target;

        }

        protected async Task<T> CallOperationMethod(object criteria, Operation operation)
        {
            return await CallOperationMethod(Scope.Resolve<T>(), criteria, operation);
        }
        protected async Task<T> CallOperationMethod(T target, object criteria, Operation operation)
        {
            if (criteria == null) { throw new ArgumentNullException(nameof(criteria)); }

            await CheckAccess(operation.ToAuthorizationOperation(), criteria);

            // This needs to be target.GetType() instead of a generic method
            // because T will be an interface but tager.GetType() will be the concrete
            var method = RegisteredOperationManager.MethodForOperation(operation, criteria.GetType());

            if (method == null)
            {
                throw new OperationMethodCallFailedException($"{operation.ToString()} method with criteria {criteria.GetType().FullName} not found.");
            }

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

            return target;
        }

    }


    public class LocalSendReceivePortal<T> : LocalReceivePortal<T>, ISendReceivePortal<T>
        where T : IEditBase
    {

        public LocalSendReceivePortal(IServiceScope scope)
            : base(scope)
        {
        }

        public async Task Update(T child)
        {
            await CallOperationMethod(child, Operation.Update);
        }

        public async Task Update(T child, object criteria)
        {
            await CallOperationMethod(child, criteria, Operation.Update);
        }


        public async Task UpdateChild(T child)
        {
            await CallOperationMethod(child, Operation.UpdateChild);
        }

        public async Task UpdateChild(T child, object criteria)
        {
            await CallOperationMethod(child, criteria, Operation.UpdateChild);
        }

        public async Task Delete(T child)
        {
            await CallOperationMethod(child, Operation.Delete);
        }

        public async Task Delete(T child, object criteria)
        {
            await CallOperationMethod(child, criteria, Operation.Delete);
        }


        public async Task DeleteChild(T child)
        {
            await CallOperationMethod(child, Operation.DeleteChild);
        }

        public async Task DeleteChild(T child, object criteria)
        {
            await CallOperationMethod(child, criteria, Operation.DeleteChild);
        }

    }



    [Serializable]
    public class OperationMethodCallFailedException : Exception
    {
        public OperationMethodCallFailedException() { }
        public OperationMethodCallFailedException(string message) : base(message) { }
        public OperationMethodCallFailedException(string message, Exception inner) : base(message, inner) { }
        protected OperationMethodCallFailedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

}
