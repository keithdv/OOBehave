using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOBehave.AuthorizationRules;
using OOBehave.Portal;
using OOBehave.UnitTest.Objects;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.ObjectPortal
{

    [TestClass]
    public class ReceivePortalListTests
    {
        private ILifetimeScope scope = AutofacContainer.GetLifetimeScope(Autofac.Portal.UnitTest);
        private IReceivePortal<IBaseObjectList> portal;
        private IBaseObjectList list;

        [TestInitialize]
        public void TestInitialize()
        {
            portal = scope.Resolve<IReceivePortal<IBaseObjectList>>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // Make sure only what  is expected to be called was called
            Assert.IsNotNull(list);
            scope.Dispose();
        }


        [TestMethod]
        public async Task ReceivePortalList_Create()
        {
            list = await portal.Create();
            Assert.IsTrue(list.CreateCalled);
            Assert.IsTrue(list.Single().CreateChildCalled);
        }

        [TestMethod]
        public async Task ReceivePortalList_CreateGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            list = await portal.Create(crit);
            Assert.AreEqual(crit, list.GuidCriteria);
            Assert.AreEqual(crit, list.Single().GuidCriteria);
        }


        [TestMethod]
        public async Task ReceivePortalList_AsyncronousScope()
        {
            using (var s = AutofacContainer.GetLifetimeScope(Autofac.Portal.UnitTest))
            {
                portal = s.Resolve<IReceivePortal<IBaseObjectList>>();
                var crit = Guid.NewGuid();
                list = await portal.Create(crit);
                var ddl = (PortalOperationDisposableDependencyList)list.First().MultipleCriteria[0];
                // PortalOperationDisposableDependencyList is InstancePerLifetimeScope so itshould have kept track of both DisposableDependency objects
                Assert.AreEqual(2, ddl.Count);
            }
        }

        [TestMethod]
        public async Task ReceivePortalList_CreateIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            list = await portal.Create(crit);
            Assert.AreEqual(crit, list.IntCriteria);
            Assert.AreEqual(crit, list.Single().IntCriteria);
        }

        [TestMethod]
        public async Task ReceivePortalList_Fetch()
        {
            list = await portal.Fetch();
            Assert.IsTrue(list.FetchCalled);
            Assert.IsTrue(list.Single().FetchChildCalled);
        }

        [TestMethod]
        public async Task ReceivePortalList_FetchGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            list = await portal.Fetch(crit);
            Assert.AreEqual(crit, list.GuidCriteria);
            Assert.AreEqual(crit, list.Single().GuidCriteria);
        }

        [TestMethod]
        public async Task ReceivePortalList_FetchIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            list = await portal.Fetch(crit);
            Assert.AreEqual(crit, list.IntCriteria);
            Assert.AreEqual(crit, list.Single().IntCriteria);
        }

    }
}
