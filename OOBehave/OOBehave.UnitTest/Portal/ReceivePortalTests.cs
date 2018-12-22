using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOBehave.Portal;
using OOBehave.UnitTest.Objects;
using System;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.ObjectPortal
{

    [TestClass]
    public class ReceivePortalTests
    {
        private ILifetimeScope scope = AutofacContainer.GetLifetimeScope();
        private IReceivePortal<IReadOnlyObject> portal;
        private IReadOnlyObject domainObject;

        [TestInitialize]
        public void TestInitialize()
        {
            portal = scope.Resolve<IReceivePortal<IReadOnlyObject>>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // Make sure only what  is expected to be called was called
            Assert.IsNotNull(domainObject);
            scope.Dispose();
        }

        [TestMethod]
        public async Task ReceivePortal_Create()
        {
            domainObject = await portal.Create();
            Assert.IsTrue(domainObject.CreateCalled);
        }

        [TestMethod]
        public async Task ReceivePortal_CreateGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            domainObject = await portal.Create(crit);
            Assert.AreEqual(crit, domainObject.GuidCriteria);
        }

        [TestMethod]
        public async Task ReceivePortal_CreateIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            domainObject = await portal.Create(crit);
            Assert.AreEqual(crit, domainObject.IntCriteria);
        }


        [TestMethod]
        public async Task ReceivePortal_CreateChild()
        {
            domainObject = await portal.CreateChild();
            Assert.IsTrue(domainObject.CreateChildCalled);
        }

        [TestMethod]
        public async Task ReceivePortal_CreateChildGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            domainObject = await portal.CreateChild(crit);
            Assert.AreEqual(crit, domainObject.GuidCriteria);
        }

        [TestMethod]
        public async Task ReceivePortal_CreateChildIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            domainObject = await portal.CreateChild(crit);
            Assert.AreEqual(crit, domainObject.IntCriteria);
        }


        [TestMethod]
        public async Task ReceivePortal_Fetch()
        {
            domainObject = await portal.Fetch();
            Assert.IsTrue(domainObject.FetchCalled);
        }

        [TestMethod]
        public async Task ReceivePortal_FetchGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            domainObject = await portal.Fetch(crit);
            Assert.AreEqual(crit, domainObject.GuidCriteria);
        }

        [TestMethod]
        public async Task ReceivePortal_FetchIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            domainObject = await portal.Fetch(crit);
            Assert.AreEqual(crit, domainObject.IntCriteria);
        }


        [TestMethod]
        public async Task ReceivePortal_FetchChild()
        {
            domainObject = await portal.FetchChild();
            Assert.IsTrue(domainObject.FetchChildCalled);
        }

        [TestMethod]
        public async Task ReceivePortal_FetchChildGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            domainObject = await portal.FetchChild(crit);
            Assert.AreEqual(crit, domainObject.GuidCriteria);
        }

        [TestMethod]
        public async Task ReceivePortal_FetchChildIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            domainObject = await portal.FetchChild(crit);
            Assert.AreEqual(crit, domainObject.IntCriteria);
        }

    }
}
