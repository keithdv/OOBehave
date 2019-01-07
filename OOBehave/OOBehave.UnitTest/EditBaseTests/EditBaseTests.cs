using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
    public class EditBaseTests
    {

        private ILifetimeScope scope;
        private IEditPerson editPerson;

        [TestInitialize]
        public void TestInitialize()
        {
            scope = AutofacContainer.GetLifetimeScope();

            editPerson = scope.Resolve<IEditPerson>();
            var parentDto = scope.Resolve<IReadOnlyList<PersonDto>>().Where(p => !p.FatherId.HasValue && !p.MotherId.HasValue).First();

            editPerson.FillFromDto(parentDto);
            editPerson.MarkOld();
            editPerson.MarkUnmodified();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Assert.IsFalse(editPerson.IsBusy);
        }

        [TestMethod]
        public void EditBaseTest()
        {
            Assert.IsFalse(editPerson.IsModified);
            Assert.IsFalse(editPerson.IsSelfModified);
            Assert.IsFalse(editPerson.IsSavable);
        }

        [TestMethod]
        public void EditBaseTest_SetString_IsModified()
        {
            editPerson.FullName = Guid.NewGuid().ToString();
            Assert.IsTrue(editPerson.IsModified);
            Assert.IsTrue(editPerson.IsSelfModified);
            CollectionAssert.AreEquivalent(new List<string>() { nameof(IEditPerson.FullName), }, editPerson.ModifiedProperties.ToList());
        }

        [TestMethod]
        public void EditBaseTest_SetSameString_IsModified_False()
        {
            var firstName = editPerson.FirstName;
            editPerson.FirstName = firstName;
            Assert.IsFalse(editPerson.IsModified);
            Assert.IsFalse(editPerson.IsSelfModified);
        }

        [TestMethod]
        public void EditBaseTest_SetNonLoadedProperty_IsModified()
        {
            // Set a property that isn't loaded during the Fetch/Create
            editPerson.Age = 10;
            Assert.IsTrue(editPerson.IsModified);
            Assert.IsTrue(editPerson.IsSelfModified);
            CollectionAssert.AreEquivalent(new List<string>() { nameof(IEditPerson.Age), }, editPerson.ModifiedProperties.ToList());
        }


        [TestMethod]
        public void EditBaseTest_InitiallyDefined_SameInstance_IsModified_False()
        {
            var list = editPerson.InitiallyDefined;
            Assert.IsNotNull(list);
            editPerson.InitiallyDefined = list;
            Assert.IsFalse(editPerson.IsModified);
            Assert.IsFalse(editPerson.IsSelfModified);
            Assert.AreEqual(0, editPerson.ModifiedProperties.Count());
        }

        [TestMethod]
        public void EditBaseTest_InitiallyDefined_NewInstance_IsModified_True()
        {
            editPerson.InitiallyDefined = editPerson.InitiallyDefined.ToList();
            Assert.IsTrue(editPerson.IsModified);
            Assert.IsTrue(editPerson.IsSelfModified);
            CollectionAssert.AreEquivalent(new List<string>() { nameof(IEditPerson.InitiallyDefined), }, editPerson.ModifiedProperties.ToList());
        }

        [TestMethod]
        public void EditBaseTest_InitiallyNull_IsModified()
        {
            editPerson.InitiallyNull = new List<int>() { 3, 4, 5 };
            Assert.IsTrue(editPerson.IsModified);
            Assert.IsTrue(editPerson.IsSelfModified);

        }

        [TestMethod]
        public void EditBaseTest_IsDeleted()
        {
            editPerson.Delete();
            Assert.IsTrue(editPerson.IsDeleted);
            Assert.IsTrue(editPerson.IsModified);
            Assert.IsTrue(editPerson.IsSelfModified);
        }

        [TestMethod]
        public void EditBaseTest_IsSavable()
        {
            editPerson.FirstName = Guid.NewGuid().ToString();
            Assert.IsTrue(editPerson.IsSavable);
        }

        [TestMethod]
        public async Task EditBaseTest_Save()
        {
            var mock = scope.Resolve<MockSendReceivePortal<EditPerson>>();
            mock.MockPortal.Setup(x => x.Update((EditPerson) editPerson)).Returns(Task.CompletedTask);

            editPerson.FirstName = Guid.NewGuid().ToString();
            await editPerson.Save();

            mock.MockPortal.Verify(x => x.Update((EditPerson)editPerson), Times.Once);
        }


    }
}

