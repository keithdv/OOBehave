using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using OOBehave.Portal;
using OOBehave.Portal.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.Netwonsoft.Json.Test
{
    namespace ServerTests
    {

        public class BaseObject : IPortalTarget
        {

            public Guid ID { get; set; }
            public string Name { get; set; }

            public void StartAllActions() { }

            public Task<IDisposable> StopAllActions() { return null; }

        }

        [TestClass]
        public class ServerTests
        {


            Mock<IServiceScope> scope = new Mock<IServiceScope>();
            private IServer server;

            Mock<IPortalOperationManager<BaseObject>> mockPortalOperationManager = new Mock<IPortalOperationManager<BaseObject>>(MockBehavior.Strict);

            Mock<ISerializer> Serializer { get; set; }
            IZip Compress { get; set; }

            [TestInitialize]
            public void TestInitailize()
            {
                Serializer = new Mock<ISerializer>(MockBehavior.Strict);
                Compress = new Zip();

                Serializer.Setup(x => x.Serialize(It.IsAny<object>())).Returns<object>(o => JsonConvert.SerializeObject(o));
                Serializer.Setup(x => x.Deserialize(It.IsAny<Type>(), It.IsAny<string>())).Returns<Type, string>((t, s) => JsonConvert.DeserializeObject(s, t));

                scope.Setup(x => x.Resolve(typeof(BaseObject))).Returns(new BaseObject());

                server = new Server(scope.Object, t => mockPortalOperationManager.Object, Compress, Serializer.Object);
            }


            private Dictionary<Type, byte[]> CriteriaData<T1, T2>(T1 criteria1, T2 criteria2)
            {
                return new Dictionary<Type, byte[]>() {
                    { typeof(T1), Compress.Compress(JsonConvert.SerializeObject(criteria1)) },
                    { typeof(T2), Compress.Compress(JsonConvert.SerializeObject(criteria2)) }};
            }

            private PortalRequest PortalRequest(PortalOperation operation, object target)
            {
                var request = new PortalRequest() { Operation = PortalOperation.Create, ObjectType = typeof(BaseObject) };
                if (target != null)
                {
                    request.ObjectData = Compress.Compress(JsonConvert.SerializeObject(target));
                }
                return request;
            }

            private PortalRequest PortalRequest<T1, T2>(PortalOperation operation, object target, T1 criteria1, T2 criteria2)
            {
                var request = new PortalRequest()
                {
                    Operation = PortalOperation.Create,
                    ObjectType = typeof(BaseObject),
                    CriteriaData = CriteriaData(criteria1, criteria2)
                };

                if (target != null)
                {
                    request.ObjectData = Compress.Compress(JsonConvert.SerializeObject(target));
                }
                return request;
            }

            private BaseObject PortalResponse(PortalResponse response)
            {
                return JsonConvert.DeserializeObject<BaseObject>(Compress.Decompress(response.ObjectData));
            }

            [TestMethod]
            public async Task Server_Handle_NoTarget()
            {
                mockPortalOperationManager.Setup(x => x.TryCallOperation(It.IsAny<BaseObject>(), PortalOperation.Create)).ReturnsAsync(true);

                var portalRequest = PortalRequest(PortalOperation.Create, null);
                var response = await server.Handle(portalRequest);
                var newTarget = PortalResponse(response);

                Assert.IsNotNull(newTarget);

                mockPortalOperationManager.VerifyAll();
                scope.VerifyAll();
            }

            [TestMethod]
            public async Task Server_Handle_NoTarget_Criteria()
            {
                var id = Guid.NewGuid();
                var name = Guid.NewGuid().ToString();

                mockPortalOperationManager.Setup(x =>
                x.TryCallOperation(It.IsAny<BaseObject>(), PortalOperation.Create,
                        It.Is<object[]>(o => o.Length == 2 && (Guid)o[0] == id && (string)o[1] == name),
                        It.Is<Type[]>(t => t.Length == 2 && t[0] == typeof(Guid) && t[1] == typeof(string))))
                .ReturnsAsync(true);

                var portalRequest = PortalRequest(PortalOperation.Create, null, id, name);
                var response = await server.Handle(portalRequest);
                var newTarget = PortalResponse(response);

                Assert.IsNotNull(newTarget);

                mockPortalOperationManager.VerifyAll();
                scope.VerifyAll();
            }

            [TestMethod]
            public async Task Server_Handle_Target()
            {
                mockPortalOperationManager.Setup(x => x.TryCallOperation(It.IsAny<BaseObject>(), PortalOperation.Create)).ReturnsAsync(true);

                var id = Guid.NewGuid();
                var portalRequest = PortalRequest(PortalOperation.Create, new BaseObject() { ID = id });

                var response = await server.Handle(portalRequest);

                var newTarget = PortalResponse(response);

                Assert.IsNotNull(newTarget);
                Assert.AreEqual(newTarget.ID, id);

                mockPortalOperationManager.VerifyAll();
                scope.Verify(x => x.Resolve(typeof(BaseObject)), Times.Never);

            }

            [TestMethod]
            public async Task Server_Handle_Target_Criteria()
            {
                var id = Guid.NewGuid();
                var name = Guid.NewGuid().ToString();

                mockPortalOperationManager.Setup(x =>
                x.TryCallOperation(It.IsAny<BaseObject>(), PortalOperation.Create,
                        It.Is<object[]>(o => o.Length == 2 && (Guid)o[0] == id && (string)o[1] == name),
                        It.Is<Type[]>(t => t.Length == 2 && t[0] == typeof(Guid) && t[1] == typeof(string))))
                .ReturnsAsync(true);

                var portalRequest = PortalRequest(PortalOperation.Create, new BaseObject() { ID = id }, id, name);
                var response = await server.Handle(portalRequest);
                var newTarget = PortalResponse(response);

                Assert.IsNotNull(newTarget);
                Assert.AreEqual(newTarget.ID, id);

                mockPortalOperationManager.VerifyAll();
                scope.Verify( x=>x.Resolve(typeof(BaseObject)), Times.Never);
            }

        }
    }
}
