using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.Portal.Core
{


    public class LocalReceivePortal<T> : IReceivePortal<T>
        where T : IBase
    {
        protected readonly IServiceScope scope;
        protected readonly IRegisteredOperationManager registeredOperationManager;

        public LocalReceivePortal(IServiceScope scope, IRegisteredOperationManager registeredOperations)
        {
            this.scope = scope;
            this.registeredOperationManager = registeredOperations;
        }

        public async Task<T> Create()
        {
            var target = scope.Resolve<T>();
            await TryCallOperation(target, Operation.Create);
            return target;
        }

        public async Task<T> Create(object criteria)
        {
            if(criteria == null) { throw new ArgumentNullException(nameof(criteria)); }
            var target = scope.Resolve<T>();
            var result = await TryCallOperation(target, criteria, Operation.Create);
            if (!result) { throw new Exception($"Create method with criteria {criteria.GetType().FullName} not found."); }
            return target;
        }

        public async Task<T> CreateChild()
        {
            var target = scope.Resolve<T>();
            await TryCallOperation(target, Operation.CreateChild);
            return target;
        }

        public async Task<T> CreateChild(object criteria)
        {
            if (criteria == null) { throw new ArgumentNullException(nameof(criteria)); }
            var target = scope.Resolve<T>();
            var result = await TryCallOperation(target, criteria, Operation.CreateChild);
            if (!result) { throw new Exception($"CreateChild method with criteria {criteria.GetType().FullName} not found."); }

            return target;
        }

        public async Task<T> Fetch()
        {
            var target = scope.Resolve<T>();
            var result = await TryCallOperation(target, Operation.Fetch);
            if (!result) { throw new Exception("Fetch method with no criteria not found."); }
            return target;
        }

        public async Task<T> Fetch(object criteria)
        {
            var target = scope.Resolve<T>();
            var result = await TryCallOperation(target, criteria, Operation.Fetch);
            if (!result) { throw new Exception($"Fetch method with criteria {criteria.GetType().FullName} not found."); }
            return target;
        }

        public async Task<T> FetchChild()
        {
            var target = scope.Resolve<T>();
            var result = await TryCallOperation(target, Operation.FetchChild);
            if (!result) { throw new Exception($"FetchChild method with no criteria not found."); }
            return target;
        }

        public async Task<T> FetchChild(object criteria)
        {
            if (criteria == null) { throw new ArgumentNullException(nameof(criteria)); }
            var target = scope.Resolve<T>();
            var result = await TryCallOperation(target, criteria, Operation.FetchChild);
            if (!result) { throw new Exception($"FetchChild method with criteria {criteria.GetType().FullName} not found."); }
            return target;
        }

        protected async Task<bool> TryCallOperation(object target, Operation operation)
        {
            var methods = registeredOperationManager.MethodsForOperation(target.GetType(), operation);
            if (methods == null) { return false; }

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

                    var result = method.Invoke(target, parameterValues);
                    if (method.ReturnType == typeof(Task))
                    {
                        await (Task)result;
                    }

                    break;
                }
            }

            return invoked;

        }

        protected async Task<bool> TryCallOperation(object target, object criteria, Operation operation)
        {

            // This needs to be target.GetType() instead of a generic method
            // because T will be an interface but tager.GetType() will be the concrete
            var method = registeredOperationManager.MethodForOperation(target.GetType(), operation, criteria.GetType());

            if (method == null) { return false; }

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
                    if (scope.TryResolve(parameter.ParameterType, out var pv))
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

    }


    public class LocalSendReceivePortal<T> : LocalReceivePortal<T>, ISendReceivePortal<T>
        where T : IEditBase
    {

        public LocalSendReceivePortal(IServiceScope scope, IRegisteredOperationManager registeredOperations) : base(scope, registeredOperations)
        {
        }

        public async Task Update(T child)
        {
            var result = await TryCallOperation(child, Operation.Update);
            if (!result) { throw new UpdateFailedException("Unable to find UpdateMethod with no criteria"); }
        }

        public async Task Update(T child, object criteria)
        {
            var result = await TryCallOperation(child, criteria, Operation.Update);
            if (!result) { throw new UpdateFailedException($"Unable to find UpdateMethod with criteria of {criteria.GetType().FullName}"); }
        }


        public async Task UpdateChild(T child)
        {
            var result = await TryCallOperation(child, Operation.UpdateChild);
            if (!result) { throw new UpdateFailedException("Unable to find UpdateChildMethod with no criteria"); }
        }

        public async Task UpdateChild(T child, object criteria)
        {
            if (criteria == null) { throw new ArgumentNullException(nameof(criteria)); }
            var result = await TryCallOperation(child, criteria, Operation.UpdateChild);
            if (!result) { throw new UpdateFailedException($"Unable to find UpdateChildMethod with criteria of {criteria.GetType().FullName}"); }
        }

        public async Task Delete(T child)
        {
            var result = await TryCallOperation(child, Operation.Delete);
            if (!result) { throw new DeleteFailedException("Unable to find DeleteMethod with no criteria"); }
        }

        public async Task Delete(T child, object criteria)
        {
            if (criteria == null) { throw new ArgumentNullException(nameof(criteria)); }
            var result = await TryCallOperation(child, criteria, Operation.Delete);
            if (!result) { throw new DeleteFailedException($"Unable to find DeleteMethod with criteria of {criteria.GetType().FullName}"); }
        }


        public async Task DeleteChild(T child)
        {
            var result = await TryCallOperation(child, Operation.DeleteChild);
            if (!result) { throw new DeleteFailedException("Unable to find DeleteChildMethod with no criteria"); }
        }

        public async Task DeleteChild(T child, object criteria)
        {
            if (criteria == null) { throw new ArgumentNullException(nameof(criteria)); }
            var result = await TryCallOperation(child, criteria, Operation.DeleteChild);
            if (!result) { throw new DeleteFailedException($"Unable to find DeleteChildMethod with criteria of {criteria.GetType().FullName}"); }
        }

    }

    [Serializable]
    public class FetchFailedException : Exception
    {
        public FetchFailedException() { }
        public FetchFailedException(string message) : base(message) { }
        public FetchFailedException(string message, Exception inner) : base(message, inner) { }
        protected FetchFailedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class UpdateFailedException : Exception
    {
        public UpdateFailedException() { }
        public UpdateFailedException(string message) : base(message) { }
        public UpdateFailedException(string message, Exception inner) : base(message, inner) { }
        protected UpdateFailedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }


    [Serializable]
    public class DeleteFailedException : Exception
    {
        public DeleteFailedException() { }
        public DeleteFailedException(string message) : base(message) { }
        public DeleteFailedException(string message, Exception inner) : base(message, inner) { }
        protected DeleteFailedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

}
