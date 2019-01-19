using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOBehave.Core;
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
    [TestCategory("ValidateBase")]
    public class ValidateBaseTests
    {

        private ILifetimeScope scope;
        private IValidateObject validate;
        private IValidateObject child;
        private List<string> propertyChangeList = new List<string>();
        [TestInitialize]
        public void TestInitailize()
        {
            scope = AutofacContainer.GetLifetimeScope();
            validate = scope.Resolve<IValidateObject>();
            child = scope.Resolve<IValidateObject>();
            validate.Child = child;
            validate.PropertyChanged += (o, e) => propertyChangeList.Add(e.PropertyName);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Assert.IsFalse(validate.IsBusy);
            Assert.IsFalse(validate.IsSelfBusy);
        }

        [TestMethod]
        public void ValidateBase_InitialValues()
        {

        }


        [TestMethod]
        public void ValidateBase_Set()
        {
            validate.FirstName = "Keith";
            Assert.IsTrue(propertyChangeList.Contains(nameof(IValidateObject.FirstName)));
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
            Assert.IsFalse(validate[nameof(validate.FirstName)].IsValid);
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
            Assert.IsTrue(validate[nameof(validate.FirstName)].IsValid);

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
            Assert.AreEqual(1, validate.RuleResultList.Count());
            Assert.AreEqual(message, validate.RuleResultList.Single().PropertyErrorMessages.Single().Value);
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



        [TestMethod]
        public void ValidateBase_PropertyIsValid_True()
        {
            validate.FirstName = "Valid";
            Assert.IsTrue(validate.PropertyIsValid[nameof(IValidateObject.FirstName)]);
            // Property event doesn't get raised unless valid actually changes
            Assert.IsFalse(propertyChangeList.Contains(nameof(IValidateBase.PropertyIsValid)));
        }

        [TestMethod]
        public void ValidateBase_PropertyIsValid_False()
        {
            validate.FirstName = "Error";
            Assert.IsFalse(validate.PropertyIsValid[nameof(IValidateObject.FirstName)]);
            // Property event doesn't get raised unless valid actually changes
            Assert.IsTrue(propertyChangeList.Contains(nameof(IValidateBase.PropertyIsValid)));
        }

        [TestMethod]
        public void ValidateBase_PropertyErrorMessage_True()
        {
            validate.FirstName = "Valid";
            Assert.IsTrue(string.IsNullOrWhiteSpace(validate.PropertyErrorMessage[nameof(IValidateObject.FirstName)]));
            // Property event doesn't get raised unless valid actually changes
            Assert.IsFalse(propertyChangeList.Contains(nameof(IValidateBase.PropertyErrorMessage)));
        }

        [TestMethod]
        public void ValidateBase_PropertyErrorMessage_False()
        {
            validate.FirstName = "Error";
            Assert.IsFalse(string.IsNullOrWhiteSpace(validate.PropertyErrorMessage[nameof(IValidateObject.FirstName)]));
            // Property event doesn't get raised unless valid actually changes
            Assert.IsTrue(propertyChangeList.Contains(nameof(IValidateBase.PropertyErrorMessage)));

        }
    }
}
