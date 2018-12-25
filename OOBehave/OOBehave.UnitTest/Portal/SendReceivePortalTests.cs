using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOBehave.Portal;
using OOBehave.UnitTest.Objects;
using System;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.ObjectPortal
{

    [TestClass]
    public class SendReceivePortalTests
    {
        private ILifetimeScope scope = AutofacContainer.GetLifetimeScope();
        private ISendReceivePortal<IEditObject> portal;
        private IEditObject editObject;

        [TestInitialize]
        public void TestInitialize()
        {
            portal = scope.Resolve<ISendReceivePortal<IEditObject>>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // Make sure only what  is expected to be called was called
            Assert.IsNotNull(editObject);
            scope.Dispose();
        }

        [TestMethod]
        public async Task SendReceivePortal_Create()
        {
            editObject = await portal.Create();
            Assert.IsTrue(editObject.CreateCalled);
            Assert.IsTrue(editObject.IsNew);
            Assert.IsFalse(editObject.IsChild);
        }

        [TestMethod]
        public async Task SendReceivePortal_CreateGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            editObject = await portal.Create(crit);
            Assert.AreEqual(crit, editObject.GuidCriteria);
        }

        [TestMethod]
        public async Task SendReceivePortal_CreateIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            editObject = await portal.Create(crit);
            Assert.AreEqual(crit, editObject.IntCriteria);
        }


        [TestMethod]
        public async Task SendReceivePortal_CreateChild()
        {
            editObject = await portal.CreateChild();
            Assert.IsTrue(editObject.CreateChildCalled);
            Assert.IsTrue(editObject.IsNew);
            Assert.IsTrue(editObject.IsChild);
        }

        [TestMethod]
        public async Task SendReceivePortal_CreateChildGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            editObject = await portal.CreateChild(crit);
            Assert.AreEqual(crit, editObject.GuidCriteria);
        }

        [TestMethod]
        public async Task SendReceivePortal_CreateChildIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            editObject = await portal.CreateChild(crit);
            Assert.AreEqual(crit, editObject.IntCriteria);
        }


        [TestMethod]
        public async Task SendReceivePortal_Fetch()
        {
            editObject = await portal.Fetch();
            Assert.IsTrue(editObject.FetchCalled);
            Assert.IsFalse(editObject.IsNew);
            Assert.IsFalse(editObject.IsChild);
        }

        [TestMethod]
        public async Task SendReceivePortal_FetchGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            editObject = await portal.Fetch(crit);
            Assert.AreEqual(crit, editObject.GuidCriteria);
        }

        [TestMethod]
        public async Task SendReceivePortal_FetchIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            editObject = await portal.Fetch(crit);
            Assert.AreEqual(crit, editObject.IntCriteria);
        }


        [TestMethod]
        public async Task SendReceivePortal_FetchChild()
        {
            editObject = await portal.FetchChild();
            Assert.IsTrue(editObject.FetchChildCalled);
            Assert.IsFalse(editObject.IsNew);
            Assert.IsTrue(editObject.IsChild);
        }

        [TestMethod]
        public async Task SendReceivePortal_FetchChildGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            editObject = await portal.FetchChild(crit);
            Assert.AreEqual(crit, editObject.GuidCriteria);
        }

        [TestMethod]
        public async Task SendReceivePortal_FetchChildIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            editObject = await portal.FetchChild(crit);
            Assert.AreEqual(crit, editObject.IntCriteria);
        }

        [TestMethod]
        public async Task SendReceivePortal_Update()
        {
            editObject = await portal.Create();
            editObject.Name = Guid.NewGuid().ToString();
            await portal.Update(editObject);
            Assert.IsTrue(editObject.UpdateCalled);
            Assert.IsFalse(editObject.IsNew);
            Assert.IsFalse(editObject.IsChild);
            Assert.IsFalse(editObject.IsModified);
        }

        [TestMethod]
        public async Task SendReceivePortal_UpdateGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            editObject = await portal.Create();
            await portal.Update(editObject, crit);
            Assert.AreEqual(crit, editObject.GuidCriteria);
        }

        [TestMethod]
        public async Task SendReceivePortal_UpdateIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            editObject = await portal.Create();
            await portal.Update(editObject, crit);
            Assert.AreEqual(crit, editObject.IntCriteria);
        }

        [TestMethod]
        public async Task SendReceivePortal_UpdateChild()
        {
            editObject = await portal.CreateChild();
            await portal.UpdateChild(editObject);
            Assert.IsTrue(editObject.UpdateChildCalled);
            Assert.IsFalse(editObject.IsNew);
            Assert.IsTrue(editObject.IsChild);
        }

        [TestMethod]
        public async Task SendReceivePortal_UpdateChildGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            editObject = await portal.Create();
            await portal.UpdateChild(editObject, crit);
            Assert.AreEqual(crit, editObject.GuidCriteria);
        }

        [TestMethod]
        public async Task SendReceivePortal_UpdateChildIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            editObject = await portal.Create();
            await portal.UpdateChild(editObject, crit);
            Assert.AreEqual(crit, editObject.IntCriteria);
        }


        [TestMethod]
        public async Task SendReceivePortal_Delete()
        {
            editObject = await portal.Create();
            await portal.Delete(editObject);
            Assert.IsTrue(editObject.DeleteCalled);
        }

        [TestMethod]
        public async Task SendReceivePortal_DeleteGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            editObject = await portal.Create();
            await portal.Delete(editObject, crit);
            Assert.AreEqual(crit, editObject.GuidCriteria);
        }

        [TestMethod]
        public async Task SendReceivePortal_DeleteIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            editObject = await portal.Create();
            await portal.Delete(editObject, crit);
            Assert.AreEqual(crit, editObject.IntCriteria);
        }

        [TestMethod]
        public async Task SendReceivePortal_DeleteChild()
        {
            editObject = await portal.Create();
            await portal.DeleteChild(editObject);
            Assert.IsTrue(editObject.DeleteChildCalled);
        }

        [TestMethod]
        public async Task SendReceivePortal_DeleteChildGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            editObject = await portal.Create();
            await portal.DeleteChild(editObject, crit);
            Assert.AreEqual(crit, editObject.GuidCriteria);
        }

        [TestMethod]
        public async Task SendReceivePortal_DeleteChildIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            editObject = await portal.Create();
            await portal.DeleteChild(editObject, crit);
            Assert.AreEqual(crit, editObject.IntCriteria);
        }
    }
}
