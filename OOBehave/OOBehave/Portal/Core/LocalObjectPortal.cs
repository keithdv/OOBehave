﻿using OOBehave.AuthorizationRules;
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
        where T : IPortalEditTarget, IEditMetaProperties
    {

        public LocalSendReceivePortal(IServiceScope scope)
            : base(scope)
        {
        }

        public async Task Update(T target)
        {
            if (target.IsDeleted)
            {
                if (!target.IsNew)
                {
                    await CallOperationMethod(target, PortalOperation.Delete);
                }
            }
            else if (target.IsNew)
            {
                await CallOperationMethod(target, PortalOperation.Insert);
            }
            else
            {
                await CallOperationMethod(target, PortalOperation.Update);
            }
        }

        public async Task Update(T target, object criteria)
        {
            if (target.IsDeleted)
            {
                if (!target.IsNew)
                {
                    await CallOperationMethod(target, criteria, PortalOperation.Delete);
                }
            }
            else if (target.IsNew)
            {
                await CallOperationMethod(target, criteria, PortalOperation.Insert);
            }
            else
            {
                await CallOperationMethod(target, criteria, PortalOperation.Update);
            }
        }

        public async Task UpdateChild(T child)
        {
            if (child.IsDeleted)
            {
                if (!child.IsNew)
                {
                    await CallOperationMethod(child, PortalOperation.DeleteChild);
                }
            }
            else if (child.IsNew)
            {
                await CallOperationMethod(child, PortalOperation.InsertChild);
            }
            else
            {
                await CallOperationMethod(child, PortalOperation.UpdateChild);
            }
        }

        public async Task UpdateChild(T child, object criteria)
        {
            if (child.IsDeleted)
            {
                if (!child.IsNew)
                {
                    await CallOperationMethod(child, criteria, PortalOperation.DeleteChild);
                }
            }
            else if (child.IsNew)
            {
                await CallOperationMethod(child, criteria, PortalOperation.InsertChild);
            }
            else
            {
                await CallOperationMethod(child, criteria, PortalOperation.UpdateChild);
            }
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
