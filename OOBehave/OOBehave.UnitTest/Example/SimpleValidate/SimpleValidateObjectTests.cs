using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOBehave.UnitTest.Example.SimpleValidate
{
    [TestClass]
    public class SimpleValidateObjectTests
    {
        private ILifetimeScope scope;

        [TestInitialize]
        public void TestInitialize()
        {
            scope = AutofacContainer.GetLifetimeScope();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            scope.Dispose();
        }


        [TestMethod]
        public void SimpleValidateObject()
        {
            var validateObject = scope.Resolve<SimpleValidateObject>();

            validateObject.FirstName = "John";
            validateObject.LastName = "Smith";
            Assert.AreEqual("John Smith", validateObject.ShortName);
            Assert.IsTrue(validateObject.IsValid);
        }

        [TestMethod]
        public void SimpleValidateObject_InValid()
        {
            var validateObject = scope.Resolve<SimpleValidateObject>();

            validateObject.FirstName = string.Empty;
            validateObject.LastName = "Smith";

            Assert.IsFalse(validateObject.IsValid);
            Assert.AreEqual(string.Empty, validateObject.ShortName);
            Assert.AreEqual(1, validateObject.RuleResultList.Where(r => r.IsError).Count());
        }

        [TestMethod]
        public void SimpleValidateObject_InValid_Fixed()
        {
            var validateObject = scope.Resolve<SimpleValidateObject>();

            validateObject.FirstName = string.Empty;
            Assert.IsFalse(validateObject.IsValid);

            validateObject.FirstName = "John";
            validateObject.LastName = "Smith";
            Assert.IsTrue(validateObject.IsValid);

        }

    }
}
