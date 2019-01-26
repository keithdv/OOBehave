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
        private ILifetimeScope scope = AutofacContainer.GetLifetimeScope(Autofac.Portal.UnitTest);
        private ISendReceivePortal<IEditObjectList> portal;
        private IEditObjectList editObjectList;

        [TestInitialize]
        public void TestInitialize()
        {
            portal = scope.Resolve<ISendReceivePortal<IEditObjectList>>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // Make sure only what  is expected to be called was called
            Assert.IsNotNull(editObjectList);
            scope.Dispose();
        }

        [TestMethod]
        public async Task SendReceivePortalList_Create()
        {
            editObjectList = await portal.Create();
            Assert.IsTrue(editObjectList.CreateCalled);
            Assert.IsTrue(editObjectList.IsNew);
            Assert.IsFalse(editObjectList.IsChild);
        }

        [TestMethod]
        public async Task SendReceivePortalList_CreateGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            editObjectList = await portal.Create(crit);
            Assert.AreEqual(crit, editObjectList.GuidCriteria);
            Assert.IsTrue(editObjectList.CreateCalled);
        }

        [TestMethod]
        public async Task SendReceivePortalList_CreateIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            editObjectList = await portal.Create(crit);
            Assert.AreEqual(crit, editObjectList.IntCriteria);
            Assert.IsTrue(editObjectList.CreateCalled);
        }


        [TestMethod]
        public async Task SendReceivePortalList_Fetch()
        {
            editObjectList = await portal.Fetch();
            Assert.IsTrue(editObjectList.ID.HasValue);
            Assert.IsTrue(editObjectList.FetchCalled);
            Assert.IsFalse(editObjectList.IsNew);
            Assert.IsFalse(editObjectList.IsChild);
            Assert.IsFalse(editObjectList.IsModified);
            Assert.IsFalse(editObjectList.IsSelfModified);
            Assert.IsFalse(editObjectList.IsBusy);
            Assert.IsFalse(editObjectList.IsSelfBusy);
        }

        [TestMethod]
        public async Task SendReceivePortalList_FetchGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            editObjectList = await portal.Fetch(crit);
            Assert.AreEqual(crit, editObjectList.GuidCriteria);
            Assert.IsTrue(editObjectList.FetchCalled);
        }

        [TestMethod]
        public async Task SendReceivePortalList_FetchIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            editObjectList = await portal.Fetch(crit);
            Assert.AreEqual(crit, editObjectList.IntCriteria);
            Assert.IsTrue(editObjectList.FetchCalled);
        }



        [TestMethod]
        public async Task SendReceivePortalList_Update()
        {
            editObjectList = await portal.Fetch();
            var id = Guid.NewGuid();
            editObjectList.ID = Guid.NewGuid();
            await portal.Update(editObjectList);
            Assert.AreNotEqual(id, editObjectList.ID);
            Assert.IsTrue(editObjectList.UpdateCalled);
            Assert.IsFalse(editObjectList.IsNew);
            Assert.IsFalse(editObjectList.IsChild);
            Assert.IsFalse(editObjectList.IsModified);
        }

        [TestMethod]
        public async Task SendReceivePortalList_UpdateGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            editObjectList = await portal.Fetch();
            editObjectList.ID = Guid.NewGuid();
            await portal.Update(editObjectList, crit);
            Assert.AreEqual(crit, editObjectList.GuidCriteria);
            Assert.IsTrue(editObjectList.UpdateCalled);
        }

        [TestMethod]
        public async Task SendReceivePortalList_UpdateIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            editObjectList = await portal.Fetch();
            editObjectList.ID = Guid.NewGuid();
            await portal.Update(editObjectList, crit);
            Assert.AreEqual(crit, editObjectList.IntCriteria);
            Assert.IsTrue(editObjectList.UpdateCalled);
        }



        [TestMethod]
        public async Task SendReceivePortalList_Insert()
        {
            editObjectList = await portal.Create();
            editObjectList.ID = Guid.Empty;
            await portal.Update(editObjectList);
            Assert.AreNotEqual(Guid.Empty, editObjectList.ID);
            Assert.IsTrue(editObjectList.InsertCalled);
            Assert.IsFalse(editObjectList.IsNew);
            Assert.IsFalse(editObjectList.IsChild);
            Assert.IsFalse(editObjectList.IsModified);
        }

        [TestMethod]
        public async Task SendReceivePortalList_InsertGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            editObjectList = await portal.Create();
            await portal.Update(editObjectList, crit);
            Assert.IsTrue(editObjectList.InsertCalled);
            Assert.AreEqual(crit, editObjectList.GuidCriteria);
        }

        [TestMethod]
        public async Task SendReceivePortalList_InsertIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            editObjectList = await portal.Create();
            await portal.Update(editObjectList, crit);
            Assert.IsTrue(editObjectList.InsertCalled);
            Assert.AreEqual(crit, editObjectList.IntCriteria);
        }



        [TestMethod]
        public async Task SendReceivePortalList_Delete()
        {
            editObjectList = await portal.Fetch();
            editObjectList.Delete();
            await portal.Update(editObjectList);
            Assert.IsTrue(editObjectList.DeleteCalled);
        }

        [TestMethod]
        public async Task SendReceivePortalList_DeleteGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            editObjectList = await portal.Fetch();
            editObjectList.Delete();
            await portal.Update(editObjectList, crit);
            Assert.IsTrue(editObjectList.DeleteCalled);
            Assert.AreEqual(crit, editObjectList.GuidCriteria);
        }

        [TestMethod]
        public async Task SendReceivePortalList_DeleteIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            editObjectList = await portal.Fetch();
            editObjectList.Delete();
            await portal.Update(editObjectList, crit);
            Assert.IsTrue(editObjectList.DeleteCalled);
            Assert.AreEqual(crit, editObjectList.IntCriteria);
        }

        [TestMethod]
        public async Task SendReceivePortalList_Remove_Save_IsModified()
        {
            editObjectList = await portal.Fetch();
            var child = await editObjectList.CreateAdd();

            child.MarkOld();

            editObjectList.Remove(child);
            await editObjectList.Save();

            Assert.IsFalse(editObjectList.IsModified);
            Assert.IsTrue(child.DeleteChildCalled);
        }

    }
}
