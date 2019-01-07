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
    public class EditListBaseTests
    {

        private ILifetimeScope scope;
        private IEditPersonList list;
        private IEditPerson child;

        [TestInitialize]
        public void TestInitialize()
        {
            scope = AutofacContainer.GetLifetimeScope();
            var parentDto = scope.Resolve<IReadOnlyList<PersonDto>>().Where(p => !p.FatherId.HasValue && !p.MotherId.HasValue).First();

            list = scope.Resolve<IEditPersonList>();
            list.FillFromDto(parentDto);
            list.MarkUnmodified();
            list.MarkOld();

            child = scope.Resolve<IEditPerson>();
            child.MarkUnmodified();
            child.MarkOld();
            child.MarkAsChild();
            list.Add(child);

            Assert.IsFalse(list.IsBusy);

        }

        [TestCleanup]
        public void TestCleanup()
        {
            Assert.IsFalse(list.IsBusy);
        }

        [TestMethod]
        public void EditListBaseTest()
        {
            Assert.IsFalse(list.IsModified);
            Assert.IsFalse(list.IsSelfModified);
        }

        [TestMethod]
        public void EditListBaseTest_SetString_IsModified()
        {
            list.FullName = Guid.NewGuid().ToString();
            Assert.IsTrue(list.IsModified);
            Assert.IsTrue(list.IsSelfModified);
            CollectionAssert.AreEquivalent(new List<string>() { nameof(IEditPerson.FullName), }, list.ModifiedProperties.ToList());
        }

        [TestMethod]
        public void EditListBaseTest_SetSameString_IsModified_False()
        {
            var firstName = list.FirstName;
            list.FirstName = firstName;
            Assert.IsFalse(list.IsModified);
            Assert.IsFalse(list.IsSelfModified);
        }

        [TestMethod]
        public void EditListBaseTest_SetNonLoadedProperty_IsModified()
        {
            // Set a property that isn't loaded during the Fetch/Create
            list.Age = 10;
            Assert.IsTrue(list.IsModified);
            Assert.IsTrue(list.IsSelfModified);
            CollectionAssert.AreEquivalent(new List<string>() { nameof(IEditPerson.Age), }, list.ModifiedProperties.ToList());
        }


        [TestMethod]
        public async Task EditListBaseTest_ModifyChild_IsModified()
        {

            child.FirstName = Guid.NewGuid().ToString();
            await list.WaitForRules();
            Assert.IsTrue(list.IsModified);
            Assert.IsTrue(child.IsModified);

        }

        [TestMethod]
        public async Task EditListBaseTest_ModifyChild_IsSelfModified()
        {

            child.FirstName = Guid.NewGuid().ToString();
            await list.WaitForRules();

            Assert.IsFalse(list.IsSelfModified);
            Assert.IsTrue(child.IsSelfModified);

        }

        [TestMethod]
        public async Task EditListBaseTest_ModifyChild_IsSavable()
        {

            child.FirstName = Guid.NewGuid().ToString();
            await list.WaitForRules();

            Assert.IsTrue(list.IsSavable);
            Assert.IsFalse(child.IsSavable);

        }

        [TestMethod]
        public void EditListBaseTest_IsDeleted()
        {
            list.Delete();
            Assert.IsTrue(list.IsDeleted);
            Assert.IsTrue(list.IsModified);
            Assert.IsTrue(list.IsSelfModified);
        }
    }
}

