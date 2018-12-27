using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOBehave.Portal;
using OOBehave.UnitTest.Objects;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.ObjectPortal
{

    [TestClass]
    public class ReceivePortalChildListTests
    {
        private ILifetimeScope scope = AutofacContainer.GetLifetimeScope();
        private IReceivePortalChild<IBaseObjectList> portal;
        private IBaseObjectList list;

        [TestInitialize]
        public void TestInitialize()
        {
            portal = scope.Resolve<IReceivePortalChild<IBaseObjectList>>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // Make sure only what  is expected to be called was called
            Assert.IsNotNull(list);
            scope.Dispose();
        }

        [TestMethod]
        public async Task ReceivePortalChildList_CreateChild()
        {
            list = await portal.CreateChild();
            Assert.IsTrue(list.CreateChildCalled);
            Assert.IsTrue(list.Single().CreateChildCalled);
        }

        [TestMethod]
        public async Task ReceivePortalChildList_CreateGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            list = await portal.CreateChild(crit);
            Assert.AreEqual(crit, list.GuidCriteria);
            Assert.AreEqual(crit, list.Single().GuidCriteria);
        }

        [TestMethod]
        public async Task ReceivePortalChildList_CreateIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            list = await portal.CreateChild(crit);
            Assert.AreEqual(crit, list.IntCriteria);
            Assert.AreEqual(crit, list.Single().IntCriteria);
        }

        [TestMethod]
        public async Task ReceivePortalChildList_FetchChild()
        {
            list = await portal.FetchChild();
            Assert.IsTrue(list.FetchChildCalled);
            Assert.IsTrue(list.Single().FetchChildCalled);
        }

        [TestMethod]
        public async Task ReceivePortalChildList_FetchGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            list = await portal.FetchChild(crit);
            Assert.AreEqual(crit, list.GuidCriteria);
            Assert.AreEqual(crit, list.Single().GuidCriteria);
        }

        [TestMethod]
        public async Task ReceivePortalChildList_FetchIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            list = await portal.FetchChild(crit);
            Assert.AreEqual(crit, list.IntCriteria);
            Assert.AreEqual(crit, list.Single().IntCriteria);
        }

    }
}
