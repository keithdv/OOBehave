using OOBehave.AuthorizationRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.Portal.Core
{


    public class LocalReceivePortal<T> : ObjectPortalBase<T>, IReceivePortal<T>, IReceivePortalChild<T>
        where T : IPortalTarget
    {

        public LocalReceivePortal(IServiceScope scope)
            : base(scope)
        {
        }

        public async Task<T> Create()
        {
            return await CallOperationMethod(PortalOperation.Create, false);
        }

        public async Task<T> Create(object criteria)
        {
            return await CallOperationMethod(criteria, PortalOperation.Create);
        }

        public async Task<T> CreateChild()
        {
            return await CallOperationMethod(PortalOperation.CreateChild, false);
        }

        public async Task<T> CreateChild(object criteria)
        {
            return await CallOperationMethod(criteria, PortalOperation.CreateChild);
        }

        public async Task<T> Fetch()
        {
            return await CallOperationMethod(PortalOperation.Fetch);
        }

        public async Task<T> Fetch(object criteria)
        {
            return await CallOperationMethod(criteria, PortalOperation.Fetch);
        }

        public async Task<T> FetchChild()
        {
            return await CallOperationMethod(PortalOperation.FetchChild);
        }

        public async Task<T> FetchChild(object criteria)
        {
            return await CallOperationMethod(criteria, PortalOperation.FetchChild);
        }

        protected async Task<T> CallOperationMethod(PortalOperation operation, bool throwException = true)
        {
            var target = Scope.Resolve<T>();
            await CallOperationMethod(target, operation, throwException);
            return target;
        }

        protected async Task CallOperationMethod(T target, PortalOperation operation, bool throwException = true)
        {

            var success = await OperationManager.TryCallOperation(target, operation);

            if (!success && throwException)
            {
                throw new OperationMethodCallFailedException($"{operation.ToString()} method with no criteria not found on {target.GetType().FullName}.");
            }

        }

        protected async Task<T> CallOperationMethod(object criteria, PortalOperation operation)
        {
            var target = Scope.Resolve<T>();
            await CallOperationMethod(target, criteria, operation);
            return target;
        }
        protected async Task CallOperationMethod(T target, object criteria, PortalOperation operation)
        {
            if (criteria == null) { throw new ArgumentNullException(nameof(criteria)); }

            var success = await OperationManager.TryCallOperation(target, criteria, operation);

            if (!success)
            {
                throw new OperationMethodCallFailedException($"{operation.ToString()} method on {typeof(T).FullName} with criteria {criteria.GetType().FullName} not found.");
            }

        }

    }


    public class LocalSendReceivePortal<T> : LocalReceivePortal<T>, ISendReceivePortal<T>, ISendReceivePortalChild<T>
        where T : IPortalEditTarget
    {

        public LocalSendReceivePortal(IServiceScope scope)
            : base(scope)
        {
        }

        public async Task Update(T child)
        {
            await CallOperationMethod(child, PortalOperation.Update);
        }

        public async Task Update(T child, object criteria)
        {
            await CallOperationMethod(child, criteria, PortalOperation.Update);
        }


        public async Task UpdateChild(T child)
        {
            await CallOperationMethod(child, PortalOperation.UpdateChild);
        }

        public async Task UpdateChild(T child, object criteria)
        {
            await CallOperationMethod(child, criteria, PortalOperation.UpdateChild);
        }

        public async Task Delete(T child)
        {
            await CallOperationMethod(child, PortalOperation.Delete);
        }

        public async Task Delete(T child, object criteria)
        {
            await CallOperationMethod(child, criteria, PortalOperation.Delete);
        }


        public async Task DeleteChild(T child)
        {
            await CallOperationMethod(child, PortalOperation.DeleteChild);
        }

        public async Task DeleteChild(T child, object criteria)
        {
            await CallOperationMethod(child, criteria, PortalOperation.DeleteChild);
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
