using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOBehave.Rules;
using OOBehave.UnitTest.Objects;
using OOBehave.UnitTest.PersonObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOBehave.UnitTest.ValidateBase
{
    public class ValidateDependencyRules : PersonBase<ValidateDependencyRules>, IValidate
    {

        public ValidateDependencyRules(IValidateBaseServices<ValidateDependencyRules> services,
                ShortNameDependencyRule<ValidateDependencyRules> shortNameRule,
                FullNameDependencyRule<ValidateDependencyRules> fullNameRule,
                PersonDependencyRule<ValidateDependencyRules> firstNameRule) : base(services)
        {
            RuleExecute.AddRule(shortNameRule);
            RuleExecute.AddRule(fullNameRule);
            RuleExecute.AddRule(firstNameRule);
        }

    }

    [TestClass]
    public class ValidateDependencyRulesTests
    {


        ValidateDependencyRules validate;
        ILifetimeScope scope;

        [TestInitialize]
        public void TestInitailize()
        {
            scope = AutofacContainer.GetLifetimeScope();
            validate = scope.Resolve<ValidateDependencyRules>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            scope.Dispose();
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

        //[TestMethod]
        //public void Validate_RulesCreated()
        //{
        //    Assert.IsTrue(Core.Factory.StaticFactory.RuleManager.RegisteredRules.ContainsKey(typeof(Validate)));
        //    Assert.AreEqual(3, Core.Factory.StaticFactory.RuleManager.RegisteredRules[typeof(Validate)].Count);
        //    Assert.IsInstanceOfType(((IRegisteredRuleList<Validate>) Core.Factory.StaticFactory.RuleManager.RegisteredRules[typeof(Validate)]).First(), typeof(ShortNameCascadeRule));
        //    Assert.IsInstanceOfType(((IRegisteredRuleList<Validate>)Core.Factory.StaticFactory.RuleManager.RegisteredRules[typeof(Validate)]).Take(2).Last(), typeof(FullNameCascadeRule));
        //    Assert.IsInstanceOfType(((IRegisteredRuleList<Validate>)Core.Factory.StaticFactory.RuleManager.RegisteredRules[typeof(Validate)]).Take(3).Last(), typeof(FirstNameTargetRule));
        //}

        [TestMethod]
        public void Validate_CascadeRule()
        {

            validate.FirstName = "John";
            validate.LastName = "Smith";

            Assert.AreEqual("John Smith", validate.ShortName);

        }

        [TestMethod]
        public void Validate_CascadeRule_Recursive()
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
            validate.Title = "Mr.";
            validate.FirstName = "Error";
            validate.LastName = "Smith";

            Assert.IsFalse(validate.IsValid);
            Assert.AreEqual("Error", validate.BrokenRulePropertyMessages(nameof(validate.FirstName)).Single());
        }

        [TestMethod]
        public void Validate_CascadeRule_IsValid_False_Fixed()
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
        public void Validate_TargetRule_IsValid_False()
        {

            validate.Title = "Mr.";
            validate.FirstName = "John";
            validate.LastName = "Smith";
            Assert.IsTrue(validate.IsValid);

            validate.ShortName = "";

            Assert.IsFalse(validate.IsValid);
            Assert.AreEqual(1, validate.BrokenRulePropertyMessages(nameof(validate.ShortName)).Count());

        }


        [TestMethod]
        public void ValidateDependencyRules_DisposableDependency_Count()
        {
            var dependencies = scope.Resolve<DisposableDependencyList>();

            Assert.AreEqual(3, dependencies.Count);
            Assert.AreEqual(3, dependencies.Select(x => x.UniqueId).Distinct().Count());
            Assert.IsFalse(dependencies.Where(x => x.IsDisposed).Any());
        }

        [TestMethod]
        public void ValidateDependencyRules_DisposableDependency_Unique()
        {
            var dependencies = scope.Resolve<DisposableDependencyList>();

            Assert.AreEqual(3, dependencies.Select(x => x.UniqueId).Distinct().Count());
        }

        [TestMethod]
        public void ValidateDependencyRules_DisposableDependency_NotDisposed()
        {
            var dependencies = scope.Resolve<DisposableDependencyList>();

            Assert.IsFalse(dependencies.Where(x => x.IsDisposed).Any());
        }

        [TestMethod]
        public void ValidateDependencyRules_DisposableDependency_Dispose()
        {
            var dependencies = scope.Resolve<DisposableDependencyList>();

            scope.Dispose();

            Assert.IsFalse(dependencies.Where(x => !x.IsDisposed).Any());
        }

    }
}
