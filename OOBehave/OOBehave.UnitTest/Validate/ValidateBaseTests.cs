using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOBehave.UnitTest.Validate
{

    [TestClass]
    public class ValidateBaseTests
    {


        Validate validate;

        [TestInitialize]
        public void TestInitailize()
        {
            validate = new Validate(Core.Factory.StaticFactory.CreateValidateBaseServices<Validate>());
        }

        [TestMethod]
        public void Validate_Const()
        {

        }


        [TestMethod]
        public void Validate_Set()
        {
            validate.Name = Guid.NewGuid().ToString();
        }

        [TestMethod]
        public void Validate_SetGet()
        {
            var name = Guid.NewGuid().ToString();
            validate.Name = name;
            Assert.AreEqual(name, validate.Name);
        }

        [TestMethod]
        public void Validate_RulesCreated()
        {
            Assert.IsTrue(Core.Factory.StaticFactory.RuleManager.RegisteredRules.ContainsKey(typeof(Validate)));
            Assert.AreEqual(1, Core.Factory.StaticFactory.RuleManager.RegisteredRules[typeof(Validate)].Count);
            Assert.IsInstanceOfType(Core.Factory.StaticFactory.RuleManager.RegisteredRules[typeof(Validate)].Single(), typeof(TestRule));
        }
    }
}
