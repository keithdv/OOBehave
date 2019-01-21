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

        public LocalReceivePortal(IPortalScope portalScope)
            : base(portalScope)
        {
        }

        public Task<T> Create()
        {
            using (PortalOperationScope())
            {
                return CallOperationMethod(PortalOperation.Create, false);
            }
        }
        public Task<T> Create<C0>(C0 criteria0)
        {
            using (PortalOperationScope())
            {
                return CallOperationMethod(PortalOperation.Create, new object[] { criteria0 }, new Type[] { typeof(C0) });
            }
        }
        public Task<T> Create<C0, C1>(C0 criteria0, C1 criteria1)
        {
            using (PortalOperationScope())
            {
                return CallOperationMethod(PortalOperation.Create, new object[] { criteria0, criteria1 }, new Type[] { typeof(C0), typeof(C1) });
            }
        }
        public Task<T> Create<C0, C1, C2>(C0 criteria0, C1 criteria1, C2 criteria2)
        {
            using (PortalOperationScope())
            {
                return CallOperationMethod(PortalOperation.Create, new object[] { criteria0, criteria1, criteria2 }, new Type[] { typeof(C0), typeof(C1), typeof(C2) });
            }
        }
        public Task<T> Create<C0, C1, C2, C3>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3)
        {
            using (PortalOperationScope())
            {
                return CallOperationMethod(PortalOperation.Create, new object[] { criteria0, criteria1, criteria2, criteria3 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3) });
            }
        }
        public Task<T> Create<C0, C1, C2, C3, C4>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4)
        {
            using (PortalOperationScope())
            {
                return CallOperationMethod(PortalOperation.Create, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4) });
            }
        }
        public Task<T> Create<C0, C1, C2, C3, C4, C5>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5)
        {
            using (PortalOperationScope())
            {
                return CallOperationMethod(PortalOperation.Create, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5) });
            }
        }
        public Task<T> Create<C0, C1, C2, C3, C4, C5, C6>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6)
        {
            using (PortalOperationScope())
            {
                return CallOperationMethod(PortalOperation.Create, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6) });
            }
        }
        public Task<T> Create<C0, C1, C2, C3, C4, C5, C6, C7>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6, C7 criteria7)
        {
            using (PortalOperationScope())
            {
                return CallOperationMethod(PortalOperation.Create, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6, criteria7 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6), typeof(C7) });
            }
        }
        public Task<T> Fetch()
        {
            using (PortalOperationScope())
            {
                return CallOperationMethod(PortalOperation.Fetch);
            }
        }
        public Task<T> Fetch<C0>(C0 criteria0)
        {
            using (PortalOperationScope())
            {
                return CallOperationMethod(PortalOperation.Fetch, new object[] { criteria0 }, new Type[] { typeof(C0) });
            }
        }
        public Task<T> Fetch<C0, C1>(C0 criteria0, C1 criteria1)
        {
            using (PortalOperationScope())
            {
                return CallOperationMethod(PortalOperation.Fetch, new object[] { criteria0, criteria1 }, new Type[] { typeof(C0), typeof(C1) });
            }
        }
        public Task<T> Fetch<C0, C1, C2>(C0 criteria0, C1 criteria1, C2 criteria2)
        {
            using (PortalOperationScope())
            {
                return CallOperationMethod(PortalOperation.Fetch, new object[] { criteria0, criteria1, criteria2 }, new Type[] { typeof(C0), typeof(C1), typeof(C2) });
            }
        }
        public Task<T> Fetch<C0, C1, C2, C3>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3)
        {
            using (PortalOperationScope())
            {
                return CallOperationMethod(PortalOperation.Fetch, new object[] { criteria0, criteria1, criteria2, criteria3 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3) });
            }
        }
        public Task<T> Fetch<C0, C1, C2, C3, C4>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4)
        {
            using (PortalOperationScope())
            {
                return CallOperationMethod(PortalOperation.Fetch, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4) });
            }
        }
        public Task<T> Fetch<C0, C1, C2, C3, C4, C5>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5)
        {
            using (PortalOperationScope())
            {
                return CallOperationMethod(PortalOperation.Fetch, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5) });
            }
        }
        public Task<T> Fetch<C0, C1, C2, C3, C4, C5, C6>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6)
        {
            using (PortalOperationScope())
            {
                return CallOperationMethod(PortalOperation.Fetch, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6) });
            }
        }
        public Task<T> Fetch<C0, C1, C2, C3, C4, C5, C6, C7>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6, C7 criteria7)
        {
            using (PortalOperationScope())
            {
                return CallOperationMethod(PortalOperation.Fetch, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6, criteria7 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6), typeof(C7) });
            }
        }
        public Task<T> CreateChild()
        {

            {
                return CallOperationMethod(PortalOperation.CreateChild, false);
            }
        }
        public Task<T> CreateChild<C0>(C0 criteria0)
        {

            {
                return CallOperationMethod(PortalOperation.CreateChild, new object[] { criteria0 }, new Type[] { typeof(C0) });
            }
        }
        public Task<T> CreateChild<C0, C1>(C0 criteria0, C1 criteria1)
        {

            {
                return CallOperationMethod(PortalOperation.CreateChild, new object[] { criteria0, criteria1 }, new Type[] { typeof(C0), typeof(C1) });
            }
        }
        public Task<T> CreateChild<C0, C1, C2>(C0 criteria0, C1 criteria1, C2 criteria2)
        {

            {
                return CallOperationMethod(PortalOperation.CreateChild, new object[] { criteria0, criteria1, criteria2 }, new Type[] { typeof(C0), typeof(C1), typeof(C2) });
            }
        }
        public Task<T> CreateChild<C0, C1, C2, C3>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3)
        {

            {
                return CallOperationMethod(PortalOperation.CreateChild, new object[] { criteria0, criteria1, criteria2, criteria3 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3) });
            }
        }
        public Task<T> CreateChild<C0, C1, C2, C3, C4>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4)
        {

            {
                return CallOperationMethod(PortalOperation.CreateChild, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4) });
            }
        }
        public Task<T> CreateChild<C0, C1, C2, C3, C4, C5>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5)
        {

            {
                return CallOperationMethod(PortalOperation.CreateChild, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5) });
            }
        }
        public Task<T> CreateChild<C0, C1, C2, C3, C4, C5, C6>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6)
        {

            {
                return CallOperationMethod(PortalOperation.CreateChild, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6) });
            }
        }
        public Task<T> CreateChild<C0, C1, C2, C3, C4, C5, C6, C7>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6, C7 criteria7)
        {

            {
                return CallOperationMethod(PortalOperation.CreateChild, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6, criteria7 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6), typeof(C7) });
            }
        }
        public Task<T> FetchChild()
        {

            {
                return CallOperationMethod(PortalOperation.FetchChild);
            }
        }
        public Task<T> FetchChild<C0>(C0 criteria0)
        {

            {
                return CallOperationMethod(PortalOperation.FetchChild, new object[] { criteria0 }, new Type[] { typeof(C0) });
            }
        }
        public Task<T> FetchChild<C0, C1>(C0 criteria0, C1 criteria1)
        {

            {
                return CallOperationMethod(PortalOperation.FetchChild, new object[] { criteria0, criteria1 }, new Type[] { typeof(C0), typeof(C1) });
            }
        }
        public Task<T> FetchChild<C0, C1, C2>(C0 criteria0, C1 criteria1, C2 criteria2)
        {

            {
                return CallOperationMethod(PortalOperation.FetchChild, new object[] { criteria0, criteria1, criteria2 }, new Type[] { typeof(C0), typeof(C1), typeof(C2) });
            }
        }
        public Task<T> FetchChild<C0, C1, C2, C3>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3)
        {

            {
                return CallOperationMethod(PortalOperation.FetchChild, new object[] { criteria0, criteria1, criteria2, criteria3 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3) });
            }
        }
        public Task<T> FetchChild<C0, C1, C2, C3, C4>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4)
        {

            {
                return CallOperationMethod(PortalOperation.FetchChild, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4) });
            }
        }
        public Task<T> FetchChild<C0, C1, C2, C3, C4, C5>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5)
        {

            {
                return CallOperationMethod(PortalOperation.FetchChild, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5) });
            }
        }
        public Task<T> FetchChild<C0, C1, C2, C3, C4, C5, C6>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6)
        {

            {
                return CallOperationMethod(PortalOperation.FetchChild, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6) });
            }
        }
        public Task<T> FetchChild<C0, C1, C2, C3, C4, C5, C6, C7>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6, C7 criteria7)
        {

            {
                return CallOperationMethod(PortalOperation.FetchChild, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6, criteria7 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6), typeof(C7) });
            }
        }

        protected async Task<T> CallOperationMethod(PortalOperation operation, bool throwException = true)
        {
            var target = PortalScope.TargetScope.Resolve<T>();
            await CallOperationMethod(target, operation, throwException).ConfigureAwait(false);
            return target;
        }

        protected async Task CallOperationMethod(T target, PortalOperation operation, bool throwException = true)
        {
            var success = await OperationManager.TryCallOperation(target, operation).ConfigureAwait(false);

            if (!success && throwException)
            {
                throw new OperationMethodCallFailedException($"{operation.ToString()} method with no criteria not found on {target.GetType().FullName}.");
            }
        }

        protected async Task<T> CallOperationMethod(PortalOperation operation, object[] criteria, Type[] criteriaTypes)
        {
            var target = PortalScope.TargetScope.Resolve<T>();
            await CallOperationMethod(target, operation, criteria, criteriaTypes).ConfigureAwait(false);
            return target;
        }

        protected async Task CallOperationMethod(T target, PortalOperation operation, object[] criteria, Type[] criteriaTypes)
        {
            if (target == null) { throw new ArgumentNullException(nameof(target)); }
            if (criteria == null) { throw new ArgumentNullException(nameof(criteria)); }

            var success = await OperationManager.TryCallOperation(target, operation, criteria, criteriaTypes).ConfigureAwait(false);

            if (!success)
            {
                throw new OperationMethodCallFailedException($"{operation.ToString()} method on {target.GetType().FullName} with criteria [{string.Join(", ", criteriaTypes.Select(x => x.FullName))}] not found.");
            }
        }

    }


    public class LocalSendReceivePortal<T> : LocalReceivePortal<T>, ISendReceivePortal<T>, ISendReceivePortalChild<T>
        where T : IPortalEditTarget, IEditMetaProperties
    {

        public LocalSendReceivePortal(IPortalScope portalScope)
            : base(portalScope)
        {
        }

        public Task Update(T target)
        {
            using (PortalOperationScope())
            {
                if (target.IsDeleted)
                {
                    if (!target.IsNew)
                    {
                        return CallOperationMethod(target, PortalOperation.Delete);
                    }
                    return Task.CompletedTask;
                }
                else if (target.IsNew)
                {
                    return CallOperationMethod(target, PortalOperation.Insert);
                }
                else
                {
                    return CallOperationMethod(target, PortalOperation.Update);
                }
            }
        }
        public Task Update<C0>(T target, C0 criteria0)
        {
            using (PortalOperationScope())
            {
                var objectArray = new object[] { criteria0 };
                var typeArray = new Type[] { typeof(C0) };

                if (target.IsDeleted)
                {
                    if (!target.IsNew)
                    {
                        return CallOperationMethod(target, PortalOperation.Delete, objectArray, typeArray);
                    }
                    return Task.CompletedTask;
                }
                else if (target.IsNew)
                {
                    return CallOperationMethod(target, PortalOperation.Insert, objectArray, typeArray);
                }
                else
                {
                    return CallOperationMethod(target, PortalOperation.Update, objectArray, typeArray);
                }
            }
        }
        public Task Update<C0, C1>(T target, C0 criteria0, C1 criteria1)
        {
            using (PortalOperationScope())
            {
                var objectArray = new object[] { criteria0, criteria1 };
                var typeArray = new Type[] { typeof(C0), typeof(C1) };

                if (target.IsDeleted)
                {
                    if (!target.IsNew)
                    {
                        return CallOperationMethod(target, PortalOperation.Delete, objectArray, typeArray);
                    }
                    return Task.CompletedTask;
                }
                else if (target.IsNew)
                {
                    return CallOperationMethod(target, PortalOperation.Insert, objectArray, typeArray);
                }
                else
                {
                    return CallOperationMethod(target, PortalOperation.Update, objectArray, typeArray);
                }
            }
        }
        public Task Update<C0, C1, C2>(T target, C0 criteria0, C1 criteria1, C2 criteria2)
        {
            using (PortalOperationScope())
            {
                var objectArray = new object[] { criteria0, criteria1, criteria2 };
                var typeArray = new Type[] { typeof(C0), typeof(C1), typeof(C2) };

                if (target.IsDeleted)
                {
                    if (!target.IsNew)
                    {
                        return CallOperationMethod(target, PortalOperation.Delete, objectArray, typeArray);
                    }
                    return Task.CompletedTask;
                }
                else if (target.IsNew)
                {
                    return CallOperationMethod(target, PortalOperation.Insert, objectArray, typeArray);
                }
                else
                {
                    return CallOperationMethod(target, PortalOperation.Update, objectArray, typeArray);
                }
            }
        }
        public Task Update<C0, C1, C2, C3>(T target, C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3)
        {
            using (PortalOperationScope())
            {
                var objectArray = new object[] { criteria0, criteria1, criteria2, criteria3 };
                var typeArray = new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3) };

                if (target.IsDeleted)
                {
                    if (!target.IsNew)
                    {
                        return CallOperationMethod(target, PortalOperation.Delete, objectArray, typeArray);
                    }
                    return Task.CompletedTask;
                }
                else if (target.IsNew)
                {
                    return CallOperationMethod(target, PortalOperation.Insert, objectArray, typeArray);
                }
                else
                {
                    return CallOperationMethod(target, PortalOperation.Update, objectArray, typeArray);
                }
            }
        }
        public Task Update<C0, C1, C2, C3, C4>(T target, C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4)
        {
            using (PortalOperationScope())
            {
                var objectArray = new object[] { criteria0, criteria1, criteria2, criteria3, criteria4 };
                var typeArray = new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4) };

                if (target.IsDeleted)
                {
                    if (!target.IsNew)
                    {
                        return CallOperationMethod(target, PortalOperation.Delete, objectArray, typeArray);
                    }
                    return Task.CompletedTask;
                }
                else if (target.IsNew)
                {
                    return CallOperationMethod(target, PortalOperation.Insert, objectArray, typeArray);
                }
                else
                {
                    return CallOperationMethod(target, PortalOperation.Update, objectArray, typeArray);
                }
            }
        }
        public Task Update<C0, C1, C2, C3, C4, C5>(T target, C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5)
        {
            using (PortalOperationScope())
            {
                var objectArray = new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5 };
                var typeArray = new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5) };

                if (target.IsDeleted)
                {
                    if (!target.IsNew)
                    {
                        return CallOperationMethod(target, PortalOperation.Delete, objectArray, typeArray);
                    }
                    return Task.CompletedTask;
                }
                else if (target.IsNew)
                {
                    return CallOperationMethod(target, PortalOperation.Insert, objectArray, typeArray);
                }
                else
                {
                    return CallOperationMethod(target, PortalOperation.Update, objectArray, typeArray);
                }
            }
        }
        public Task Update<C0, C1, C2, C3, C4, C5, C6>(T target, C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6)
        {
            using (PortalOperationScope())
            {
                var objectArray = new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6 };
                var typeArray = new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6) };

                if (target.IsDeleted)
                {
                    if (!target.IsNew)
                    {
                        return CallOperationMethod(target, PortalOperation.Delete, objectArray, typeArray);
                    }
                    return Task.CompletedTask;
                }
                else if (target.IsNew)
                {
                    return CallOperationMethod(target, PortalOperation.Insert, objectArray, typeArray);
                }
                else
                {
                    return CallOperationMethod(target, PortalOperation.Update, objectArray, typeArray);
                }
            }
        }
        public Task Update<C0, C1, C2, C3, C4, C5, C6, C7>(T target, C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6, C7 criteria7)
        {
            using (PortalOperationScope())
            {
                var objectArray = new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6, criteria7 };
                var typeArray = new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6), typeof(C7) };

                if (target.IsDeleted)
                {
                    if (!target.IsNew)
                    {
                        return CallOperationMethod(target, PortalOperation.Delete, objectArray, typeArray);
                    }
                    return Task.CompletedTask;
                }
                else if (target.IsNew)
                {
                    return CallOperationMethod(target, PortalOperation.Insert, objectArray, typeArray);
                }
                else
                {
                    return CallOperationMethod(target, PortalOperation.Update, objectArray, typeArray);
                }
            }
        }
        public Task UpdateChild(T target)
        {
            if (target.IsDeleted)
            {
                if (!target.IsNew)
                {
                    return CallOperationMethod(target, PortalOperation.DeleteChild);
                }
                return Task.CompletedTask;
            }
            else if (target.IsNew)
            {
                return CallOperationMethod(target, PortalOperation.InsertChild);
            }
            else
            {
                return CallOperationMethod(target, PortalOperation.UpdateChild);
            }
        }
        public Task UpdateChild<C0>(T target, C0 criteria0)
        {
            var objectArray = new object[] { criteria0 };
            var typeArray = new Type[] { typeof(C0) };

            if (target.IsDeleted)
            {
                if (!target.IsNew)
                {
                    return CallOperationMethod(target, PortalOperation.DeleteChild, objectArray, typeArray);
                }
                return Task.CompletedTask;
            }
            else if (target.IsNew)
            {
                return CallOperationMethod(target, PortalOperation.InsertChild, objectArray, typeArray);
            }
            else
            {
                return CallOperationMethod(target, PortalOperation.UpdateChild, objectArray, typeArray);
            }
        }
        public Task UpdateChild<C0, C1>(T target, C0 criteria0, C1 criteria1)
        {
            var objectArray = new object[] { criteria0, criteria1 };
            var typeArray = new Type[] { typeof(C0), typeof(C1) };

            if (target.IsDeleted)
            {
                if (!target.IsNew)
                {
                    return CallOperationMethod(target, PortalOperation.DeleteChild, objectArray, typeArray);
                }
                return Task.CompletedTask;
            }
            else if (target.IsNew)
            {
                return CallOperationMethod(target, PortalOperation.InsertChild, objectArray, typeArray);
            }
            else
            {
                return CallOperationMethod(target, PortalOperation.UpdateChild, objectArray, typeArray);
            }
        }
        public Task UpdateChild<C0, C1, C2>(T target, C0 criteria0, C1 criteria1, C2 criteria2)
        {
            var objectArray = new object[] { criteria0, criteria1, criteria2 };
            var typeArray = new Type[] { typeof(C0), typeof(C1), typeof(C2) };

            if (target.IsDeleted)
            {
                if (!target.IsNew)
                {
                    return CallOperationMethod(target, PortalOperation.DeleteChild, objectArray, typeArray);
                }
                return Task.CompletedTask;
            }
            else if (target.IsNew)
            {
                return CallOperationMethod(target, PortalOperation.InsertChild, objectArray, typeArray);
            }
            else
            {
                return CallOperationMethod(target, PortalOperation.UpdateChild, objectArray, typeArray);
            }
        }
        public Task UpdateChild<C0, C1, C2, C3>(T target, C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3)
        {
            var objectArray = new object[] { criteria0, criteria1, criteria2, criteria3 };
            var typeArray = new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3) };

            if (target.IsDeleted)
            {
                if (!target.IsNew)
                {
                    return CallOperationMethod(target, PortalOperation.DeleteChild, objectArray, typeArray);
                }
                return Task.CompletedTask;
            }
            else if (target.IsNew)
            {
                return CallOperationMethod(target, PortalOperation.InsertChild, objectArray, typeArray);
            }
            else
            {
                return CallOperationMethod(target, PortalOperation.UpdateChild, objectArray, typeArray);
            }
        }
        public Task UpdateChild<C0, C1, C2, C3, C4>(T target, C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4)
        {
            var objectArray = new object[] { criteria0, criteria1, criteria2, criteria3, criteria4 };
            var typeArray = new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4) };

            if (target.IsDeleted)
            {
                if (!target.IsNew)
                {
                    return CallOperationMethod(target, PortalOperation.DeleteChild, objectArray, typeArray);
                }
                return Task.CompletedTask;
            }
            else if (target.IsNew)
            {
                return CallOperationMethod(target, PortalOperation.InsertChild, objectArray, typeArray);
            }
            else
            {
                return CallOperationMethod(target, PortalOperation.UpdateChild, objectArray, typeArray);
            }
        }
        public Task UpdateChild<C0, C1, C2, C3, C4, C5>(T target, C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5)
        {
            var objectArray = new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5 };
            var typeArray = new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5) };

            if (target.IsDeleted)
            {
                if (!target.IsNew)
                {
                    return CallOperationMethod(target, PortalOperation.DeleteChild, objectArray, typeArray);
                }
                return Task.CompletedTask;
            }
            else if (target.IsNew)
            {
                return CallOperationMethod(target, PortalOperation.InsertChild, objectArray, typeArray);
            }
            else
            {
                return CallOperationMethod(target, PortalOperation.UpdateChild, objectArray, typeArray);
            }
        }
        public Task UpdateChild<C0, C1, C2, C3, C4, C5, C6>(T target, C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6)
        {
            var objectArray = new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6 };
            var typeArray = new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6) };

            if (target.IsDeleted)
            {
                if (!target.IsNew)
                {
                    return CallOperationMethod(target, PortalOperation.DeleteChild, objectArray, typeArray);
                }
                return Task.CompletedTask;
            }
            else if (target.IsNew)
            {
                return CallOperationMethod(target, PortalOperation.InsertChild, objectArray, typeArray);
            }
            else
            {
                return CallOperationMethod(target, PortalOperation.UpdateChild, objectArray, typeArray);
            }
        }
        public Task UpdateChild<C0, C1, C2, C3, C4, C5, C6, C7>(T target, C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6, C7 criteria7)
        {
            var objectArray = new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6, criteria7 };
            var typeArray = new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6), typeof(C7) };

            if (target.IsDeleted)
            {
                if (!target.IsNew)
                {
                    return CallOperationMethod(target, PortalOperation.DeleteChild, objectArray, typeArray);
                }
                return Task.CompletedTask;
            }
            else if (target.IsNew)
            {
                return CallOperationMethod(target, PortalOperation.InsertChild, objectArray, typeArray);
            }
            else
            {
                return CallOperationMethod(target, PortalOperation.UpdateChild, objectArray, typeArray);
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
