using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOBehave.Portal;
using OOBehave.UnitTest.Objects;
using System;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.ObjectPortal
{

    [TestClass]
    public class ObjectPortalTests
    {
        private ILifetimeScope scope = AutofacContainer.GetLifetimeScope();
        private IObjectPortal<IDomainObject> portal;
        private IDomainObject domainObject;

        [TestInitialize]
        public void TestInitialize()
        {
            portal = scope.Resolve<IObjectPortal<IDomainObject>>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // Make sure only what  is expected to be called was called
            Assert.IsNotNull(domainObject);
            scope.Dispose();
        }

        [TestMethod]
        public async Task ObjectPortal_Create()
        {
            domainObject = await portal.Create();
            Assert.IsTrue(domainObject.CreateCalled);
        }

        [TestMethod]
        public async Task ObjectPortal_CreateGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            domainObject = await portal.Create(crit);
            Assert.AreEqual(crit, domainObject.GuidCriteria);
        }

        [TestMethod]
        public async Task ObjectPortal_CreateIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            domainObject = await portal.Create(crit);
            Assert.AreEqual(crit, domainObject.IntCriteria);
        }


        [TestMethod]
        public async Task ObjectPortal_CreateChild()
        {
            domainObject = await portal.CreateChild();
            Assert.IsTrue(domainObject.CreateChildCalled);
        }

        [TestMethod]
        public async Task ObjectPortal_CreateChildGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            domainObject = await portal.CreateChild(crit);
            Assert.AreEqual(crit, domainObject.GuidCriteria);
        }

        [TestMethod]
        public async Task ObjectPortal_CreateChildIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            domainObject = await portal.CreateChild(crit);
            Assert.AreEqual(crit, domainObject.IntCriteria);
        }

        [TestMethod]
        public async Task ObjectPortal_Update()
        {
            domainObject = await portal.Create();
            await portal.Update(domainObject);
            Assert.IsTrue(domainObject.UpdateCalled);
        }

        [TestMethod]
        public async Task ObjectPortal_UpdateGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            domainObject = await portal.Create();
            await portal.Update(domainObject, crit);
            Assert.AreEqual(crit, domainObject.GuidCriteria);
        }

        [TestMethod]
        public async Task ObjectPortal_UpdateIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            domainObject = await portal.Create();
            await portal.Update(domainObject, crit);
            Assert.AreEqual(crit, domainObject.IntCriteria);
        }

        [TestMethod]
        public async Task ObjectPortal_UpdateChild()
        {
            domainObject = await portal.Create();
            await portal.UpdateChild(domainObject);
            Assert.IsTrue(domainObject.UpdateChildCalled);
        }

        [TestMethod]
        public async Task ObjectPortal_UpdateChildGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            domainObject = await portal.Create();
            await portal.UpdateChild(domainObject, crit);
            Assert.AreEqual(crit, domainObject.GuidCriteria);
        }

        [TestMethod]
        public async Task ObjectPortal_UpdateChildIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            domainObject = await portal.Create();
            await portal.UpdateChild(domainObject, crit);
            Assert.AreEqual(crit, domainObject.IntCriteria);
        }


        [TestMethod]
        public async Task ObjectPortal_Delete()
        {
            domainObject = await portal.Create();
            await portal.Delete(domainObject);
            Assert.IsTrue(domainObject.DeleteCalled);
        }

        [TestMethod]
        public async Task ObjectPortal_DeleteGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            domainObject = await portal.Create();
            await portal.Delete(domainObject, crit);
            Assert.AreEqual(crit, domainObject.GuidCriteria);
        }

        [TestMethod]
        public async Task ObjectPortal_DeleteIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            domainObject = await portal.Create();
            await portal.Delete(domainObject, crit);
            Assert.AreEqual(crit, domainObject.IntCriteria);
        }

        [TestMethod]
        public async Task ObjectPortal_DeleteChild()
        {
            domainObject = await portal.Create();
            await portal.DeleteChild(domainObject);
            Assert.IsTrue(domainObject.DeleteChildCalled);
        }

        [TestMethod]
        public async Task ObjectPortal_DeleteChildGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            domainObject = await portal.Create();
            await portal.DeleteChild(domainObject, crit);
            Assert.AreEqual(crit, domainObject.GuidCriteria);
        }

        [TestMethod]
        public async Task ObjectPortal_DeleteChildIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            domainObject = await portal.Create();
            await portal.DeleteChild(domainObject, crit);
            Assert.AreEqual(crit, domainObject.IntCriteria);
        }
    }
}
