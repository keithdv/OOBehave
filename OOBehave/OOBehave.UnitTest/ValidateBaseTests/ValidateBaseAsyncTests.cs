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
    public class ValidateBaseAsyncTests
    {


        private ILifetimeScope scope;
        private IValidateAsyncObject validate;
        private IValidateAsyncObject child;
        private List<string> propertyChangedList = new List<string>();

        [TestInitialize]
        public void TestInitailize()
        {
            scope = AutofacContainer.GetLifetimeScope();
            var validateDto = scope.Resolve<IReadOnlyList<PersonDto>>().Where(p => !p.FatherId.HasValue && !p.MotherId.HasValue).First();
            validate = scope.Resolve<IValidateAsyncObject>();
            child = scope.Resolve<IValidateAsyncObject>();
            validate.Child = child;

            validate.PropertyChanged += (o, e) => propertyChangedList.Add(e.PropertyName);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Assert.IsFalse(validate.IsBusy);
            Assert.IsFalse(validate.IsSelfBusy);
        }

        [TestMethod]
        public void ValidateBaseAsync_Const()
        {

        }


        [TestMethod]
        public async Task ValidateBaseAsync_Set()
        {
            validate.FirstName = "Keith";
            await validate.WaitForRules();
        }

        [TestMethod]
        public async Task ValidateBaseAsync_Set_IsBusy_True()
        {
            validate.FirstName = "Keith";
            Assert.IsTrue(validate.IsBusy);
            Assert.IsTrue(validate.IsSelfBusy);
            await validate.WaitForRules();
            Assert.IsFalse(validate.IsBusy);
            Assert.IsFalse(validate.IsSelfBusy);
        }

        [TestMethod]
        public async Task ValidateBaseAsync_SetGet()
        {
            var name = Guid.NewGuid().ToString();
            validate.ShortName = name;
            Assert.AreEqual(name, validate.ShortName);
            await validate.WaitForRules();
        }

        //[TestMethod]
        //public void ValidateBaseAsync_RulesCreated()
        //{
        //    Assert.IsTrue(Core.Factory.StaticFactory.RuleManager.RegisteredRules.ContainsKey(typeof(ValidateBaseAsync)));
        //    Assert.AreEqual(3, Core.Factory.StaticFactory.RuleManager.RegisteredRules[typeof(ValidateBaseAsync)].Count);
        //    Assert.IsInstanceOfType(((IRegisteredRuleList<ValidateBaseAsync>) Core.Factory.StaticFactory.RuleManager.RegisteredRules[typeof(ValidateBaseAsync)]).First(), typeof(ShortNameAsyncRule));
        //    Assert.IsInstanceOfType(((IRegisteredRuleList<ValidateBaseAsync>)Core.Factory.StaticFactory.RuleManager.RegisteredRules[typeof(ValidateBaseAsync)]).Take(2).Last(), typeof(FullNameAsyncRule));
        //    Assert.IsInstanceOfType(((IRegisteredRuleList<ValidateBaseAsync>)Core.Factory.StaticFactory.RuleManager.RegisteredRules[typeof(ValidateBaseAsync)]).Take(3).Last(), typeof(FirstNameTargetAsyncRule));
        //}


        [TestMethod]
        public async Task ValidateBaseAsync_Rule()
        {

            validate.FirstName = "John";
            validate.LastName = "Smith";

            await validate.WaitForRules();

            Assert.AreEqual("John Smith", validate.ShortName);

        }

        [TestMethod]
        public async Task ValidateBaseAsync_Rule_Recursive()
        {

            validate.Title = "Mr.";
            validate.FirstName = "John";
            validate.LastName = "Smith";

            await validate.WaitForRules();

            Assert.AreEqual("John Smith", validate.ShortName);
            Assert.AreEqual("Mr. John Smith", validate.FullName);

        }

        [TestMethod]
        public async Task ValidateBaseAsync_Rule_IsValid_True()
        {
            validate.Title = "Mr.";
            validate.FirstName = "John";
            validate.LastName = "Smith";

            await validate.WaitForRules();

            Assert.IsTrue(validate.IsValid);
        }

        [TestMethod]
        public async Task ValidateBaseAsync_Rule_IsValid_False()
        {
            validate.Title = "Mr.";
            validate.FirstName = "Error";
            validate.LastName = "Smith";

            await validate.WaitForRules();

            Assert.IsFalse(validate.IsValid);
            Assert.IsFalse(validate[nameof(validate.FirstName)].IsValid);
        }

        [TestMethod]
        public async Task ValidateBaseAsync_Rule_IsValid_False_Fixed()
        {
            validate.Title = "Mr.";
            validate.FirstName = "Error";
            validate.LastName = "Smith";

            await validate.WaitForRules();

            Assert.IsFalse(validate.IsValid);

            validate.FirstName = "John";

            await validate.WaitForRules();

            Assert.IsTrue(validate.IsValid);
            Assert.IsTrue(validate[nameof(validate.FirstName)].IsValid);

        }


        [TestMethod]
        public async Task ValidateBaseAsync_Invalid()
        {
            validate.FirstName = "Error";

            await validate.WaitForRules();

            Assert.IsFalse(validate.IsBusy);
            Assert.IsFalse(validate.IsValid);
            Assert.IsFalse(validate.IsSelfValid);
            Assert.IsTrue(child.IsValid);
            Assert.IsTrue(child.IsSelfValid);
        }

        [TestMethod]
        public async Task ValidateBaseAsync_Child_Invalid()
        {
            child.FirstName = "Error";
            await validate.WaitForRules();

            Assert.IsFalse(validate.IsBusy);
            Assert.IsFalse(validate.IsValid);
            Assert.IsTrue(validate.IsSelfValid);
            Assert.IsFalse(child.IsValid);
            Assert.IsFalse(child.IsSelfValid);
        }

        [TestMethod]
        public async Task ValidateBaseAsync_Child_IsBusy()
        {
            child.FirstName = "Error";

            Assert.IsTrue(validate.IsBusy);
            Assert.IsFalse(validate.IsSelfBusy);
            Assert.IsTrue(child.IsBusy);
            Assert.IsTrue(child.IsSelfBusy);

            await validate.WaitForRules();

            Assert.IsFalse(validate.IsBusy);
            Assert.IsFalse(validate.IsValid);
            Assert.IsTrue(validate.IsSelfValid);
            Assert.IsFalse(child.IsValid);
            Assert.IsFalse(child.IsSelfValid);
        }

        [TestMethod]
        public void ValidateBaseAsync_PropertyWithNoRules()
        {
            // If the property has no rules than we shouldn't call CheckRulesQueue

            validate.NoRules = "Value";

            Assert.IsFalse(validate.IsBusy);

        }

        [TestMethod]
        public async Task ValidateBaseAsync_PropertyIsBusy_TrueAsync()
        {
            validate.FirstName = "Valid";
            Assert.IsTrue(validate.PropertyIsBusy[nameof(IValidateObject.FirstName)]);
            await validate.WaitForRules();
        }

        [TestMethod]
        public async Task ValidateBaseAsync_PropertyIsBusy_False()
        {
            Assert.IsFalse(validate.PropertyIsBusy[nameof(IValidateObject.FirstName)]);
            validate.FirstName = "Valid";
            await validate.WaitForRules();
            Assert.IsFalse(validate.PropertyIsBusy[nameof(IValidateObject.FirstName)]);
        }
    }
}
