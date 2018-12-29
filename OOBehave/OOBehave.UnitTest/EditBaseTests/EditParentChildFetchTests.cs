using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOBehave.Portal;
using OOBehave.UnitTest.PersonObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.EditBaseTests
{

    [TestClass]
    public class EditParentChildFetchTests
    {

        private ILifetimeScope scope;
        private IEditPersonParentChild parent;
        private IEditPersonParentChild child;
        private IEditPersonParentChild grandChild;

        [TestInitialize]
        public void TestInitialize()
        {
            scope = AutofacContainer.GetLifetimeScope();
            var parentDto = scope.Resolve<IReadOnlyList<PersonDto>>().Where(p => !p.FatherId.HasValue && !p.MotherId.HasValue).First();

            var portal = scope.Resolve<ISendReceivePortal<IEditPersonParentChild>>();
            parent = portal.Fetch(parentDto).Result;
            child = parent.Child;
            grandChild = child?.Child;
        }

        [TestMethod]
        public void EditParentChildFetchTest_Fetch_InitialMeta()
        {
            void AssertMeta(IEditPersonParentChild t)
            {
                Assert.IsNotNull(t);
                Assert.IsFalse(t.IsModified);
                Assert.IsFalse(t.IsSelfModified);
                Assert.IsFalse(t.IsNew);
                Assert.IsFalse(t.IsSavable);
            }

            AssertMeta(parent);
            AssertMeta(child);
            AssertMeta(grandChild);

            Assert.IsFalse(parent.IsChild);
            Assert.IsTrue(child.IsChild);
            Assert.IsTrue(grandChild.IsChild);

        }

        [TestMethod]
        public async Task EditParentChildFetchTest_ModifyGrandChild_IsModified()
        {

            grandChild.FirstName = Guid.NewGuid().ToString();
            await parent.WaitForRules();
            Assert.IsTrue(parent.IsModified);
            Assert.IsTrue(child.IsModified);
            Assert.IsTrue(grandChild.IsModified);

        }

        [TestMethod]
        public async Task EditParentChildFetchTest_ModifyGrandChild_IsSelfModified()
        {

            grandChild.FirstName = Guid.NewGuid().ToString();
            await parent.WaitForRules();

            Assert.IsFalse(parent.IsSelfModified);
            Assert.IsFalse(child.IsSelfModified);
            Assert.IsTrue(grandChild.IsSelfModified);

        }

        [TestMethod]
        public async Task EditParentChildFetchTest_ModifyGrandChild_IsSavable()
        {

            grandChild.FirstName = Guid.NewGuid().ToString();
            await parent.WaitForRules();

            Assert.IsTrue(parent.IsSavable);
            Assert.IsFalse(child.IsSavable);
            Assert.IsFalse(grandChild.IsSavable);

        }

    }
}

