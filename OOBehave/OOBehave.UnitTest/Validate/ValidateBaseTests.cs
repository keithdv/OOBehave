using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOBehave.Rules;
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
            validate.FirstName = "Keith";
        }

        [TestMethod]
        public void Validate_SetGet()
        {
            var name = Guid.NewGuid().ToString();
            validate.ShortName = name;
            Assert.AreEqual(name, validate.ShortName);
        }

        [TestMethod]
        public void Validate_RulesCreated()
        {
            Assert.IsTrue(Core.Factory.StaticFactory.RuleManager.RegisteredRules.ContainsKey(typeof(Validate)));
            Assert.AreEqual(3, Core.Factory.StaticFactory.RuleManager.RegisteredRules[typeof(Validate)].Count);
            Assert.IsInstanceOfType(((IRegisteredRuleList<Validate>) Core.Factory.StaticFactory.RuleManager.RegisteredRules[typeof(Validate)]).First(), typeof(ShortNameCascadeRule));
            Assert.IsInstanceOfType(((IRegisteredRuleList<Validate>)Core.Factory.StaticFactory.RuleManager.RegisteredRules[typeof(Validate)]).Take(2).Last(), typeof(FullNameCascadeRule));
            Assert.IsInstanceOfType(((IRegisteredRuleList<Validate>)Core.Factory.StaticFactory.RuleManager.RegisteredRules[typeof(Validate)]).Take(3).Last(), typeof(FirstNameTargetRule));
        }

        [TestMethod]
        public void Validate_NameCascadeRule()
        {

            validate.FirstName = "John";
            validate.LastName = "Smith";

            Assert.AreEqual("John Smith", validate.ShortName);

        }

        [TestMethod]
        public void Validate_CascadeRule_FullName()
        {

            validate.Title = "Mr.";
            validate.FirstName = "John";
            validate.LastName = "Smith";

            Assert.AreEqual("John Smith", validate.ShortName);
            Assert.AreEqual("Mr. John Smith", validate.FullName);

        }

        [TestMethod]
        public void Validate_CascadeRule_IsValid_True()
        {
            validate.Title = "Mr.";
            validate.FirstName = "John";
            validate.LastName = "Smith";

            Assert.IsTrue(validate.IsValid);
        }

        [TestMethod]
        public void Validate_CascadeRule_IsValid_False()
        {
            validate.LastName = "Smith";
            validate.FirstName = "Error";

            Assert.IsFalse(validate.IsValid);
        }

        [TestMethod]
        public void Validate_CascadeRule_IsValid_False_Fixed()
        {
            validate.LastName = "Smith";
            validate.FirstName = "Error";

            Assert.IsFalse(validate.IsValid);

            validate.FirstName = "John";

            Assert.IsTrue(validate.IsValid);
        }

        [TestMethod]
        public void Validate_TargetRule_IsValid_False()
        {

            validate.FirstName = "Error";
            validate.LastName = "Smith";
            validate.Title = "Mr.";

            Assert.IsFalse(validate.IsValid);
        }

        [TestMethod]
        public void Validate_TargetRule_IsValid_False_Fixed()
        {
            validate.LastName = "Smith";
            validate.FirstName = "Error";

            Assert.IsFalse(validate.IsValid);

            validate.FirstName = "John";

            Assert.IsTrue(validate.IsValid);
        }
    }
}
