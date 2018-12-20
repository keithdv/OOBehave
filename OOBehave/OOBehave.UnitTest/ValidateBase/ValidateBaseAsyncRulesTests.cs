using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOBehave.Rules;
using OOBehave.UnitTest.PersonObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.ValidateBase
{

    public class ValidateAsyncRules : PersonBase<ValidateAsyncRules>, IValidate
    {

        public ValidateAsyncRules(IValidateBaseServices<ValidateAsyncRules> services) : base(services)
        {
            RuleExecute.AddRule(new ShortNameAsyncRule<ValidateAsyncRules>());
            RuleExecute.AddRule(new FullNameAsyncRule<ValidateAsyncRules>());
            RuleExecute.AddRule(new PersonAsyncRule<ValidateAsyncRules>());
        }

    }

    [TestClass]
    public class ValidateBaseAsyncRulesTests
    {


        ValidateAsyncRules validate;

        [TestInitialize]
        public void TestInitailize()
        {
            validate = AutofacContainer.GetLifetimeScope().Resolve<ValidateAsyncRules>();
        }

        [TestMethod]
        public void ValidateAsyncRules_Const()
        {

        }


        [TestMethod]
        public void ValidateAsyncRules_Set()
        {
            validate.FirstName = "Keith";
        }

        [TestMethod]
        public void ValidateAsyncRules_SetGet()
        {
            var name = Guid.NewGuid().ToString();
            validate.ShortName = name;
            Assert.AreEqual(name, validate.ShortName);
        }

        //[TestMethod]
        //public void ValidateAsyncRules_RulesCreated()
        //{
        //    Assert.IsTrue(Core.Factory.StaticFactory.RuleManager.RegisteredRules.ContainsKey(typeof(ValidateAsyncRules)));
        //    Assert.AreEqual(3, Core.Factory.StaticFactory.RuleManager.RegisteredRules[typeof(ValidateAsyncRules)].Count);
        //    Assert.IsInstanceOfType(((IRegisteredRuleList<ValidateAsyncRules>) Core.Factory.StaticFactory.RuleManager.RegisteredRules[typeof(ValidateAsyncRules)]).First(), typeof(ShortNameCascadeAsyncRule));
        //    Assert.IsInstanceOfType(((IRegisteredRuleList<ValidateAsyncRules>)Core.Factory.StaticFactory.RuleManager.RegisteredRules[typeof(ValidateAsyncRules)]).Take(2).Last(), typeof(FullNameCascadeAsyncRule));
        //    Assert.IsInstanceOfType(((IRegisteredRuleList<ValidateAsyncRules>)Core.Factory.StaticFactory.RuleManager.RegisteredRules[typeof(ValidateAsyncRules)]).Take(3).Last(), typeof(FirstNameTargetAsyncRule));
        //}


        [TestMethod]
        public async Task ValidateAsyncRules_CascadeRule()
        {

            validate.FirstName = "John";
            validate.LastName = "Smith";

            await validate.WaitForRules();

            Assert.AreEqual("John Smith", validate.ShortName);

        }

        [TestMethod]
        public async Task ValidateAsyncRules_CascadeRule_Recursive()
        {

            validate.Title = "Mr.";
            validate.FirstName = "John";
            validate.LastName = "Smith";

            await validate.WaitForRules();

            Assert.AreEqual("John Smith", validate.ShortName);
            Assert.AreEqual("Mr. John Smith", validate.FullName);

        }

        [TestMethod]
        public async Task ValidateAsyncRules_CascadeRule_IsValid_True()
        {
            validate.Title = "Mr.";
            validate.FirstName = "John";
            validate.LastName = "Smith";

            await validate.WaitForRules();

            Assert.IsTrue(validate.IsValid);
        }

        [TestMethod]
        public async Task ValidateAsyncRules_CascadeRule_IsValid_False()
        {
            validate.Title = "Mr.";
            validate.FirstName = "Error";
            validate.LastName = "Smith";

            await validate.WaitForRules();

            Assert.IsFalse(validate.IsValid);
            Assert.AreEqual("Error", validate.BrokenRulePropertyMessages(nameof(validate.FirstName)).Single());
        }

        [TestMethod]
        public async Task ValidateAsyncRules_CascadeRule_IsValid_False_Fixed()
        {
            validate.Title = "Mr.";
            validate.FirstName = "Error";
            validate.LastName = "Smith";

            await validate.WaitForRules();

            Assert.IsFalse(validate.IsValid);

            validate.FirstName = "John";

            await validate.WaitForRules();

            Assert.IsTrue(validate.IsValid);
            Assert.AreEqual(0, validate.BrokenRulePropertyMessages(nameof(validate.FirstName)).Count());

        }

        [TestMethod]
        public async Task ValidateAsyncRules_TargetRule_IsValid_False()
        {

            validate.Title = "Mr.";
            validate.FirstName = "John";
            validate.LastName = "Smith";

            await validate.WaitForRules();

            Assert.IsTrue(validate.IsValid);

            validate.ShortName = "";

            await validate.WaitForRules();

            Assert.IsFalse(validate.IsValid);
            Assert.AreEqual(1, validate.BrokenRulePropertyMessages(nameof(validate.ShortName)).Count());

        }
    }
}
