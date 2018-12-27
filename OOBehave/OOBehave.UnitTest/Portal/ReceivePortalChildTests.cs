using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOBehave.Portal;
using OOBehave.UnitTest.Objects;
using System;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.ObjectPortal
{

    [TestClass]
    public class ReceivePortalChildTests
    {
        private ILifetimeScope scope = AutofacContainer.GetLifetimeScope();
        private IReceivePortalChild<IBaseObject> portal;
        private IBaseObject domainObject;

        [TestInitialize]
        public void TestInitialize()
        {
            portal = scope.Resolve<IReceivePortalChild<IBaseObject>>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // Make sure only what  is expected to be called was called
            Assert.IsNotNull(domainObject);
            scope.Dispose();
        }

        [TestMethod]
        public async Task ReceivePortalChild_CreateChild()
        {
            domainObject = await portal.CreateChild();
            Assert.IsTrue(domainObject.CreateChildCalled);
        }

        [TestMethod]
        public async Task ReceivePortalChild_CreateChildGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            domainObject = await portal.CreateChild(crit);
            Assert.AreEqual(crit, domainObject.GuidCriteria);
        }

        [TestMethod]
        public async Task ReceivePortalChild_CreateChildIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            domainObject = await portal.CreateChild(crit);
            Assert.AreEqual(crit, domainObject.IntCriteria);
        }

        [TestMethod]
        public async Task ReceivePortalChild_FetchChild()
        {
            domainObject = await portal.FetchChild();
            Assert.IsTrue(domainObject.FetchChildCalled);
        }

        [TestMethod]
        public async Task ReceivePortalChild_FetchChildGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            domainObject = await portal.FetchChild(crit);
            Assert.AreEqual(crit, domainObject.GuidCriteria);
        }

        [TestMethod]
        public async Task ReceivePortalChild_FetchChildIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            domainObject = await portal.FetchChild(crit);
            Assert.AreEqual(crit, domainObject.IntCriteria);
        }

    }
}
