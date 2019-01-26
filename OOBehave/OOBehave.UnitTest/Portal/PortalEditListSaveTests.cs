using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOBehave.Portal;
using OOBehave.UnitTest.ObjectPortal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.Portal
{
    [TestClass]
    public class PortalEditListSaveTests
    {

        private ILifetimeScope scope = AutofacContainer.GetLifetimeScope(Autofac.Portal.UnitTest);
        private ISendReceivePortal<IEditObjectList> portal;
        private IEditObjectList list;
        private IEditObject child;

        [TestInitialize]
        public void TestInitialize()
        {
            portal = scope.Resolve<ISendReceivePortal<IEditObjectList>>();
            list = portal.Fetch().Result;
            child = list.CreateAdd().Result;
            child.MarkUnmodified();
            child.MarkOld();

            Assert.IsFalse(list.IsModified);

        }

        [TestCleanup]
        public void TestCleanup()
        {
            scope.Dispose();
        }

        [TestMethod]
        public async Task PortalEditListSave_SaveList()
        {
            list.ID = Guid.Empty;
            await list.Save();
            Assert.AreNotEqual(Guid.Empty, list.ID);
            Assert.IsTrue(list.UpdateCalled);
            Assert.IsFalse(child.UpdateChildCalled);
            Assert.IsFalse(list.IsModified);
        }

        [TestMethod]
        public async Task PortalEditListSave_ChildUpdated()
        {
            child.ID = Guid.Empty;
            await list.Save();
            Assert.AreNotEqual(Guid.Empty, list.ID);
            Assert.IsFalse(list.UpdateCalled);
            Assert.IsTrue(child.UpdateChildCalled);
        }

        [TestMethod]
        public async Task PortalEditListSave_ChildRemoved()
        {
            list.Remove(child);
            await list.Save();
            Assert.IsFalse(list.UpdateCalled); // Update was called but IsSelfModified is false
            Assert.IsTrue(child.DeleteChildCalled);
        }

        [TestMethod]
        public async Task PortalEditListSave_ChildAdded()
        {
            var newChild = await list.CreateAdd();
            await list.Save();
            Assert.IsFalse(list.UpdateCalled); // Update was called but IsSelfModified is false
            Assert.IsFalse(child.InsertChildCalled);
            Assert.IsTrue(newChild.InsertChildCalled);
        }
    }
}
