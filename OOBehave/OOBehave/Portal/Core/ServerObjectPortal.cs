using OOBehave.AuthorizationRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.Portal.Core
{


    public class ServerReceivePortal<T> : ObjectPortalBase<T>, IReceivePortal<T>, IReceivePortalChild<T>
        where T : IPortalTarget
    {
        public IServiceScope Scope { get; }

        public ServerReceivePortal(IServiceScope scope) : base(scope)
        {
            Scope = scope;
        }

        public async Task<T> Create()
        {
            return await CallOperationMethod(PortalOperation.Create, false).ConfigureAwait(false);
        }
        public async Task<T> Create<C0>(C0 criteria0)
        {
            return await CallOperationMethod(PortalOperation.Create, new object[] { criteria0 }, new Type[] { typeof(C0) }).ConfigureAwait(false);
        }
        public async Task<T> Create<C0, C1>(C0 criteria0, C1 criteria1)
        {
            return await CallOperationMethod(PortalOperation.Create, new object[] { criteria0, criteria1 }, new Type[] { typeof(C0), typeof(C1) }).ConfigureAwait(false);
        }
        public async Task<T> Create<C0, C1, C2>(C0 criteria0, C1 criteria1, C2 criteria2)
        {
            return await CallOperationMethod(PortalOperation.Create, new object[] { criteria0, criteria1, criteria2 }, new Type[] { typeof(C0), typeof(C1), typeof(C2) }).ConfigureAwait(false);
        }
        public async Task<T> Create<C0, C1, C2, C3>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3)
        {
            return await CallOperationMethod(PortalOperation.Create, new object[] { criteria0, criteria1, criteria2, criteria3 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3) }).ConfigureAwait(false);
        }
        public async Task<T> Create<C0, C1, C2, C3, C4>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4)
        {
            return await CallOperationMethod(PortalOperation.Create, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4) }).ConfigureAwait(false);
        }
        public async Task<T> Create<C0, C1, C2, C3, C4, C5>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5)
        {
            return await CallOperationMethod(PortalOperation.Create, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5) }).ConfigureAwait(false);
        }
        public async Task<T> Create<C0, C1, C2, C3, C4, C5, C6>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6)
        {
            return await CallOperationMethod(PortalOperation.Create, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6) }).ConfigureAwait(false);
        }
        public async Task<T> Create<C0, C1, C2, C3, C4, C5, C6, C7>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6, C7 criteria7)
        {
            return await CallOperationMethod(PortalOperation.Create, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6, criteria7 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6), typeof(C7) }).ConfigureAwait(false);
        }
        public async Task<T> Fetch()
        {
            return await CallOperationMethod(PortalOperation.Fetch).ConfigureAwait(false);
        }
        public async Task<T> Fetch<C0>(C0 criteria0)
        {
            return await CallOperationMethod(PortalOperation.Fetch, new object[] { criteria0 }, new Type[] { typeof(C0) }).ConfigureAwait(false);
        }
        public async Task<T> Fetch<C0, C1>(C0 criteria0, C1 criteria1)
        {
            return await CallOperationMethod(PortalOperation.Fetch, new object[] { criteria0, criteria1 }, new Type[] { typeof(C0), typeof(C1) }).ConfigureAwait(false);
        }
        public async Task<T> Fetch<C0, C1, C2>(C0 criteria0, C1 criteria1, C2 criteria2)
        {
            return await CallOperationMethod(PortalOperation.Fetch, new object[] { criteria0, criteria1, criteria2 }, new Type[] { typeof(C0), typeof(C1), typeof(C2) }).ConfigureAwait(false);
        }
        public async Task<T> Fetch<C0, C1, C2, C3>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3)
        {
            return await CallOperationMethod(PortalOperation.Fetch, new object[] { criteria0, criteria1, criteria2, criteria3 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3) }).ConfigureAwait(false);
        }
        public async Task<T> Fetch<C0, C1, C2, C3, C4>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4)
        {
            return await CallOperationMethod(PortalOperation.Fetch, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4) }).ConfigureAwait(false);
        }
        public async Task<T> Fetch<C0, C1, C2, C3, C4, C5>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5)
        {
            return await CallOperationMethod(PortalOperation.Fetch, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5) }).ConfigureAwait(false);
        }
        public async Task<T> Fetch<C0, C1, C2, C3, C4, C5, C6>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6)
        {
            return await CallOperationMethod(PortalOperation.Fetch, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6) }).ConfigureAwait(false);
        }
        public async Task<T> Fetch<C0, C1, C2, C3, C4, C5, C6, C7>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6, C7 criteria7)
        {
            return await CallOperationMethod(PortalOperation.Fetch, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6, criteria7 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6), typeof(C7) }).ConfigureAwait(false);
        }
        public Task<T> CreateChild()
        {
            return CallOperationMethod(PortalOperation.CreateChild, false);
        }
        public Task<T> CreateChild<C0>(C0 criteria0)
        {
            return CallOperationMethod(PortalOperation.CreateChild, new object[] { criteria0 }, new Type[] { typeof(C0) });
        }
        public Task<T> CreateChild<C0, C1>(C0 criteria0, C1 criteria1)
        {
            return CallOperationMethod(PortalOperation.CreateChild, new object[] { criteria0, criteria1 }, new Type[] { typeof(C0), typeof(C1) });
        }
        public Task<T> CreateChild<C0, C1, C2>(C0 criteria0, C1 criteria1, C2 criteria2)
        {
            return CallOperationMethod(PortalOperation.CreateChild, new object[] { criteria0, criteria1, criteria2 }, new Type[] { typeof(C0), typeof(C1), typeof(C2) });
        }
        public Task<T> CreateChild<C0, C1, C2, C3>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3)
        {
            return CallOperationMethod(PortalOperation.CreateChild, new object[] { criteria0, criteria1, criteria2, criteria3 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3) });
        }
        public Task<T> CreateChild<C0, C1, C2, C3, C4>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4)
        {
            return CallOperationMethod(PortalOperation.CreateChild, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4) });
        }
        public Task<T> CreateChild<C0, C1, C2, C3, C4, C5>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5)
        {
            return CallOperationMethod(PortalOperation.CreateChild, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5) });
        }
        public Task<T> CreateChild<C0, C1, C2, C3, C4, C5, C6>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6)
        {
            return CallOperationMethod(PortalOperation.CreateChild, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6) });
        }
        public Task<T> CreateChild<C0, C1, C2, C3, C4, C5, C6, C7>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6, C7 criteria7)
        {
            return CallOperationMethod(PortalOperation.CreateChild, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6, criteria7 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6), typeof(C7) });
        }
        public Task<T> FetchChild()
        {
            return CallOperationMethod(PortalOperation.FetchChild);
        }
        public Task<T> FetchChild<C0>(C0 criteria0)
        {
            return CallOperationMethod(PortalOperation.FetchChild, new object[] { criteria0 }, new Type[] { typeof(C0) });
        }
        public Task<T> FetchChild<C0, C1>(C0 criteria0, C1 criteria1)
        {
            return CallOperationMethod(PortalOperation.FetchChild, new object[] { criteria0, criteria1 }, new Type[] { typeof(C0), typeof(C1) });
        }
        public Task<T> FetchChild<C0, C1, C2>(C0 criteria0, C1 criteria1, C2 criteria2)
        {
            return CallOperationMethod(PortalOperation.FetchChild, new object[] { criteria0, criteria1, criteria2 }, new Type[] { typeof(C0), typeof(C1), typeof(C2) });
        }
        public Task<T> FetchChild<C0, C1, C2, C3>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3)
        {
            return CallOperationMethod(PortalOperation.FetchChild, new object[] { criteria0, criteria1, criteria2, criteria3 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3) });
        }
        public Task<T> FetchChild<C0, C1, C2, C3, C4>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4)
        {
            return CallOperationMethod(PortalOperation.FetchChild, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4) });
        }
        public Task<T> FetchChild<C0, C1, C2, C3, C4, C5>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5)
        {
            return CallOperationMethod(PortalOperation.FetchChild, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5) });
        }
        public Task<T> FetchChild<C0, C1, C2, C3, C4, C5, C6>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6)
        {
            return CallOperationMethod(PortalOperation.FetchChild, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6) });
        }
        public Task<T> FetchChild<C0, C1, C2, C3, C4, C5, C6, C7>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6, C7 criteria7)
        {
            return CallOperationMethod(PortalOperation.FetchChild, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6, criteria7 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6), typeof(C7) });
        }

        protected async Task<T> CallOperationMethod(PortalOperation operation, bool throwException = true)
        {
            var target = Scope.Resolve<T>();
            await CallOperationMethod(Scope, target, operation, throwException).ConfigureAwait(false);
            return target;
        }


        protected async Task<T> CallOperationMethod(PortalOperation operation, object[] criteria, Type[] criteriaTypes)
        {
            var target = Scope.Resolve<T>();
            await CallOperationMethod(Scope, target, operation, criteria, criteriaTypes).ConfigureAwait(false);
            return target;
        }


    }


    public class ServerSendReceivePortal<T> : ServerReceivePortal<T>, ISendReceivePortal<T>, ISendReceivePortalChild<T>
        where T : IPortalEditTarget, IEditMetaProperties
    {

        public ServerSendReceivePortal(IServiceScope scope)
            : base(scope)
        {
            Scope = scope;
        }

        public IServiceScope Scope { get; }

        public async Task Update(T target)
        {

            {
                if (target.IsDeleted)
                {
                    if (!target.IsNew)
                    {
                        await CallOperationMethod(Scope, target, PortalOperation.Delete).ConfigureAwait(false);
                    }
                    await Task.CompletedTask;
                }
                else if (target.IsNew)
                {
                    await CallOperationMethod(Scope, target, PortalOperation.Insert).ConfigureAwait(false);
                }
                else
                {
                    await CallOperationMethod(Scope, target, PortalOperation.Update).ConfigureAwait(false);
                }
            }
        }
        public async Task Update<C0>(T target, C0 criteria0)
        {

            {
                var objectArray = new object[] { criteria0 };
                var typeArray = new Type[] { typeof(C0) };

                if (target.IsDeleted)
                {
                    if (!target.IsNew)
                    {
                        await CallOperationMethod(Scope, target, PortalOperation.Delete, objectArray, typeArray).ConfigureAwait(false);
                    }
                    await Task.CompletedTask;
                }
                else if (target.IsNew)
                {
                    await CallOperationMethod(Scope, target, PortalOperation.Insert, objectArray, typeArray).ConfigureAwait(false);
                }
                else
                {
                    await CallOperationMethod(Scope, target, PortalOperation.Update, objectArray, typeArray).ConfigureAwait(false);
                }
            }
        }
        public async Task Update<C0, C1>(T target, C0 criteria0, C1 criteria1)
        {

            {
                var objectArray = new object[] { criteria0, criteria1 };
                var typeArray = new Type[] { typeof(C0), typeof(C1) };

                if (target.IsDeleted)
                {
                    if (!target.IsNew)
                    {
                        await CallOperationMethod(Scope, target, PortalOperation.Delete, objectArray, typeArray).ConfigureAwait(false);
                    }
                    await Task.CompletedTask;
                }
                else if (target.IsNew)
                {
                    await CallOperationMethod(Scope, target, PortalOperation.Insert, objectArray, typeArray).ConfigureAwait(false);
                }
                else
                {
                    await CallOperationMethod(Scope, target, PortalOperation.Update, objectArray, typeArray).ConfigureAwait(false);
                }
            }
        }
        public async Task Update<C0, C1, C2>(T target, C0 criteria0, C1 criteria1, C2 criteria2)
        {

            {
                var objectArray = new object[] { criteria0, criteria1, criteria2 };
                var typeArray = new Type[] { typeof(C0), typeof(C1), typeof(C2) };

                if (target.IsDeleted)
                {
                    if (!target.IsNew)
                    {
                        await CallOperationMethod(Scope, target, PortalOperation.Delete, objectArray, typeArray).ConfigureAwait(false);
                    }
                    await Task.CompletedTask;
                }
                else if (target.IsNew)
                {
                    await CallOperationMethod(Scope, target, PortalOperation.Insert, objectArray, typeArray).ConfigureAwait(false);
                }
                else
                {
                    await CallOperationMethod(Scope, target, PortalOperation.Update, objectArray, typeArray).ConfigureAwait(false);
                }
            }
        }
        public async Task Update<C0, C1, C2, C3>(T target, C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3)
        {

            {
                var objectArray = new object[] { criteria0, criteria1, criteria2, criteria3 };
                var typeArray = new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3) };

                if (target.IsDeleted)
                {
                    if (!target.IsNew)
                    {
                        await CallOperationMethod(Scope, target, PortalOperation.Delete, objectArray, typeArray).ConfigureAwait(false);
                    }
                    await Task.CompletedTask;
                }
                else if (target.IsNew)
                {
                    await CallOperationMethod(Scope, target, PortalOperation.Insert, objectArray, typeArray).ConfigureAwait(false);
                }
                else
                {
                    await CallOperationMethod(Scope, target, PortalOperation.Update, objectArray, typeArray).ConfigureAwait(false);
                }
            }
        }
        public async Task Update<C0, C1, C2, C3, C4>(T target, C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4)
        {

            {
                var objectArray = new object[] { criteria0, criteria1, criteria2, criteria3, criteria4 };
                var typeArray = new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4) };

                if (target.IsDeleted)
                {
                    if (!target.IsNew)
                    {
                        await CallOperationMethod(Scope, target, PortalOperation.Delete, objectArray, typeArray).ConfigureAwait(false);
                    }
                    await Task.CompletedTask;
                }
                else if (target.IsNew)
                {
                    await CallOperationMethod(Scope, target, PortalOperation.Insert, objectArray, typeArray).ConfigureAwait(false);
                }
                else
                {
                    await CallOperationMethod(Scope, target, PortalOperation.Update, objectArray, typeArray).ConfigureAwait(false);
                }
            }
        }
        public async Task Update<C0, C1, C2, C3, C4, C5>(T target, C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5)
        {

            {
                var objectArray = new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5 };
                var typeArray = new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5) };

                if (target.IsDeleted)
                {
                    if (!target.IsNew)
                    {
                        await CallOperationMethod(Scope, target, PortalOperation.Delete, objectArray, typeArray).ConfigureAwait(false);
                    }
                    await Task.CompletedTask;
                }
                else if (target.IsNew)
                {
                    await CallOperationMethod(Scope, target, PortalOperation.Insert, objectArray, typeArray).ConfigureAwait(false);
                }
                else
                {
                    await CallOperationMethod(Scope, target, PortalOperation.Update, objectArray, typeArray).ConfigureAwait(false);
                }
            }
        }
        public async Task Update<C0, C1, C2, C3, C4, C5, C6>(T target, C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6)
        {

            {
                var objectArray = new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6 };
                var typeArray = new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6) };

                if (target.IsDeleted)
                {
                    if (!target.IsNew)
                    {
                        await CallOperationMethod(Scope, target, PortalOperation.Delete, objectArray, typeArray).ConfigureAwait(false);
                    }
                    await Task.CompletedTask;
                }
                else if (target.IsNew)
                {
                    await CallOperationMethod(Scope, target, PortalOperation.Insert, objectArray, typeArray).ConfigureAwait(false);
                }
                else
                {
                    await CallOperationMethod(Scope, target, PortalOperation.Update, objectArray, typeArray).ConfigureAwait(false);
                }
            }
        }
        public async Task Update<C0, C1, C2, C3, C4, C5, C6, C7>(T target, C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6, C7 criteria7)
        {

            {
                var objectArray = new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6, criteria7 };
                var typeArray = new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6), typeof(C7) };

                if (target.IsDeleted)
                {
                    if (!target.IsNew)
                    {
                        await CallOperationMethod(Scope, target, PortalOperation.Delete, objectArray, typeArray).ConfigureAwait(false);
                    }
                    await Task.CompletedTask;
                }
                else if (target.IsNew)
                {
                    await CallOperationMethod(Scope, target, PortalOperation.Insert, objectArray, typeArray).ConfigureAwait(false);
                }
                else
                {
                    await CallOperationMethod(Scope, target, PortalOperation.Update, objectArray, typeArray).ConfigureAwait(false);
                }
            }
        }
        public Task UpdateChild(T target)
        {
            if (target.IsDeleted)
            {
                if (!target.IsNew)
                {
                    return CallOperationMethod(Scope, target, PortalOperation.DeleteChild);
                }
                return Task.CompletedTask;
            }
            else if (target.IsNew)
            {
                return CallOperationMethod(Scope, target, PortalOperation.InsertChild);
            }
            else
            {
                return CallOperationMethod(Scope, target, PortalOperation.UpdateChild);
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
                    return CallOperationMethod(Scope, target, PortalOperation.DeleteChild, objectArray, typeArray);
                }
                return Task.CompletedTask;
            }
            else if (target.IsNew)
            {
                return CallOperationMethod(Scope, target, PortalOperation.InsertChild, objectArray, typeArray);
            }
            else
            {
                return CallOperationMethod(Scope, target, PortalOperation.UpdateChild, objectArray, typeArray);
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
                    return CallOperationMethod(Scope, target, PortalOperation.DeleteChild, objectArray, typeArray);
                }
                return Task.CompletedTask;
            }
            else if (target.IsNew)
            {
                return CallOperationMethod(Scope, target, PortalOperation.InsertChild, objectArray, typeArray);
            }
            else
            {
                return CallOperationMethod(Scope, target, PortalOperation.UpdateChild, objectArray, typeArray);
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
                    return CallOperationMethod(Scope, target, PortalOperation.DeleteChild, objectArray, typeArray);
                }
                return Task.CompletedTask;
            }
            else if (target.IsNew)
            {
                return CallOperationMethod(Scope, target, PortalOperation.InsertChild, objectArray, typeArray);
            }
            else
            {
                return CallOperationMethod(Scope, target, PortalOperation.UpdateChild, objectArray, typeArray);
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
                    return CallOperationMethod(Scope, target, PortalOperation.DeleteChild, objectArray, typeArray);
                }
                return Task.CompletedTask;
            }
            else if (target.IsNew)
            {
                return CallOperationMethod(Scope, target, PortalOperation.InsertChild, objectArray, typeArray);
            }
            else
            {
                return CallOperationMethod(Scope, target, PortalOperation.UpdateChild, objectArray, typeArray);
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
                    return CallOperationMethod(Scope, target, PortalOperation.DeleteChild, objectArray, typeArray);
                }
                return Task.CompletedTask;
            }
            else if (target.IsNew)
            {
                return CallOperationMethod(Scope, target, PortalOperation.InsertChild, objectArray, typeArray);
            }
            else
            {
                return CallOperationMethod(Scope, target, PortalOperation.UpdateChild, objectArray, typeArray);
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
                    return CallOperationMethod(Scope, target, PortalOperation.DeleteChild, objectArray, typeArray);
                }
                return Task.CompletedTask;
            }
            else if (target.IsNew)
            {
                return CallOperationMethod(Scope, target, PortalOperation.InsertChild, objectArray, typeArray);
            }
            else
            {
                return CallOperationMethod(Scope, target, PortalOperation.UpdateChild, objectArray, typeArray);
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
                    return CallOperationMethod(Scope, target, PortalOperation.DeleteChild, objectArray, typeArray);
                }
                return Task.CompletedTask;
            }
            else if (target.IsNew)
            {
                return CallOperationMethod(Scope, target, PortalOperation.InsertChild, objectArray, typeArray);
            }
            else
            {
                return CallOperationMethod(Scope, target, PortalOperation.UpdateChild, objectArray, typeArray);
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
                    return CallOperationMethod(Scope, target, PortalOperation.DeleteChild, objectArray, typeArray);
                }
                return Task.CompletedTask;
            }
            else if (target.IsNew)
            {
                return CallOperationMethod(Scope, target, PortalOperation.InsertChild, objectArray, typeArray);
            }
            else
            {
                return CallOperationMethod(Scope, target, PortalOperation.UpdateChild, objectArray, typeArray);
            }
        }

        protected Task CallOperationMethod(T target, PortalOperation operation, object[] criteria, Type[] criteriaTypes)
        {
            return base.CallOperationMethod(Scope, target, operation, criteria, criteriaTypes);
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
