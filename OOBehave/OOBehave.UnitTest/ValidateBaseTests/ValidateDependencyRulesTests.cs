using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOBehave.Rules;
using OOBehave.UnitTest.Objects;
using OOBehave.UnitTest.PersonObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOBehave.UnitTest.ValidateBaseTests
{
    public interface IValidateDependencyRules : IPersonBase { }

    public class ValidateDependencyRules : PersonValidateBase<ValidateDependencyRules>, IValidateDependencyRules
    {

        public ValidateDependencyRules(IValidateBaseServices<ValidateDependencyRules> services,
                IShortNameDependencyRule<ValidateDependencyRules> shortNameRule,
                IFullNameDependencyRule<ValidateDependencyRules> fullNameRule) : base(services)
        {
            RuleExecute.AddRule(shortNameRule);
            RuleExecute.AddRule(fullNameRule);
        }

    }

    [TestClass]
    public class ValidateDependencyRulesTests
    {


        IValidateDependencyRules validate;
        ILifetimeScope scope;

        [TestInitialize]
        public void TestInitailize()
        {
            scope = AutofacContainer.GetLifetimeScope();
            validate = scope.Resolve<IValidateDependencyRules>();
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
        //    Assert.IsInstanceOfType(((IRegisteredRuleList<Validate>) Core.Factory.StaticFactory.RuleManager.RegisteredRules[typeof(Validate)]).First(), typeof(ShortNameRule));
        //    Assert.IsInstanceOfType(((IRegisteredRuleList<Validate>)Core.Factory.StaticFactory.RuleManager.RegisteredRules[typeof(Validate)]).Take(2).Last(), typeof(FullNameRule));
        //}

        [TestMethod]
        public void Validate_Rule()
        {

            validate.FirstName = "John";
            validate.LastName = "Smith";

            Assert.AreEqual("John Smith", validate.ShortName);

        }

        [TestMethod]
        public void Validate_Rule_Recursive()
        {

            validate.Title = "Mr.";
            validate.FirstName = "John";
            validate.LastName = "Smith";

            Assert.AreEqual("John Smith", validate.ShortName);
            Assert.AreEqual("Mr. John Smith", validate.FullName);

        }

        [TestMethod]
        public void Validate_Rule_IsValid_True()
        {
            validate.Title = "Mr.";
            validate.FirstName = "John";
            validate.LastName = "Smith";

            Assert.IsTrue(validate.IsValid);
        }

        [TestMethod]
        public void Validate_Rule_IsValid_False()
        {
            validate.Title = "Mr.";
            validate.FirstName = "Error";
            validate.LastName = "Smith";

            Assert.IsFalse(validate.IsValid);
            Assert.AreEqual("Error", validate.BrokenRulePropertyMessages(nameof(validate.FirstName)).Single());
        }

        [TestMethod]
        public void Validate_Rule_IsValid_False_Fixed()
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
        public void ValidateDependencyRules_DisposableDependency_Count()
        {
            var dependencies = scope.Resolve<DisposableDependencyList>();

            Assert.AreEqual(2, dependencies.Count);
            Assert.AreEqual(2, dependencies.Select(x => x.UniqueId).Distinct().Count());
            Assert.IsFalse(dependencies.Where(x => x.IsDisposed).Any());
        }

        [TestMethod]
        public void ValidateDependencyRules_DisposableDependency_Unique()
        {
            var dependencies = scope.Resolve<DisposableDependencyList>();

            Assert.AreEqual(2, dependencies.Select(x => x.UniqueId).Distinct().Count());
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
