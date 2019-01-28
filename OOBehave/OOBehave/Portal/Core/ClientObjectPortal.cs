using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.Portal.Core
{
    public class ClientReceivePortal<T> : IReceivePortal<T>, IReceivePortalChild<T>
        where T : class, IPortalTarget
    {

        public ClientReceivePortal(IServiceScope scope, ISerializer serializer, IZip compress, IOOBehaveConfiguration configuration)
        {
            Serializer = serializer;
            Compress = compress;
            Configuration = configuration;
            var type = typeof(T);

            if (type.IsInterface)
            {
                // To find the static method this needs to be the concrete type
                ConcreteType = scope.ConcreteType(type) ?? throw new Exception($"Type {type.FullName} is not registered");
            }
            else
            {
                ConcreteType = type;
            }
        }

        public Type ConcreteType { get; }

        public IServiceScope UsingOperationScope() { return null; }

        public ISerializer Serializer { get; }
        public IZip Compress { get; }
        public IOOBehaveConfiguration Configuration { get; }

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

        protected static HttpClient httpClient = new HttpClient();

        protected virtual HttpClient HttpClient => httpClient;


        protected Task<T> CallOperationMethod(PortalOperation operation, bool throwException = true)
        {
            return CallOperationMethod(operation, null, null, null, throwException);
        }
        protected Task<T> CallOperationMethod(PortalOperation operation, object[] criteria, Type[] criteriaType, bool throwException = true)
        {
            return CallOperationMethod(operation, null, criteria, criteriaType, throwException);
        }
        protected async Task<T> CallOperationMethod(PortalOperation operation, T target, object[] criteria, Type[] criteriaType, bool throwException = true)
        {

            using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, Configuration.PortalURL))
            {

                var portalRequest = new PortalRequest() { Operation = operation, ObjectType = ConcreteType };

                if (target != null)
                {
                    portalRequest.ObjectData = Compress.Compress(Serializer.Serialize(target));
                }

                if (criteria != null)
                {

                    Dictionary<Type, byte[]> criteriaData = new Dictionary<Type, byte[]>();

                    for (var i = 0; i < criteria.Length; i++)
                    {
                        var data = Compress.Compress(Serializer.Serialize(criteria[i]));
                        criteriaData.Add(criteriaType[i], data);
                    }

                    portalRequest.CriteriaData = criteriaData;

                }

                httpRequest.Content = new StringContent(Serializer.Serialize(portalRequest), System.Text.Encoding.UTF8, "application/json");

                var httpResponse = await HttpClient.SendAsync(httpRequest);

                httpResponse.EnsureSuccessStatusCode();

                var response = Serializer.Deserialize<PortalResponse>(await httpResponse.Content.ReadAsStringAsync());

                if (response.Exception != null)
                {
                    Exception dex = null;
                    try
                    {
                        dex = (Exception)Serializer.Deserialize(response.ExceptionType, Compress.Decompress(response.Exception));
                    }
                    catch (Exception ex)
                    {
                        if (!string.IsNullOrWhiteSpace(response.ExceptionMessage))
                        {
                            throw new Exception(response.ExceptionMessage);
                        }
                        else
                        {
                            throw new Exception("Fatal Error: There was an error on the server that cannot be transferred to the client.");
                        }
                    }

                    throw dex;
                }

                T obj = null;

                if (response.ObjectData != null)
                {
                    if (target != null)
                    {
                        Serializer.Populate(Compress.Decompress(response.ObjectData), target);
                    }
                    else
                    {
                        var json = Compress.Decompress(response.ObjectData);
                        obj = (T) Serializer.Deserialize(ConcreteType, json);
                    }
                }

                return obj;
            }
        }
    }

    public class ClientSendReceivePortal<T> : ClientReceivePortal<T>, ISendReceivePortal<T>, ISendReceivePortalChild<T>
        where T : class, IPortalEditTarget, IEditMetaProperties
    {
        public ClientSendReceivePortal(IServiceScope scope, ISerializer serializer, IZip compress, IOOBehaveConfiguration configuration) : base(scope, serializer, compress, configuration)
        {
        }

        public async Task CallOperationMethod(T target, PortalOperation operation)
        {
            var newT = await base.CallOperationMethod(operation, target, null, null, true).ConfigureAwait(false);
        }

        public async Task CallOperationMethod(T target, PortalOperation operation, object[] criteria, Type[] criteriaType)
        {
            var newT = await base.CallOperationMethod(operation, target, criteria, criteriaType, true).ConfigureAwait(false);
        }

        public async Task Update(T target)
        {
            if (target.IsDeleted)
            {
                if (!target.IsNew)
                {
                    await CallOperationMethod(target, PortalOperation.Delete).ConfigureAwait(false);
                }
                await Task.CompletedTask;
            }
            else if (target.IsNew)
            {
                await CallOperationMethod(target, PortalOperation.Insert).ConfigureAwait(false);
            }
            else
            {
                await CallOperationMethod(target, PortalOperation.Update).ConfigureAwait(false);
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
                    await CallOperationMethod(target, PortalOperation.Delete, objectArray, typeArray).ConfigureAwait(false);
                }
                await Task.CompletedTask;
            }
            else if (target.IsNew)
            {
                await CallOperationMethod(target, PortalOperation.Insert, objectArray, typeArray).ConfigureAwait(false);
            }
            else
            {
                await CallOperationMethod(target, PortalOperation.Update, objectArray, typeArray).ConfigureAwait(false);
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
                    await CallOperationMethod(target, PortalOperation.Delete, objectArray, typeArray).ConfigureAwait(false);
                }
                await Task.CompletedTask;
            }
            else if (target.IsNew)
            {
                await CallOperationMethod(target, PortalOperation.Insert, objectArray, typeArray).ConfigureAwait(false);
            }
            else
            {
                await CallOperationMethod(target, PortalOperation.Update, objectArray, typeArray).ConfigureAwait(false);
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
                    await CallOperationMethod(target, PortalOperation.Delete, objectArray, typeArray).ConfigureAwait(false);
                }
                await Task.CompletedTask;
            }
            else if (target.IsNew)
            {
                await CallOperationMethod(target, PortalOperation.Insert, objectArray, typeArray).ConfigureAwait(false);
            }
            else
            {
                await CallOperationMethod(target, PortalOperation.Update, objectArray, typeArray).ConfigureAwait(false);
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
                    await CallOperationMethod(target, PortalOperation.Delete, objectArray, typeArray).ConfigureAwait(false);
                }
                await Task.CompletedTask;
            }
            else if (target.IsNew)
            {
                await CallOperationMethod(target, PortalOperation.Insert, objectArray, typeArray).ConfigureAwait(false);
            }
            else
            {
                await CallOperationMethod(target, PortalOperation.Update, objectArray, typeArray).ConfigureAwait(false);
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
                    await CallOperationMethod(target, PortalOperation.Delete, objectArray, typeArray).ConfigureAwait(false);
                }
                await Task.CompletedTask;
            }
            else if (target.IsNew)
            {
                await CallOperationMethod(target, PortalOperation.Insert, objectArray, typeArray).ConfigureAwait(false);
            }
            else
            {
                await CallOperationMethod(target, PortalOperation.Update, objectArray, typeArray).ConfigureAwait(false);
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
                    await CallOperationMethod(target, PortalOperation.Delete, objectArray, typeArray).ConfigureAwait(false);
                }
                await Task.CompletedTask;
            }
            else if (target.IsNew)
            {
                await CallOperationMethod(target, PortalOperation.Insert, objectArray, typeArray).ConfigureAwait(false);
            }
            else
            {
                await CallOperationMethod(target, PortalOperation.Update, objectArray, typeArray).ConfigureAwait(false);
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
                    await CallOperationMethod(target, PortalOperation.Delete, objectArray, typeArray).ConfigureAwait(false);
                }
                await Task.CompletedTask;
            }
            else if (target.IsNew)
            {
                await CallOperationMethod(target, PortalOperation.Insert, objectArray, typeArray).ConfigureAwait(false);
            }
            else
            {
                await CallOperationMethod(target, PortalOperation.Update, objectArray, typeArray).ConfigureAwait(false);
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
                    await CallOperationMethod(target, PortalOperation.Delete, objectArray, typeArray).ConfigureAwait(false);
                }
                await Task.CompletedTask;
            }
            else if (target.IsNew)
            {
                await CallOperationMethod(target, PortalOperation.Insert, objectArray, typeArray).ConfigureAwait(false);
            }
            else
            {
                await CallOperationMethod(target, PortalOperation.Update, objectArray, typeArray).ConfigureAwait(false);
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
}
