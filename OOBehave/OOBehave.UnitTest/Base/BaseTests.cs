using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave.UnitTest.Base
{
    [TestClass]
    public class BaseTests
    {
        private Base single;

        [TestInitialize]
        public void TestInitialize()
        {
            single = AutofacContainer.Resolve<Base>();
        }
        [TestMethod]
        public void Base_Construct()
        {
            var name = single.Name;
        }

        [TestMethod]
        public void Base_Set()
        {
            single.Name = Guid.NewGuid().ToString();
        }

        [TestMethod]
        public void Base_SetGet()
        {
            var name = Guid.NewGuid().ToString();
            single.Name = name;
            Assert.AreEqual(name, single.Name);
        }
    }
}
