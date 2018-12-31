using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOBehave.Portal;
using OOBehave.Rules;
using OOBehave.UnitTest.PersonObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.ValidateBaseTests
{

    public interface IValidate : IPersonBase { uint RuleRunCount { get; } }

    public class Validate : PersonValidateBase<Validate>, IValidate
    {
        public IShortNameRule<Validate> ShortNameRule { get; }
        public IFullNameRule<Validate> FullNameRule { get; }

        public Validate(IValidateBaseServices<Validate> services,
            IShortNameRule<Validate> shortNameRule,
            IFullNameRule<Validate> fullNameRule
            ) : base(services)
        {
            RuleExecute.AddRules(shortNameRule, fullNameRule);
            ShortNameRule = shortNameRule;
            FullNameRule = fullNameRule;
        }

        [Fetch]
        [FetchChild]
        private void Fetch(PersonDto person)
        {
            base.FillFromDto(person);
        }

        public uint RuleRunCount => ShortNameRule.RunCount + FullNameRule.RunCount;

    }

    [TestClass]
    public class ValidateBaseTests
    {


        IValidate validate;

        [TestInitialize]
        public void TestInitailize()
        {
            validate = AutofacContainer.GetLifetimeScope().Resolve<IValidate>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Assert.IsFalse(validate.IsBusy);
            Assert.IsFalse(validate.IsSelfBusy);
        }

        [TestMethod]
        public void Validate_Constructor()
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
        public async Task Validate_RunSelfRules()
        {
            var ruleCount = validate.RuleRunCount;
            await validate.RunSelfRules();
            Assert.AreEqual(ruleCount + 2, validate.RuleRunCount);
        }
    }
}
