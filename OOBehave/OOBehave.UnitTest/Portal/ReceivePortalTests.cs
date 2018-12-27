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
        private IReceivePortal<IBaseObject> portal;
        private IBaseObject domainObject;

        [TestInitialize]
        public void TestInitialize()
        {
            portal = scope.Resolve<IReceivePortal<IBaseObject>>();
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

    }
}
