using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOBehave.Portal;
using OOBehave.UnitTest.Objects;
using System;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.ObjectPortal
{

    [TestClass]
    public class SendReceivePortalChildTests
    {
        private ILifetimeScope scope = AutofacContainer.GetLifetimeScope();
        private ISendReceivePortalChild<IEditObject> portal;
        private IEditObject editObject;

        [TestInitialize]
        public void TestInitialize()
        {
            portal = scope.Resolve<ISendReceivePortalChild<IEditObject>>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // Make sure only what  is expected to be called was called
            Assert.IsNotNull(editObject);
            scope.Dispose();
        }

        [TestMethod]
        public async Task SendReceivePortalChild_CreateChild()
        {
            editObject = await portal.CreateChild();
            Assert.IsTrue(editObject.CreateChildCalled);
            Assert.IsTrue(editObject.IsNew);
            Assert.IsTrue(editObject.IsChild);
        }

        [TestMethod]
        public async Task SendReceivePortalChild_CreateChildGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            editObject = await portal.CreateChild(crit);
            Assert.AreEqual(crit, editObject.GuidCriteria);
        }

        [TestMethod]
        public async Task SendReceivePortalChild_CreateChildIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            editObject = await portal.CreateChild(crit);
            Assert.AreEqual(crit, editObject.IntCriteria);
        }




        [TestMethod]
        public async Task SendReceivePortalChild_FetchChild()
        {
            editObject = await portal.FetchChild();
            Assert.IsTrue(editObject.FetchChildCalled);
            Assert.IsFalse(editObject.IsNew);
            Assert.IsTrue(editObject.IsChild);
        }

        [TestMethod]
        public async Task SendReceivePortalChild_FetchChildGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            editObject = await portal.FetchChild(crit);
            Assert.AreEqual(crit, editObject.GuidCriteria);
        }

        [TestMethod]
        public async Task SendReceivePortalChild_FetchChildIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            editObject = await portal.FetchChild(crit);
            Assert.AreEqual(crit, editObject.IntCriteria);
        }

        [TestMethod]
        public async Task SendReceivePortalChild_UpdateChild()
        {
            editObject = await portal.CreateChild();
            await portal.UpdateChild(editObject);
            Assert.IsTrue(editObject.UpdateChildCalled);
            Assert.IsFalse(editObject.IsNew);
            Assert.IsTrue(editObject.IsChild);
        }

        [TestMethod]
        public async Task SendReceivePortalChild_UpdateChildGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            editObject = await portal.CreateChild();
            await portal.UpdateChild(editObject, crit);
            Assert.AreEqual(crit, editObject.GuidCriteria);
        }

        [TestMethod]
        public async Task SendReceivePortalChild_UpdateChildIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            editObject = await portal.CreateChild();
            await portal.UpdateChild(editObject, crit);
            Assert.AreEqual(crit, editObject.IntCriteria);
        }


        [TestMethod]
        public async Task SendReceivePortalChild_DeleteChild()
        {
            editObject = await portal.CreateChild();
            await portal.DeleteChild(editObject);
            Assert.IsTrue(editObject.DeleteChildCalled);
        }

        [TestMethod]
        public async Task SendReceivePortalChild_DeleteChildGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            editObject = await portal.CreateChild();
            await portal.DeleteChild(editObject, crit);
            Assert.AreEqual(crit, editObject.GuidCriteria);
        }

        [TestMethod]
        public async Task SendReceivePortalChild_DeleteChildIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            editObject = await portal.CreateChild();
            await portal.DeleteChild(editObject, crit);
            Assert.AreEqual(crit, editObject.IntCriteria);
        }
    }
}
