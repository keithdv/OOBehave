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
        private IEditPerson parent;
        private IEditPerson child;

        [TestInitialize]
        public void TestInitialize()
        {
            scope = AutofacContainer.GetLifetimeScope();
            var persons = scope.Resolve<IReadOnlyList<PersonDto>>();
            


            parent = scope.Resolve<IEditPerson>();
            parent.FillFromDto(persons.Where(p => !p.FatherId.HasValue && !p.MotherId.HasValue).First());

            child = scope.Resolve<IEditPerson>();
            child.FillFromDto(persons.Where(p => p.FatherId == parent.Id).First());
            parent.Child = child;

            child.MarkOld();
            child.MarkUnmodified();
            child.MarkAsChild();
            parent.MarkOld();
            parent.MarkUnmodified();


        }

        [TestMethod]
        public void EditParentChildFetchTest_Fetch_InitialMeta()
        {
            void AssertMeta(IEditPerson t)
            {
                Assert.IsNotNull(t);
                Assert.IsFalse(t.IsModified);
                Assert.IsFalse(t.IsSelfModified);
                Assert.IsFalse(t.IsNew);
                Assert.IsFalse(t.IsSavable);
            }

            AssertMeta(parent);
            AssertMeta(child);

            Assert.IsFalse(parent.IsChild);
            Assert.IsTrue(child.IsChild);

        }

        [TestMethod]
        public async Task EditParentChildFetchTest_ModifyChild_IsModified()
        {

            child.FirstName = Guid.NewGuid().ToString();
            await parent.WaitForRules();
            Assert.IsTrue(parent.IsModified);
            Assert.IsTrue(child.IsModified);

        }

        [TestMethod]
        public async Task EditParentChildFetchTest_ModifyChild_IsSelfModified()
        {

            child.FirstName = Guid.NewGuid().ToString();
            await parent.WaitForRules();

            Assert.IsFalse(parent.IsSelfModified);
            Assert.IsTrue(child.IsSelfModified);

        }

        [TestMethod]
        public async Task EditParentChildFetchTest_ModifyChild_IsSavable()
        {

            child.FirstName = Guid.NewGuid().ToString();
            await parent.WaitForRules();

            Assert.IsTrue(parent.IsSavable);
            Assert.IsFalse(child.IsSavable);

        }


        [TestMethod]
        public void EditParentChildFetchTest_Parent()
        {
            Assert.AreSame(parent, child.Parent);
        }
    }
}

