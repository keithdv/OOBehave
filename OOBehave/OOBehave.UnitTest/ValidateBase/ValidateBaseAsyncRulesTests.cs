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

    public interface IValidateAsyncRules : IPersonBase { }

    public class ValidateAsyncRules : PersonValidateBase<ValidateAsyncRules>, IValidateAsyncRules
    {

        public ValidateAsyncRules(IValidateBaseServices<ValidateAsyncRules> services,
            IShortNameAsyncRule<ValidateAsyncRules> shortNameRule,
            IFullNameAsyncRule<ValidateAsyncRules> fullNameRule) : base(services)
        {
            RuleExecute.AddRules(shortNameRule, fullNameRule);
        }

    }

    [TestClass]
    public class ValidateBaseAsyncRulesTests
    {


        IValidateAsyncRules validate;

        [TestInitialize]
        public void TestInitailize()
        {
            validate = AutofacContainer.GetLifetimeScope().Resolve<IValidateAsyncRules>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Assert.IsFalse(validate.IsBusy);
            Assert.IsFalse(validate.IsSelfBusy);
        }

        [TestMethod]
        public void ValidateAsyncRules_Const()
        {

        }


        [TestMethod]
        public async Task ValidateAsyncRules_Set()
        {
            validate.FirstName = "Keith";
            await validate.WaitForRules();
        }

        [TestMethod]
        public async Task ValidateAsyncRules_Set_IsBusy_True()
        {
            validate.FirstName = "Keith";
            Assert.IsTrue(validate.IsBusy);
            Assert.IsTrue(validate.IsSelfBusy);
            await validate.WaitForRules();
            Assert.IsFalse(validate.IsBusy);
            Assert.IsFalse(validate.IsSelfBusy);
        }

        [TestMethod]
        public async Task ValidateAsyncRules_SetGet()
        {
            var name = Guid.NewGuid().ToString();
            validate.ShortName = name;
            Assert.AreEqual(name, validate.ShortName);
            await validate.WaitForRules();
        }

        //[TestMethod]
        //public void ValidateAsyncRules_RulesCreated()
        //{
        //    Assert.IsTrue(Core.Factory.StaticFactory.RuleManager.RegisteredRules.ContainsKey(typeof(ValidateAsyncRules)));
        //    Assert.AreEqual(3, Core.Factory.StaticFactory.RuleManager.RegisteredRules[typeof(ValidateAsyncRules)].Count);
        //    Assert.IsInstanceOfType(((IRegisteredRuleList<ValidateAsyncRules>) Core.Factory.StaticFactory.RuleManager.RegisteredRules[typeof(ValidateAsyncRules)]).First(), typeof(ShortNameAsyncRule));
        //    Assert.IsInstanceOfType(((IRegisteredRuleList<ValidateAsyncRules>)Core.Factory.StaticFactory.RuleManager.RegisteredRules[typeof(ValidateAsyncRules)]).Take(2).Last(), typeof(FullNameAsyncRule));
        //    Assert.IsInstanceOfType(((IRegisteredRuleList<ValidateAsyncRules>)Core.Factory.StaticFactory.RuleManager.RegisteredRules[typeof(ValidateAsyncRules)]).Take(3).Last(), typeof(FirstNameTargetAsyncRule));
        //}


        [TestMethod]
        public async Task ValidateAsyncRules_Rule()
        {

            validate.FirstName = "John";
            validate.LastName = "Smith";

            await validate.WaitForRules();

            Assert.AreEqual("John Smith", validate.ShortName);

        }

        [TestMethod]
        public async Task ValidateAsyncRules_Rule_Recursive()
        {

            validate.Title = "Mr.";
            validate.FirstName = "John";
            validate.LastName = "Smith";

            await validate.WaitForRules();

            Assert.AreEqual("John Smith", validate.ShortName);
            Assert.AreEqual("Mr. John Smith", validate.FullName);

        }

        [TestMethod]
        public async Task ValidateAsyncRules_Rule_IsValid_True()
        {
            validate.Title = "Mr.";
            validate.FirstName = "John";
            validate.LastName = "Smith";

            await validate.WaitForRules();

            Assert.IsTrue(validate.IsValid);
        }

        [TestMethod]
        public async Task ValidateAsyncRules_Rule_IsValid_False()
        {
            validate.Title = "Mr.";
            validate.FirstName = "Error";
            validate.LastName = "Smith";

            await validate.WaitForRules();

            Assert.IsFalse(validate.IsValid);
            Assert.AreEqual("Error", validate.BrokenRulePropertyMessages(nameof(validate.FirstName)).Single());
        }

        [TestMethod]
        public async Task ValidateAsyncRules_Rule_IsValid_False_Fixed()
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
    }
}
