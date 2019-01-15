using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
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
        }

    }
}
