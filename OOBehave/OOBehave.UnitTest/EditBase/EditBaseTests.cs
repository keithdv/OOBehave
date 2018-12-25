using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOBehave.Portal;
using OOBehave.UnitTest.PersonObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOBehave.UnitTest.EditBase
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
            var parentDto = scope.Resolve<IReadOnlyList<PersonDto>>().Where(p => !p.FatherId.HasValue && !p.MotherId.HasValue).First();

            var portal = scope.Resolve<ISendReceivePortal<IEditPerson>>();
            editPerson = portal.Fetch(parentDto).Result;
        }

        [TestMethod]
        public void EditBaseTest()
        {
            Assert.IsFalse(editPerson.IsModified);
            Assert.IsFalse(editPerson.IsSelfModified);
        }

        [TestMethod]
        public void EditBaseTest_SetString_IsModified()
        {
            editPerson.FirstName = Guid.NewGuid().ToString();
            Assert.IsTrue(editPerson.IsModified);
            Assert.IsTrue(editPerson.IsSelfModified);
        }

        [TestMethod]
        public void EditBaseTest_SameValue()
        {
            Assert.Fail("Do test");
        }

        [TestMethod]
        public void EditBaseTest_SameClass()
        {
            Assert.Fail("Do Test");
        }

    }
}

