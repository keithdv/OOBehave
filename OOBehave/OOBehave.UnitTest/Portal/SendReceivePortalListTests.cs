using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOBehave.Portal;
using OOBehave.UnitTest.Objects;
using System;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.ObjectPortal
{

    [TestClass]
    public class SendReceivePortalListTests
    {
        private ILifetimeScope scope = AutofacContainer.GetLifetimeScope(true);
        private ISendReceivePortal<IEditObjectList> portal;
        private IEditObjectList editObject;

        [TestInitialize]
        public void TestInitialize()
        {
            portal = scope.Resolve<ISendReceivePortal<IEditObjectList>>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // Make sure only what  is expected to be called was called
            Assert.IsNotNull(editObject);
            scope.Dispose();
        }

        [TestMethod]
        public async Task SendReceivePortalList_Create()
        {
            editObject = await portal.Create();
            Assert.IsTrue(editObject.CreateCalled);
            Assert.IsTrue(editObject.IsNew);
            Assert.IsFalse(editObject.IsChild);
        }

        [TestMethod]
        public async Task SendReceivePortalList_CreateGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            editObject = await portal.Create(crit);
            Assert.AreEqual(crit, editObject.GuidCriteria);
            Assert.IsTrue(editObject.CreateCalled);
        }

        [TestMethod]
        public async Task SendReceivePortalList_CreateIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            editObject = await portal.Create(crit);
            Assert.AreEqual(crit, editObject.IntCriteria);
            Assert.IsTrue(editObject.CreateCalled);
        }


        [TestMethod]
        public async Task SendReceivePortalList_Fetch()
        {
            editObject = await portal.Fetch();
            Assert.IsTrue(editObject.ID.HasValue);
            Assert.IsTrue(editObject.FetchCalled);
            Assert.IsFalse(editObject.IsNew);
            Assert.IsFalse(editObject.IsChild);
            Assert.IsFalse(editObject.IsModified);
            Assert.IsFalse(editObject.IsSelfModified);
            Assert.IsFalse(editObject.IsBusy);
            Assert.IsFalse(editObject.IsSelfBusy);
        }

        [TestMethod]
        public async Task SendReceivePortalList_FetchGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            editObject = await portal.Fetch(crit);
            Assert.AreEqual(crit, editObject.GuidCriteria);
            Assert.IsTrue(editObject.FetchCalled);
        }

        [TestMethod]
        public async Task SendReceivePortalList_FetchIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            editObject = await portal.Fetch(crit);
            Assert.AreEqual(crit, editObject.IntCriteria);
            Assert.IsTrue(editObject.FetchCalled);
        }



        [TestMethod]
        public async Task SendReceivePortalList_Update()
        {
            editObject = await portal.Fetch();
            var id = Guid.NewGuid();
            editObject.ID = Guid.NewGuid();
            await portal.Update(editObject);
            Assert.AreNotEqual(id, editObject.ID);
            Assert.IsTrue(editObject.UpdateCalled);
            Assert.IsFalse(editObject.IsNew);
            Assert.IsFalse(editObject.IsChild);
            Assert.IsFalse(editObject.IsModified);
        }

        [TestMethod]
        public async Task SendReceivePortalList_UpdateGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            editObject = await portal.Fetch();
            editObject.ID = Guid.NewGuid();
            await portal.Update(editObject, crit);
            Assert.AreEqual(crit, editObject.GuidCriteria);
            Assert.IsTrue(editObject.UpdateCalled);
        }

        [TestMethod]
        public async Task SendReceivePortalList_UpdateIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            editObject = await portal.Fetch();
            editObject.ID = Guid.NewGuid();
            await portal.Update(editObject, crit);
            Assert.AreEqual(crit, editObject.IntCriteria);
            Assert.IsTrue(editObject.UpdateCalled);
        }



        [TestMethod]
        public async Task SendReceivePortalList_Insert()
        {
            editObject = await portal.Create();
            editObject.ID = Guid.Empty;
            await portal.Update(editObject);
            Assert.AreNotEqual(Guid.Empty, editObject.ID);
            Assert.IsTrue(editObject.InsertCalled);
            Assert.IsFalse(editObject.IsNew);
            Assert.IsFalse(editObject.IsChild);
            Assert.IsFalse(editObject.IsModified);
        }

        [TestMethod]
        public async Task SendReceivePortalList_InsertGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            editObject = await portal.Create();
            await portal.Update(editObject, crit);
            Assert.IsTrue(editObject.InsertCalled);
            Assert.AreEqual(crit, editObject.GuidCriteria);
        }

        [TestMethod]
        public async Task SendReceivePortalList_InsertIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            editObject = await portal.Create();
            await portal.Update(editObject, crit);
            Assert.IsTrue(editObject.InsertCalled);
            Assert.AreEqual(crit, editObject.IntCriteria);
        }



        [TestMethod]
        public async Task SendReceivePortalList_Delete()
        {
            editObject = await portal.Fetch();
            editObject.Delete();
            await portal.Update(editObject);
            Assert.IsTrue(editObject.DeleteCalled);
        }

        [TestMethod]
        public async Task SendReceivePortalList_DeleteGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            editObject = await portal.Fetch();
            editObject.Delete();
            await portal.Update(editObject, crit);
            Assert.IsTrue(editObject.DeleteCalled);
            Assert.AreEqual(crit, editObject.GuidCriteria);
        }

        [TestMethod]
        public async Task SendReceivePortalList_DeleteIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            editObject = await portal.Fetch();
            editObject.Delete();
            await portal.Update(editObject, crit);
            Assert.IsTrue(editObject.DeleteCalled);
            Assert.AreEqual(crit, editObject.IntCriteria);
        }

    }
}
