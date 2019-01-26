using OOBehave.AuthorizationRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OOBehave.Portal.Core
{


    public class Client2TierReceivePortal<T> : ObjectPortalBase<T>, IReceivePortal<T>, IReceivePortalChild<T>
        where T : IPortalTarget
    {

        public Client2TierReceivePortal(IServiceScope scope) : base(scope)
        {
            Scope = scope;
        }

        public override IServiceScope UsingOperationScope()
        {
            // Really really tried to do without AsyncLocal here but it works perfect for 2Tier client applications
            // Got stuck on multiple awaits on the same Client2TierReceivePortal object
            // Logically this is what we want - from here on in the call stack use these
            // Even if the same CLient2TierReceivePortal object is used this is OK

            // Only set once even if there are multiple using(UsingOperationScope)
            // Really important - we don't want the services injected on the constructor
            // to be disposed

            if (targetScope.Value == null)
            {
                targetScope.Value = Scope;
            }

            // We DO want this to be disposed
            dependencyScope.Value = Scope.BeginNewScope();
            return DependencyScope;
        }

        public IServiceScope Scope { get; }

        public async Task<T> Create()
        {
            using (var scope = dependencyScope.Value = UsingOperationScope())
            {
                return await CallOperationMethod(PortalOperation.Create, false).ConfigureAwait(false);
            }
        }
        public async Task<T> Create<C0>(C0 criteria0)
        {
            using (var scope = UsingOperationScope())
            {
                return await CallOperationMethod(PortalOperation.Create, new object[] { criteria0 }, new Type[] { typeof(C0) }).ConfigureAwait(false);
            }
        }
        public async Task<T> Create<C0, C1>(C0 criteria0, C1 criteria1)
        {
            using (var scope = UsingOperationScope())
            {
                return await CallOperationMethod(PortalOperation.Create, new object[] { criteria0, criteria1 }, new Type[] { typeof(C0), typeof(C1) }).ConfigureAwait(false);
            }
        }
        public async Task<T> Create<C0, C1, C2>(C0 criteria0, C1 criteria1, C2 criteria2)
        {
            using (var scope = UsingOperationScope())
            {
                return await CallOperationMethod(PortalOperation.Create, new object[] { criteria0, criteria1, criteria2 }, new Type[] { typeof(C0), typeof(C1), typeof(C2) }).ConfigureAwait(false);
            }
        }
        public async Task<T> Create<C0, C1, C2, C3>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3)
        {
            using (var scope = UsingOperationScope())
            {
                return await CallOperationMethod(PortalOperation.Create, new object[] { criteria0, criteria1, criteria2, criteria3 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3) }).ConfigureAwait(false);
            }
        }
        public async Task<T> Create<C0, C1, C2, C3, C4>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4)
        {
            using (var scope = UsingOperationScope())
            {
                return await CallOperationMethod(PortalOperation.Create, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4) }).ConfigureAwait(false);
            }
        }
        public async Task<T> Create<C0, C1, C2, C3, C4, C5>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5)
        {
            using (var scope = UsingOperationScope())
            {
                return await CallOperationMethod(PortalOperation.Create, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5) }).ConfigureAwait(false);
            }
        }
        public async Task<T> Create<C0, C1, C2, C3, C4, C5, C6>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6)
        {
            using (var scope = UsingOperationScope())
            {
                return await CallOperationMethod(PortalOperation.Create, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6) }).ConfigureAwait(false);
            }
        }
        public async Task<T> Create<C0, C1, C2, C3, C4, C5, C6, C7>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6, C7 criteria7)
        {
            using (var scope = UsingOperationScope())
            {
                return await CallOperationMethod(PortalOperation.Create, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6, criteria7 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6), typeof(C7) }).ConfigureAwait(false);
            }
        }
        public async Task<T> Fetch()
        {
            using (var scope = UsingOperationScope())
            {
                return await CallOperationMethod(PortalOperation.Fetch).ConfigureAwait(false);
            }
        }
        public async Task<T> Fetch<C0>(C0 criteria0)
        {
            using (var scope = UsingOperationScope())
            {
                return await CallOperationMethod(PortalOperation.Fetch, new object[] { criteria0 }, new Type[] { typeof(C0) }).ConfigureAwait(false);
            }
        }
        public async Task<T> Fetch<C0, C1>(C0 criteria0, C1 criteria1)
        {
            using (var scope = UsingOperationScope())
            {
                return await CallOperationMethod(PortalOperation.Fetch, new object[] { criteria0, criteria1 }, new Type[] { typeof(C0), typeof(C1) }).ConfigureAwait(false);
            }
        }
        public async Task<T> Fetch<C0, C1, C2>(C0 criteria0, C1 criteria1, C2 criteria2)
        {
            using (var scope = UsingOperationScope())
            {
                return await CallOperationMethod(PortalOperation.Fetch, new object[] { criteria0, criteria1, criteria2 }, new Type[] { typeof(C0), typeof(C1), typeof(C2) }).ConfigureAwait(false);
            }
        }
        public async Task<T> Fetch<C0, C1, C2, C3>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3)
        {
            using (var scope = UsingOperationScope())
            {
                return await CallOperationMethod(PortalOperation.Fetch, new object[] { criteria0, criteria1, criteria2, criteria3 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3) }).ConfigureAwait(false);
            }
        }
        public async Task<T> Fetch<C0, C1, C2, C3, C4>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4)
        {
            using (var scope = UsingOperationScope())
            {
                return await CallOperationMethod(PortalOperation.Fetch, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4) }).ConfigureAwait(false);
            }
        }
        public async Task<T> Fetch<C0, C1, C2, C3, C4, C5>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5)
        {
            using (var scope = UsingOperationScope())
            {
                return await CallOperationMethod(PortalOperation.Fetch, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5) }).ConfigureAwait(false);
            }
        }
        public async Task<T> Fetch<C0, C1, C2, C3, C4, C5, C6>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6)
        {
            using (var scope = UsingOperationScope())
            {
                return await CallOperationMethod(PortalOperation.Fetch, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6) }).ConfigureAwait(false);
            }
        }
        public async Task<T> Fetch<C0, C1, C2, C3, C4, C5, C6, C7>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6, C7 criteria7)
        {
            using (var scope = UsingOperationScope())
            {
                return await CallOperationMethod(PortalOperation.Fetch, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6, criteria7 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6), typeof(C7) }).ConfigureAwait(false);
            }
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
            var target = TargetScope.Resolve<T>();
            await CallOperationMethod(DependencyScope, target, operation, throwException).ConfigureAwait(false);
            return target;
        }

        protected async Task<T> CallOperationMethod(PortalOperation operation, object[] criteria, Type[] criteriaTypes)
        {
            var target = TargetScope.Resolve<T>();
            await CallOperationMethod(DependencyScope, target, operation, criteria, criteriaTypes).ConfigureAwait(false);
            return target;
        }


    }


    public class Client2TierSendReceivePortal<T> : Client2TierReceivePortal<T>, ISendReceivePortal<T>, ISendReceivePortalChild<T>
        where T : IPortalEditTarget, IEditMetaProperties
    {

        public Client2TierSendReceivePortal(IServiceScope portalScope)
            : base(portalScope)
        {
        }

        public async Task Update(T target)
        {
            using (var scope = UsingOperationScope())
            {
                if (target.IsDeleted)
                {
                    if (!target.IsNew)
                    {
                        await CallOperationMethod(DependencyScope, target, PortalOperation.Delete).ConfigureAwait(false);
                    }
                    await Task.CompletedTask;
                }
                else if (target.IsNew)
                {
                    await CallOperationMethod(DependencyScope, target, PortalOperation.Insert).ConfigureAwait(false);
                }
                else
                {
                    await CallOperationMethod(DependencyScope, target, PortalOperation.Update).ConfigureAwait(false);
                }
            }
        }
        public async Task Update<C0>(T target, C0 criteria0)
        {
            using (var scope = UsingOperationScope())
            {
                var objectArray = new object[] { criteria0 };
                var typeArray = new Type[] { typeof(C0) };

                if (target.IsDeleted)
                {
                    if (!target.IsNew)
                    {
                        await CallOperationMethod(DependencyScope, target, PortalOperation.Delete, objectArray, typeArray).ConfigureAwait(false);
                    }
                    await Task.CompletedTask;
                }
                else if (target.IsNew)
                {
                    await CallOperationMethod(DependencyScope, target, PortalOperation.Insert, objectArray, typeArray).ConfigureAwait(false);
                }
                else
                {
                    await CallOperationMethod(DependencyScope, target, PortalOperation.Update, objectArray, typeArray).ConfigureAwait(false);
                }
            }
        }
        public async Task Update<C0, C1>(T target, C0 criteria0, C1 criteria1)
        {
            using (var scope = UsingOperationScope())
            {
                var objectArray = new object[] { criteria0, criteria1 };
                var typeArray = new Type[] { typeof(C0), typeof(C1) };

                if (target.IsDeleted)
                {
                    if (!target.IsNew)
                    {
                        await CallOperationMethod(DependencyScope, target, PortalOperation.Delete, objectArray, typeArray).ConfigureAwait(false);
                    }
                    await Task.CompletedTask;
                }
                else if (target.IsNew)
                {
                    await CallOperationMethod(DependencyScope, target, PortalOperation.Insert, objectArray, typeArray).ConfigureAwait(false);
                }
                else
                {
                    await CallOperationMethod(DependencyScope, target, PortalOperation.Update, objectArray, typeArray).ConfigureAwait(false);
                }
            }
        }
        public async Task Update<C0, C1, C2>(T target, C0 criteria0, C1 criteria1, C2 criteria2)
        {
            using (var scope = UsingOperationScope())
            {
                var objectArray = new object[] { criteria0, criteria1, criteria2 };
                var typeArray = new Type[] { typeof(C0), typeof(C1), typeof(C2) };

                if (target.IsDeleted)
                {
                    if (!target.IsNew)
                    {
                        await CallOperationMethod(DependencyScope, target, PortalOperation.Delete, objectArray, typeArray).ConfigureAwait(false);
                    }
                    await Task.CompletedTask;
                }
                else if (target.IsNew)
                {
                    await CallOperationMethod(DependencyScope, target, PortalOperation.Insert, objectArray, typeArray).ConfigureAwait(false);
                }
                else
                {
                    await CallOperationMethod(DependencyScope, target, PortalOperation.Update, objectArray, typeArray).ConfigureAwait(false);
                }
            }
        }
        public async Task Update<C0, C1, C2, C3>(T target, C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3)
        {
            using (var scope = UsingOperationScope())
            {
                var objectArray = new object[] { criteria0, criteria1, criteria2, criteria3 };
                var typeArray = new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3) };

                if (target.IsDeleted)
                {
                    if (!target.IsNew)
                    {
                        await CallOperationMethod(DependencyScope, target, PortalOperation.Delete, objectArray, typeArray).ConfigureAwait(false);
                    }
                    await Task.CompletedTask;
                }
                else if (target.IsNew)
                {
                    await CallOperationMethod(DependencyScope, target, PortalOperation.Insert, objectArray, typeArray).ConfigureAwait(false);
                }
                else
                {
                    await CallOperationMethod(DependencyScope, target, PortalOperation.Update, objectArray, typeArray).ConfigureAwait(false);
                }
            }
        }
        public async Task Update<C0, C1, C2, C3, C4>(T target, C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4)
        {
            using (var scope = UsingOperationScope())
            {
                var objectArray = new object[] { criteria0, criteria1, criteria2, criteria3, criteria4 };
                var typeArray = new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4) };

                if (target.IsDeleted)
                {
                    if (!target.IsNew)
                    {
                        await CallOperationMethod(DependencyScope, target, PortalOperation.Delete, objectArray, typeArray).ConfigureAwait(false);
                    }
                    await Task.CompletedTask;
                }
                else if (target.IsNew)
                {
                    await CallOperationMethod(DependencyScope, target, PortalOperation.Insert, objectArray, typeArray).ConfigureAwait(false);
                }
                else
                {
                    await CallOperationMethod(DependencyScope, target, PortalOperation.Update, objectArray, typeArray).ConfigureAwait(false);
                }
            }
        }
        public async Task Update<C0, C1, C2, C3, C4, C5>(T target, C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5)
        {
            using (var scope = UsingOperationScope())
            {
                var objectArray = new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5 };
                var typeArray = new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5) };

                if (target.IsDeleted)
                {
                    if (!target.IsNew)
                    {
                        await CallOperationMethod(DependencyScope, target, PortalOperation.Delete, objectArray, typeArray).ConfigureAwait(false);
                    }
                    await Task.CompletedTask;
                }
                else if (target.IsNew)
                {
                    await CallOperationMethod(DependencyScope, target, PortalOperation.Insert, objectArray, typeArray).ConfigureAwait(false);
                }
                else
                {
                    await CallOperationMethod(DependencyScope, target, PortalOperation.Update, objectArray, typeArray).ConfigureAwait(false);
                }
            }
        }
        public async Task Update<C0, C1, C2, C3, C4, C5, C6>(T target, C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6)
        {
            using (var scope = UsingOperationScope())
            {
                var objectArray = new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6 };
                var typeArray = new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6) };

                if (target.IsDeleted)
                {
                    if (!target.IsNew)
                    {
                        await CallOperationMethod(DependencyScope, target, PortalOperation.Delete, objectArray, typeArray).ConfigureAwait(false);
                    }
                    await Task.CompletedTask;
                }
                else if (target.IsNew)
                {
                    await CallOperationMethod(DependencyScope, target, PortalOperation.Insert, objectArray, typeArray).ConfigureAwait(false);
                }
                else
                {
                    await CallOperationMethod(DependencyScope, target, PortalOperation.Update, objectArray, typeArray).ConfigureAwait(false);
                }
            }
        }
        public async Task Update<C0, C1, C2, C3, C4, C5, C6, C7>(T target, C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6, C7 criteria7)
        {
            using (var scope = UsingOperationScope())
            {
                var objectArray = new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6, criteria7 };
                var typeArray = new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6), typeof(C7) };

                if (target.IsDeleted)
                {
                    if (!target.IsNew)
                    {
                        await CallOperationMethod(DependencyScope, target, PortalOperation.Delete, objectArray, typeArray).ConfigureAwait(false);
                    }
                    await Task.CompletedTask;
                }
                else if (target.IsNew)
                {
                    await CallOperationMethod(DependencyScope, target, PortalOperation.Insert, objectArray, typeArray).ConfigureAwait(false);
                }
                else
                {
                    await CallOperationMethod(DependencyScope, target, PortalOperation.Update, objectArray, typeArray).ConfigureAwait(false);
                }
            }
        }
        public Task UpdateChild(T target)
        {
            if (target.IsDeleted)
            {
                if (!target.IsNew)
                {
                    return CallOperationMethod(DependencyScope, target, PortalOperation.DeleteChild);
                }
                return Task.CompletedTask;
            }
            else if (target.IsNew)
            {
                return CallOperationMethod(DependencyScope, target, PortalOperation.InsertChild);
            }
            else
            {
                return CallOperationMethod(DependencyScope, target, PortalOperation.UpdateChild);
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
                    return CallOperationMethod(DependencyScope, target, PortalOperation.DeleteChild, objectArray, typeArray);
                }
                return Task.CompletedTask;
            }
            else if (target.IsNew)
            {
                return CallOperationMethod(DependencyScope, target, PortalOperation.InsertChild, objectArray, typeArray);
            }
            else
            {
                return CallOperationMethod(DependencyScope, target, PortalOperation.UpdateChild, objectArray, typeArray);
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
                    return CallOperationMethod(DependencyScope, target, PortalOperation.DeleteChild, objectArray, typeArray);
                }
                return Task.CompletedTask;
            }
            else if (target.IsNew)
            {
                return CallOperationMethod(DependencyScope, target, PortalOperation.InsertChild, objectArray, typeArray);
            }
            else
            {
                return CallOperationMethod(DependencyScope, target, PortalOperation.UpdateChild, objectArray, typeArray);
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
                    return CallOperationMethod(DependencyScope, target, PortalOperation.DeleteChild, objectArray, typeArray);
                }
                return Task.CompletedTask;
            }
            else if (target.IsNew)
            {
                return CallOperationMethod(DependencyScope, target, PortalOperation.InsertChild, objectArray, typeArray);
            }
            else
            {
                return CallOperationMethod(DependencyScope, target, PortalOperation.UpdateChild, objectArray, typeArray);
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
                    return CallOperationMethod(DependencyScope, target, PortalOperation.DeleteChild, objectArray, typeArray);
                }
                return Task.CompletedTask;
            }
            else if (target.IsNew)
            {
                return CallOperationMethod(DependencyScope, target, PortalOperation.InsertChild, objectArray, typeArray);
            }
            else
            {
                return CallOperationMethod(DependencyScope, target, PortalOperation.UpdateChild, objectArray, typeArray);
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
                    return CallOperationMethod(DependencyScope, target, PortalOperation.DeleteChild, objectArray, typeArray);
                }
                return Task.CompletedTask;
            }
            else if (target.IsNew)
            {
                return CallOperationMethod(DependencyScope, target, PortalOperation.InsertChild, objectArray, typeArray);
            }
            else
            {
                return CallOperationMethod(DependencyScope, target, PortalOperation.UpdateChild, objectArray, typeArray);
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
                    return CallOperationMethod(DependencyScope, target, PortalOperation.DeleteChild, objectArray, typeArray);
                }
                return Task.CompletedTask;
            }
            else if (target.IsNew)
            {
                return CallOperationMethod(DependencyScope, target, PortalOperation.InsertChild, objectArray, typeArray);
            }
            else
            {
                return CallOperationMethod(DependencyScope, target, PortalOperation.UpdateChild, objectArray, typeArray);
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
                    return CallOperationMethod(DependencyScope, target, PortalOperation.DeleteChild, objectArray, typeArray);
                }
                return Task.CompletedTask;
            }
            else if (target.IsNew)
            {
                return CallOperationMethod(DependencyScope, target, PortalOperation.InsertChild, objectArray, typeArray);
            }
            else
            {
                return CallOperationMethod(DependencyScope, target, PortalOperation.UpdateChild, objectArray, typeArray);
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
                    return CallOperationMethod(DependencyScope, target, PortalOperation.DeleteChild, objectArray, typeArray);
                }
                return Task.CompletedTask;
            }
            else if (target.IsNew)
            {
                return CallOperationMethod(DependencyScope, target, PortalOperation.InsertChild, objectArray, typeArray);
            }
            else
            {
                return CallOperationMethod(DependencyScope, target, PortalOperation.UpdateChild, objectArray, typeArray);
            }
        }


    }


}
