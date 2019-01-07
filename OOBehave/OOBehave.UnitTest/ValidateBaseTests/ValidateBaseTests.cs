using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOBehave.Rules;
using OOBehave.UnitTest.PersonObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.ValidateBaseTests
{


    [TestClass]
    public class ValidateBaseTests
    {

        private ILifetimeScope scope;
        private IValidateObject validate;
        private IValidateObject child;

        [TestInitialize]
        public void TestInitailize()
        {
            scope = AutofacContainer.GetLifetimeScope();
            validate = scope.Resolve<IValidateObject>();
            child = scope.Resolve<IValidateObject>();
            validate.Child = child;
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Assert.IsFalse(validate.IsBusy);
            Assert.IsFalse(validate.IsSelfBusy);
        }

        [TestMethod]
        public void ValidateBase_Constructor()
        {

        }


        [TestMethod]
        public void ValidateBase_Set()
        {
            validate.FirstName = "Keith";
        }

        [TestMethod]
        public void ValidateBase_SetGet()
        {
            var name = Guid.NewGuid().ToString();
            validate.ShortName = name;
            Assert.AreEqual(name, validate.ShortName);
        }

        //[TestMethod]
        //public void ValidateBase_RulesCreated()
        //{
        //    Assert.IsTrue(Core.Factory.StaticFactory.RuleManager.RegisteredRules.ContainsKey(typeof(Validate)));
        //    Assert.AreEqual(3, Core.Factory.StaticFactory.RuleManager.RegisteredRules[typeof(Validate)].Count);
        //    Assert.IsInstanceOfType(((IRegisteredRuleList<Validate>) Core.Factory.StaticFactory.RuleManager.RegisteredRules[typeof(Validate)]).First(), typeof(ShortNameRule));
        //    Assert.IsInstanceOfType(((IRegisteredRuleList<Validate>)Core.Factory.StaticFactory.RuleManager.RegisteredRules[typeof(Validate)]).Take(2).Last(), typeof(FullNameRule));
        //}

        [TestMethod]
        public void ValidateBase_Rule()
        {

            validate.FirstName = "John";
            validate.LastName = "Smith";

            Assert.AreEqual("John Smith", validate.ShortName);

        }

        [TestMethod]
        public void ValidateBase_Rule_Recursive()
        {

            validate.Title = "Mr.";
            validate.FirstName = "John";
            validate.LastName = "Smith";

            Assert.AreEqual("John Smith", validate.ShortName);
            Assert.AreEqual("Mr. John Smith", validate.FullName);

        }

        [TestMethod]
        public void ValidateBase_Rule_SameValue()
        {

            validate.Title = "Mr.";
            validate.FirstName = "John";
            validate.LastName = "Smith";
            var ruleCount = validate.RuleRunCount;
            validate.Title = "Mr.";
            validate.FirstName = "John";
            validate.LastName = "Smith";

            Assert.AreEqual(ruleCount, validate.RuleRunCount);
        }

        [TestMethod]
        public void ValidateBase_Rule_IsValid_True()
        {
            validate.Title = "Mr.";
            validate.FirstName = "John";
            validate.LastName = "Smith";

            Assert.IsTrue(validate.IsValid);
        }

        [TestMethod]
        public void ValidateBase_Rule_IsValid_False()
        {
            validate.Title = "Mr.";
            validate.FirstName = "Error";
            validate.LastName = "Smith";

            Assert.IsFalse(validate.IsValid);
            Assert.AreEqual("Error", validate.BrokenRulePropertyMessages(nameof(validate.FirstName)).Single());
        }

        [TestMethod]
        public void ValidateBase_Rule_IsValid_False_Fixed()
        {
            validate.Title = "Mr.";
            validate.FirstName = "Error";
            validate.LastName = "Smith";

            Assert.IsFalse(validate.IsValid);

            validate.FirstName = "John";

            Assert.IsTrue(validate.IsValid);
            Assert.AreEqual(0, validate.BrokenRulePropertyMessages(nameof(validate.FirstName)).Count());

        }

        [TestMethod]
        public async Task ValidateBase_RunSelfRules()
        {
            var ruleCount = validate.RuleRunCount;
            await validate.CheckAllSelfRules();
            Assert.AreEqual(ruleCount + 2, validate.RuleRunCount);
        }

        [TestMethod]
        public async Task ValidateBase_RunAllRules()
        {
            var ruleCount = validate.RuleRunCount;
            await validate.CheckAllRules();
            Assert.AreEqual(ruleCount + 4, validate.RuleRunCount);
        }


        [TestMethod]
        public void ValidateBase_validateInvalid()
        {
            validate.FirstName = "Error";
            Assert.IsFalse(validate.IsBusy);
            Assert.IsFalse(validate.IsValid);
            Assert.IsFalse(validate.IsSelfValid);
            Assert.IsTrue(child.IsValid);
            Assert.IsTrue(child.IsSelfValid);
        }

        [TestMethod]
        public void ValidateBase_ChildInvalid()
        {
            child.FirstName = "Error";
            Assert.IsFalse(validate.IsBusy);
            Assert.IsFalse(validate.IsValid);
            Assert.IsTrue(validate.IsSelfValid);
            Assert.IsFalse(child.IsValid);
            Assert.IsFalse(child.IsSelfValid);
        }

        [TestMethod]
        public void ValidateBase_Parent()
        {
            Assert.AreSame(validate, child.Parent);
        }

        [TestMethod]
        public void ValidateBase_MarkInvalid()
        {
            string message;
            validate.TestMarkInvalid(message = Guid.NewGuid().ToString());
            Assert.IsFalse(validate.IsValid);
            Assert.IsFalse(validate.IsSelfValid);
            Assert.AreEqual(1, validate.BrokenRuleMessages.Count());
            Assert.AreEqual(message, validate.BrokenRuleMessages.Single());
        }

        [TestMethod]
        public void ValidateBase_MarkInvalid_Dont_Run_Rules()
        {
            var rrc = validate.RuleRunCount;
            string message;
            validate.TestMarkInvalid(message = Guid.NewGuid().ToString());
            validate.FirstName = Guid.NewGuid().ToString();
            Assert.AreEqual(rrc, validate.RuleRunCount);
        }
    }
}
