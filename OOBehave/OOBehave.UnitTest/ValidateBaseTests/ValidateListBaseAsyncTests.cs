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
    public class ValidateListBaseAsyncBaseAsyncTests
    {

        ILifetimeScope scope;
        IValidateAsyncObjectList List;
        IValidateAsyncObject Child;

        [TestInitialize]
        public void TestInitailize()
        {
            scope = AutofacContainer.GetLifetimeScope();
            List = scope.Resolve<IValidateAsyncObjectList>();
            Child = scope.Resolve<IValidateAsyncObject>();
            List.Add(Child);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Assert.IsFalse(List.IsBusy);
            Assert.IsFalse(List.IsSelfBusy);
        }

        [TestMethod]
        public void ValidateListBaseAsync_Constructor()
        {

        }


        [TestMethod]
        public void ValidateListBaseAsync_SetProperty()
        {
            List.FirstName = "Keith";
        }

        [TestMethod]
        public void ValidateListBaseAsync_SetGet()
        {
            var name = Guid.NewGuid().ToString();
            List.ShortName = name;
            Assert.AreEqual(name, List.ShortName);
        }

        //[TestMethod]
        //public void ValidateListBaseAsync_RulesCreated()
        //{
        //    Assert.IsTrue(Core.Factory.StaticFactory.RuleManager.RegisteredRules.ContainsKey(typeof(ValidateListBaseAsync)));
        //    Assert.AreEqual(3, Core.Factory.StaticFactory.RuleManager.RegisteredRules[typeof(ValidateListBaseAsync)].Count);
        //    Assert.IsInstanceOfType(((IRegisteredRuleList<ValidateListBaseAsync>) Core.Factory.StaticFactory.RuleManager.RegisteredRules[typeof(ValidateListBaseAsync)]).First(), typeof(ShortNameRule));
        //    Assert.IsInstanceOfType(((IRegisteredRuleList<ValidateListBaseAsync>)Core.Factory.StaticFactory.RuleManager.RegisteredRules[typeof(ValidateListBaseAsync)]).Take(2).Last(), typeof(FullNameRule));
        //}

        [TestMethod]
        public async Task ValidateListBaseAsync_Rule()
        {

            List.FirstName = "John";
            List.LastName = "Smith";

            await List.WaitForRules();

            Assert.AreEqual("John Smith", List.ShortName);

        }

        [TestMethod]
        public async Task ValidateListBaseAsync_Rule_Recursive()
        {

            List.Title = "Mr.";
            List.FirstName = "John";
            List.LastName = "Smith";

            await List.WaitForRules();

            Assert.AreEqual("John Smith", List.ShortName);
            Assert.AreEqual("Mr. John Smith", List.FullName);

        }

        [TestMethod]
        public async Task ValidateListBaseAsync_Rule_IsValid_True()
        {
            List.Title = "Mr.";
            List.FirstName = "John";
            List.LastName = "Smith";

            await List.WaitForRules();

            Assert.IsTrue(List.IsValid);
        }

        [TestMethod]
        public async Task ValidateListBaseAsync_Rule_IsValid_False()
        {
            List.Title = "Mr.";
            List.FirstName = "Error";
            List.LastName = "Smith";
            await List.WaitForRules();

            Assert.IsFalse(List.IsValid);
            Assert.AreEqual(1, List.RuleResultList.Where(r => r.IsError && r.PropertyErrorMessages.Any(p => p.Key == nameof(IValidateObject.FirstName))).Count());
        }

        [TestMethod]
        public async Task ValidateListBaseAsync_Rule_IsValid_False_Fixed()
        {
            List.Title = "Mr.";
            List.FirstName = "Error";
            List.LastName = "Smith";
            await List.WaitForRules();

            Assert.IsFalse(List.IsValid);

            List.FirstName = "John";

            Assert.IsTrue(List.IsValid);
            Assert.AreEqual(0, List.RuleResultList.Where(r => r.IsError && r.PropertyErrorMessages.Any(p => p.Key == nameof(IValidateObject.FirstName))).Count());

        }


        [TestMethod]
        public async Task ValidateListBaseAsync_ParentInvalid()
        {
            List.FirstName = "Error";
            await List.WaitForRules();
            Assert.IsFalse(List.IsBusy);
            Assert.IsFalse(List.IsValid);
            Assert.IsFalse(List.IsSelfValid);
            Assert.IsTrue(Child.IsValid);
            Assert.IsTrue(Child.IsSelfValid);
        }

        [TestMethod]
        public async Task ValidateListBaseAsync_ChildInvalid()
        {
            Child.FirstName = "Error";
            await List.WaitForRules();
            Assert.IsFalse(Child.IsValid);
            Assert.IsFalse(Child.IsSelfValid);
            Assert.IsFalse(List.IsBusy);
            Assert.IsFalse(List.IsValid);
            Assert.IsTrue(List.IsSelfValid);
        }

        [TestMethod]
        public async Task ValidateListBaseAsync_Child_IsBusy()
        {
            Child.FirstName = "Error";

            Assert.IsTrue(List.IsBusy);
            Assert.IsFalse(List.IsSelfBusy);
            Assert.IsTrue(Child.IsBusy);
            Assert.IsTrue(Child.IsSelfBusy);

            await List.WaitForRules();

            Assert.IsFalse(List.IsBusy);
            Assert.IsFalse(List.IsValid);
            Assert.IsTrue(List.IsSelfValid);
            Assert.IsFalse(Child.IsValid);
            Assert.IsFalse(Child.IsSelfValid);
        }

        [TestMethod]
        public async Task Validate_RunSelfRules()
        {
            var ruleCount = List.RuleRunCount;
            await List.CheckAllSelfRules();
            Assert.AreEqual(ruleCount + 2, List.RuleRunCount);
        }

        [TestMethod]
        public async Task Validate_RunAllRules()
        {
            var ruleCount = List.RuleRunCount;
            await List.CheckAllRules();
            Assert.AreEqual(ruleCount + 4, List.RuleRunCount); // +2 for child
        }
    }
}
