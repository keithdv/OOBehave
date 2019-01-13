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
        public async Task<T> Create<C0>(C0 criteria0)
        {
            return await CallOperationMethod(PortalOperation.Create, new object[] { criteria0 }, new Type[] { typeof(C0) });
        }
        public async Task<T> Create<C0, C1>(C0 criteria0, C1 criteria1)
        {
            return await CallOperationMethod(PortalOperation.Create, new object[] { criteria0, criteria1 }, new Type[] { typeof(C0), typeof(C1) });
        }
        public async Task<T> Create<C0, C1, C2>(C0 criteria0, C1 criteria1, C2 criteria2)
        {
            return await CallOperationMethod(PortalOperation.Create, new object[] { criteria0, criteria1, criteria2 }, new Type[] { typeof(C0), typeof(C1), typeof(C2) });
        }
        public async Task<T> Create<C0, C1, C2, C3>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3)
        {
            return await CallOperationMethod(PortalOperation.Create, new object[] { criteria0, criteria1, criteria2, criteria3 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3) });
        }
        public async Task<T> Create<C0, C1, C2, C3, C4>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4)
        {
            return await CallOperationMethod(PortalOperation.Create, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4) });
        }
        public async Task<T> Create<C0, C1, C2, C3, C4, C5>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5)
        {
            return await CallOperationMethod(PortalOperation.Create, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5) });
        }
        public async Task<T> Create<C0, C1, C2, C3, C4, C5, C6>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6)
        {
            return await CallOperationMethod(PortalOperation.Create, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6) });
        }
        public async Task<T> Create<C0, C1, C2, C3, C4, C5, C6, C7>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6, C7 criteria7)
        {
            return await CallOperationMethod(PortalOperation.Create, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6, criteria7 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6), typeof(C7) });
        }
        public async Task<T> Fetch()
        {
            return await CallOperationMethod(PortalOperation.Fetch);
        }
        public async Task<T> Fetch<C0>(C0 criteria0)
        {
            return await CallOperationMethod(PortalOperation.Fetch, new object[] { criteria0 }, new Type[] { typeof(C0) });
        }
        public async Task<T> Fetch<C0, C1>(C0 criteria0, C1 criteria1)
        {
            return await CallOperationMethod(PortalOperation.Fetch, new object[] { criteria0, criteria1 }, new Type[] { typeof(C0), typeof(C1) });
        }
        public async Task<T> Fetch<C0, C1, C2>(C0 criteria0, C1 criteria1, C2 criteria2)
        {
            return await CallOperationMethod(PortalOperation.Fetch, new object[] { criteria0, criteria1, criteria2 }, new Type[] { typeof(C0), typeof(C1), typeof(C2) });
        }
        public async Task<T> Fetch<C0, C1, C2, C3>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3)
        {
            return await CallOperationMethod(PortalOperation.Fetch, new object[] { criteria0, criteria1, criteria2, criteria3 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3) });
        }
        public async Task<T> Fetch<C0, C1, C2, C3, C4>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4)
        {
            return await CallOperationMethod(PortalOperation.Fetch, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4) });
        }
        public async Task<T> Fetch<C0, C1, C2, C3, C4, C5>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5)
        {
            return await CallOperationMethod(PortalOperation.Fetch, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5) });
        }
        public async Task<T> Fetch<C0, C1, C2, C3, C4, C5, C6>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6)
        {
            return await CallOperationMethod(PortalOperation.Fetch, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6) });
        }
        public async Task<T> Fetch<C0, C1, C2, C3, C4, C5, C6, C7>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6, C7 criteria7)
        {
            return await CallOperationMethod(PortalOperation.Fetch, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6, criteria7 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6), typeof(C7) });
        }
        public async Task<T> CreateChild()
        {
            return await CallOperationMethod(PortalOperation.CreateChild);
        }
        public async Task<T> CreateChild<C0>(C0 criteria0)
        {
            return await CallOperationMethod(PortalOperation.CreateChild, new object[] { criteria0 }, new Type[] { typeof(C0) });
        }
        public async Task<T> CreateChild<C0, C1>(C0 criteria0, C1 criteria1)
        {
            return await CallOperationMethod(PortalOperation.CreateChild, new object[] { criteria0, criteria1 }, new Type[] { typeof(C0), typeof(C1) });
        }
        public async Task<T> CreateChild<C0, C1, C2>(C0 criteria0, C1 criteria1, C2 criteria2)
        {
            return await CallOperationMethod(PortalOperation.CreateChild, new object[] { criteria0, criteria1, criteria2 }, new Type[] { typeof(C0), typeof(C1), typeof(C2) });
        }
        public async Task<T> CreateChild<C0, C1, C2, C3>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3)
        {
            return await CallOperationMethod(PortalOperation.CreateChild, new object[] { criteria0, criteria1, criteria2, criteria3 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3) });
        }
        public async Task<T> CreateChild<C0, C1, C2, C3, C4>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4)
        {
            return await CallOperationMethod(PortalOperation.CreateChild, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4) });
        }
        public async Task<T> CreateChild<C0, C1, C2, C3, C4, C5>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5)
        {
            return await CallOperationMethod(PortalOperation.CreateChild, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5) });
        }
        public async Task<T> CreateChild<C0, C1, C2, C3, C4, C5, C6>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6)
        {
            return await CallOperationMethod(PortalOperation.CreateChild, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6) });
        }
        public async Task<T> CreateChild<C0, C1, C2, C3, C4, C5, C6, C7>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6, C7 criteria7)
        {
            return await CallOperationMethod(PortalOperation.CreateChild, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6, criteria7 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6), typeof(C7) });
        }
        public async Task<T> FetchChild()
        {
            return await CallOperationMethod(PortalOperation.FetchChild);
        }
        public async Task<T> FetchChild<C0>(C0 criteria0)
        {
            return await CallOperationMethod(PortalOperation.FetchChild, new object[] { criteria0 }, new Type[] { typeof(C0) });
        }
        public async Task<T> FetchChild<C0, C1>(C0 criteria0, C1 criteria1)
        {
            return await CallOperationMethod(PortalOperation.FetchChild, new object[] { criteria0, criteria1 }, new Type[] { typeof(C0), typeof(C1) });
        }
        public async Task<T> FetchChild<C0, C1, C2>(C0 criteria0, C1 criteria1, C2 criteria2)
        {
            return await CallOperationMethod(PortalOperation.FetchChild, new object[] { criteria0, criteria1, criteria2 }, new Type[] { typeof(C0), typeof(C1), typeof(C2) });
        }
        public async Task<T> FetchChild<C0, C1, C2, C3>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3)
        {
            return await CallOperationMethod(PortalOperation.FetchChild, new object[] { criteria0, criteria1, criteria2, criteria3 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3) });
        }
        public async Task<T> FetchChild<C0, C1, C2, C3, C4>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4)
        {
            return await CallOperationMethod(PortalOperation.FetchChild, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4) });
        }
        public async Task<T> FetchChild<C0, C1, C2, C3, C4, C5>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5)
        {
            return await CallOperationMethod(PortalOperation.FetchChild, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5) });
        }
        public async Task<T> FetchChild<C0, C1, C2, C3, C4, C5, C6>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6)
        {
            return await CallOperationMethod(PortalOperation.FetchChild, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6) });
        }
        public async Task<T> FetchChild<C0, C1, C2, C3, C4, C5, C6, C7>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6, C7 criteria7)
        {
            return await CallOperationMethod(PortalOperation.FetchChild, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6, criteria7 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6), typeof(C7) });
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

        protected async Task<T> CallOperationMethod(PortalOperation operation, object[] criteria, Type[] criteriaTypes)
        {
            var target = Scope.Resolve<T>();
            await CallOperationMethod(target, operation, criteria, criteriaTypes);
            return target;
        }

        protected async Task CallOperationMethod(T target, PortalOperation operation, object[] criteria, Type[] criteriaTypes)
        {
            if (target == null) { throw new ArgumentNullException(nameof(target)); }
            if (criteria == null) { throw new ArgumentNullException(nameof(criteria)); }

            var success = await OperationManager.TryCallOperation(target, operation, criteria, criteriaTypes);

            if (!success)
            {
                throw new OperationMethodCallFailedException($"{operation.ToString()} method on {target.GetType().FullName} with criteria [{string.Join(", ", criteriaTypes.Select(x => x.FullName))}] not found.");
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
        public async Task Update<C0>(T target, C0 criteria0)
        {

            var objectArray = new object[] { criteria0 };
            var typeArray = new Type[] { typeof(C0) };

            if (target.IsDeleted)
            {
                if (!target.IsNew)
                {
                    await CallOperationMethod(target, PortalOperation.Delete, objectArray, typeArray);
                }
            }
            else if (target.IsNew)
            {
                await CallOperationMethod(target, PortalOperation.Insert, objectArray, typeArray);
            }
            else
            {
                await CallOperationMethod(target, PortalOperation.Update, objectArray, typeArray);
            }
        }
        public async Task Update<C0, C1>(T target, C0 criteria0, C1 criteria1)
        {

            var objectArray = new object[] { criteria0, criteria1 };
            var typeArray = new Type[] { typeof(C0), typeof(C1) };

            if (target.IsDeleted)
            {
                if (!target.IsNew)
                {
                    await CallOperationMethod(target, PortalOperation.Delete, objectArray, typeArray);
                }
            }
            else if (target.IsNew)
            {
                await CallOperationMethod(target, PortalOperation.Insert, objectArray, typeArray);
            }
            else
            {
                await CallOperationMethod(target, PortalOperation.Update, objectArray, typeArray);
            }
        }
        public async Task Update<C0, C1, C2>(T target, C0 criteria0, C1 criteria1, C2 criteria2)
        {

            var objectArray = new object[] { criteria0, criteria1, criteria2 };
            var typeArray = new Type[] { typeof(C0), typeof(C1), typeof(C2) };

            if (target.IsDeleted)
            {
                if (!target.IsNew)
                {
                    await CallOperationMethod(target, PortalOperation.Delete, objectArray, typeArray);
                }
            }
            else if (target.IsNew)
            {
                await CallOperationMethod(target, PortalOperation.Insert, objectArray, typeArray);
            }
            else
            {
                await CallOperationMethod(target, PortalOperation.Update, objectArray, typeArray);
            }
        }
        public async Task Update<C0, C1, C2, C3>(T target, C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3)
        {

            var objectArray = new object[] { criteria0, criteria1, criteria2, criteria3 };
            var typeArray = new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3) };

            if (target.IsDeleted)
            {
                if (!target.IsNew)
                {
                    await CallOperationMethod(target, PortalOperation.Delete, objectArray, typeArray);
                }
            }
            else if (target.IsNew)
            {
                await CallOperationMethod(target, PortalOperation.Insert, objectArray, typeArray);
            }
            else
            {
                await CallOperationMethod(target, PortalOperation.Update, objectArray, typeArray);
            }
        }
        public async Task Update<C0, C1, C2, C3, C4>(T target, C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4)
        {

            var objectArray = new object[] { criteria0, criteria1, criteria2, criteria3, criteria4 };
            var typeArray = new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4) };

            if (target.IsDeleted)
            {
                if (!target.IsNew)
                {
                    await CallOperationMethod(target, PortalOperation.Delete, objectArray, typeArray);
                }
            }
            else if (target.IsNew)
            {
                await CallOperationMethod(target, PortalOperation.Insert, objectArray, typeArray);
            }
            else
            {
                await CallOperationMethod(target, PortalOperation.Update, objectArray, typeArray);
            }
        }
        public async Task Update<C0, C1, C2, C3, C4, C5>(T target, C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5)
        {

            var objectArray = new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5 };
            var typeArray = new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5) };

            if (target.IsDeleted)
            {
                if (!target.IsNew)
                {
                    await CallOperationMethod(target, PortalOperation.Delete, objectArray, typeArray);
                }
            }
            else if (target.IsNew)
            {
                await CallOperationMethod(target, PortalOperation.Insert, objectArray, typeArray);
            }
            else
            {
                await CallOperationMethod(target, PortalOperation.Update, objectArray, typeArray);
            }
        }
        public async Task Update<C0, C1, C2, C3, C4, C5, C6>(T target, C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6)
        {

            var objectArray = new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6 };
            var typeArray = new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6) };

            if (target.IsDeleted)
            {
                if (!target.IsNew)
                {
                    await CallOperationMethod(target, PortalOperation.Delete, objectArray, typeArray);
                }
            }
            else if (target.IsNew)
            {
                await CallOperationMethod(target, PortalOperation.Insert, objectArray, typeArray);
            }
            else
            {
                await CallOperationMethod(target, PortalOperation.Update, objectArray, typeArray);
            }
        }
        public async Task Update<C0, C1, C2, C3, C4, C5, C6, C7>(T target, C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6, C7 criteria7)
        {

            var objectArray = new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6, criteria7 };
            var typeArray = new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6), typeof(C7) };

            if (target.IsDeleted)
            {
                if (!target.IsNew)
                {
                    await CallOperationMethod(target, PortalOperation.Delete, objectArray, typeArray);
                }
            }
            else if (target.IsNew)
            {
                await CallOperationMethod(target, PortalOperation.Insert, objectArray, typeArray);
            }
            else
            {
                await CallOperationMethod(target, PortalOperation.Update, objectArray, typeArray);
            }
        }
        public async Task UpdateChild(T target)
        {
            if (target.IsDeleted)
            {
                if (!target.IsNew)
                {
                    await CallOperationMethod(target, PortalOperation.DeleteChild);
                }
            }
            else if (target.IsNew)
            {
                await CallOperationMethod(target, PortalOperation.InsertChild);
            }
            else
            {
                await CallOperationMethod(target, PortalOperation.UpdateChild);
            }
        }
        public async Task UpdateChild<C0>(T target, C0 criteria0)
        {

            var objectArray = new object[] { criteria0 };
            var typeArray = new Type[] { typeof(C0) };

            if (target.IsDeleted)
            {
                if (!target.IsNew)
                {
                    await CallOperationMethod(target, PortalOperation.DeleteChild, objectArray, typeArray);
                }
            }
            else if (target.IsNew)
            {
                await CallOperationMethod(target, PortalOperation.InsertChild, objectArray, typeArray);
            }
            else
            {
                await CallOperationMethod(target, PortalOperation.UpdateChild, objectArray, typeArray);
            }
        }
        public async Task UpdateChild<C0, C1>(T target, C0 criteria0, C1 criteria1)
        {

            var objectArray = new object[] { criteria0, criteria1 };
            var typeArray = new Type[] { typeof(C0), typeof(C1) };

            if (target.IsDeleted)
            {
                if (!target.IsNew)
                {
                    await CallOperationMethod(target, PortalOperation.DeleteChild, objectArray, typeArray);
                }
            }
            else if (target.IsNew)
            {
                await CallOperationMethod(target, PortalOperation.InsertChild, objectArray, typeArray);
            }
            else
            {
                await CallOperationMethod(target, PortalOperation.UpdateChild, objectArray, typeArray);
            }
        }
        public async Task UpdateChild<C0, C1, C2>(T target, C0 criteria0, C1 criteria1, C2 criteria2)
        {

            var objectArray = new object[] { criteria0, criteria1, criteria2 };
            var typeArray = new Type[] { typeof(C0), typeof(C1), typeof(C2) };

            if (target.IsDeleted)
            {
                if (!target.IsNew)
                {
                    await CallOperationMethod(target, PortalOperation.DeleteChild, objectArray, typeArray);
                }
            }
            else if (target.IsNew)
            {
                await CallOperationMethod(target, PortalOperation.InsertChild, objectArray, typeArray);
            }
            else
            {
                await CallOperationMethod(target, PortalOperation.UpdateChild, objectArray, typeArray);
            }
        }
        public async Task UpdateChild<C0, C1, C2, C3>(T target, C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3)
        {

            var objectArray = new object[] { criteria0, criteria1, criteria2, criteria3 };
            var typeArray = new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3) };

            if (target.IsDeleted)
            {
                if (!target.IsNew)
                {
                    await CallOperationMethod(target, PortalOperation.DeleteChild, objectArray, typeArray);
                }
            }
            else if (target.IsNew)
            {
                await CallOperationMethod(target, PortalOperation.InsertChild, objectArray, typeArray);
            }
            else
            {
                await CallOperationMethod(target, PortalOperation.UpdateChild, objectArray, typeArray);
            }
        }
        public async Task UpdateChild<C0, C1, C2, C3, C4>(T target, C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4)
        {

            var objectArray = new object[] { criteria0, criteria1, criteria2, criteria3, criteria4 };
            var typeArray = new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4) };

            if (target.IsDeleted)
            {
                if (!target.IsNew)
                {
                    await CallOperationMethod(target, PortalOperation.DeleteChild, objectArray, typeArray);
                }
            }
            else if (target.IsNew)
            {
                await CallOperationMethod(target, PortalOperation.InsertChild, objectArray, typeArray);
            }
            else
            {
                await CallOperationMethod(target, PortalOperation.UpdateChild, objectArray, typeArray);
            }
        }
        public async Task UpdateChild<C0, C1, C2, C3, C4, C5>(T target, C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5)
        {

            var objectArray = new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5 };
            var typeArray = new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5) };

            if (target.IsDeleted)
            {
                if (!target.IsNew)
                {
                    await CallOperationMethod(target, PortalOperation.DeleteChild, objectArray, typeArray);
                }
            }
            else if (target.IsNew)
            {
                await CallOperationMethod(target, PortalOperation.InsertChild, objectArray, typeArray);
            }
            else
            {
                await CallOperationMethod(target, PortalOperation.UpdateChild, objectArray, typeArray);
            }
        }
        public async Task UpdateChild<C0, C1, C2, C3, C4, C5, C6>(T target, C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6)
        {

            var objectArray = new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6 };
            var typeArray = new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6) };

            if (target.IsDeleted)
            {
                if (!target.IsNew)
                {
                    await CallOperationMethod(target, PortalOperation.DeleteChild, objectArray, typeArray);
                }
            }
            else if (target.IsNew)
            {
                await CallOperationMethod(target, PortalOperation.InsertChild, objectArray, typeArray);
            }
            else
            {
                await CallOperationMethod(target, PortalOperation.UpdateChild, objectArray, typeArray);
            }
        }
        public async Task UpdateChild<C0, C1, C2, C3, C4, C5, C6, C7>(T target, C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6, C7 criteria7)
        {

            var objectArray = new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6, criteria7 };
            var typeArray = new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6), typeof(C7) };

            if (target.IsDeleted)
            {
                if (!target.IsNew)
                {
                    await CallOperationMethod(target, PortalOperation.DeleteChild, objectArray, typeArray);
                }
            }
            else if (target.IsNew)
            {
                await CallOperationMethod(target, PortalOperation.InsertChild, objectArray, typeArray);
            }
            else
            {
                await CallOperationMethod(target, PortalOperation.UpdateChild, objectArray, typeArray);
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
