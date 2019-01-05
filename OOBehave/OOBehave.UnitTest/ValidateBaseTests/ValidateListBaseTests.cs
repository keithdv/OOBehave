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
    public class ValidateListBaseTests
    {

        ILifetimeScope scope;
        IValidateObjectList List;
        IValidateObject Child;

        [TestInitialize]
        public void TestInitailize()
        {
            scope = AutofacContainer.GetLifetimeScope();
            List = scope.Resolve<IValidateObjectList>();
            Child = scope.Resolve<IValidateObject>();
            List.Add(Child);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Assert.IsFalse(List.IsBusy);
            Assert.IsFalse(List.IsSelfBusy);
        }

        [TestMethod]
        public void ValidateList_Constructor()
        {

        }


        [TestMethod]
        public void ValidateList_Set()
        {
            List.FirstName = "Keith";
        }

        [TestMethod]
        public void ValidateList_SetGet()
        {
            var name = Guid.NewGuid().ToString();
            List.ShortName = name;
            Assert.AreEqual(name, List.ShortName);
        }

        //[TestMethod]
        //public void ValidateList_RulesCreated()
        //{
        //    Assert.IsTrue(Core.Factory.StaticFactory.RuleManager.RegisteredRules.ContainsKey(typeof(ValidateList)));
        //    Assert.AreEqual(3, Core.Factory.StaticFactory.RuleManager.RegisteredRules[typeof(ValidateList)].Count);
        //    Assert.IsInstanceOfType(((IRegisteredRuleList<ValidateList>) Core.Factory.StaticFactory.RuleManager.RegisteredRules[typeof(ValidateList)]).First(), typeof(ShortNameRule));
        //    Assert.IsInstanceOfType(((IRegisteredRuleList<ValidateList>)Core.Factory.StaticFactory.RuleManager.RegisteredRules[typeof(ValidateList)]).Take(2).Last(), typeof(FullNameRule));
        //}

        [TestMethod]
        public void ValidateList_Rule()
        {

            List.FirstName = "John";
            List.LastName = "Smith";

            Assert.AreEqual("John Smith", List.ShortName);

        }

        [TestMethod]
        public void ValidateList_Rule_Recursive()
        {

            List.Title = "Mr.";
            List.FirstName = "John";
            List.LastName = "Smith";

            Assert.AreEqual("John Smith", List.ShortName);
            Assert.AreEqual("Mr. John Smith", List.FullName);

        }

        [TestMethod]
        public void ValidateList_Rule_IsValid_True()
        {
            List.Title = "Mr.";
            List.FirstName = "John";
            List.LastName = "Smith";

            Assert.IsTrue(List.IsValid);
        }

        [TestMethod]
        public void ValidateList_Rule_IsValid_False()
        {
            List.Title = "Mr.";
            List.FirstName = "Error";
            List.LastName = "Smith";

            Assert.IsFalse(List.IsValid);
            Assert.AreEqual("Error", List.BrokenRulePropertyMessages(nameof(List.FirstName)).Single());
        }

        [TestMethod]
        public void ValidateList_Rule_IsValid_False_Fixed()
        {
            List.Title = "Mr.";
            List.FirstName = "Error";
            List.LastName = "Smith";

            Assert.IsFalse(List.IsValid);

            List.FirstName = "John";

            Assert.IsTrue(List.IsValid);
            Assert.AreEqual(0, List.BrokenRulePropertyMessages(nameof(List.FirstName)).Count());

        }


        [TestMethod]
        public void ValidateList_ParentInvalid()
        {
            List.FirstName = "Error";
            Assert.IsFalse(List.IsBusy);
            Assert.IsFalse(List.IsValid);
            Assert.IsFalse(List.IsSelfValid);
            Assert.IsTrue(Child.IsValid);
            Assert.IsTrue(Child.IsSelfValid);
        }

        [TestMethod]
        public void ValidateList_ChildInvalid()
        {
            Child.FirstName = "Error";
            Assert.IsFalse(Child.IsValid);
            Assert.IsFalse(Child.IsSelfValid);
            Assert.IsFalse(List.IsBusy);
            Assert.IsFalse(List.IsValid);
            Assert.IsTrue(List.IsSelfValid);
        }

        [TestMethod]
        public void ValidateListBase_MarkInvalid()
        {
            string message;
            List.TestMarkInvalid(message = Guid.NewGuid().ToString());
            Assert.IsFalse(List.IsValid);
            Assert.IsFalse(List.IsSelfValid);
            Assert.AreEqual(1, List.BrokenRuleMessages.Count());
            Assert.AreEqual(message, List.BrokenRuleMessages.Single());
        }

        [TestMethod]
        public void ValidateListBase_MarkInvalid_Dont_Run_Rules()
        {
            var rrc = List.RuleRunCount;
            string message;
            List.TestMarkInvalid(message = Guid.NewGuid().ToString());
            List.FirstName = Guid.NewGuid().ToString();
            Assert.AreEqual(rrc, List.RuleRunCount);
        }
    }
}
