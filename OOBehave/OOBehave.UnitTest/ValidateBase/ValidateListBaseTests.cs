using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOBehave.Rules;
using OOBehave.UnitTest.PersonObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOBehave.UnitTest.ValidateBase
{

    public interface IValidateList : IPersonBase { }

    public class ValidateList : ValidateListBase<ValidateList, IValidate>, IValidateList
    {

        public ValidateList(IValidateListBaseServices<ValidateList, IValidate> services,
            IShortNameRule<ValidateList> shortNameRule,
            IFullNameRule<ValidateList> fullNameRule,
            IPersonRule<ValidateList> personRule
            ) : base(services)
        {
            RuleExecute.AddRules(shortNameRule, fullNameRule, personRule);
        }

        public string FirstName { get { return Getter<string>(); } set { Setter(value); } }

        public string LastName { get { return Getter<string>(); } set { Setter(value); } }

        public string ShortName { get { return Getter<string>(); } set { Setter(value); } }

        public string Title { get { return Getter<string>(); } set { Setter(value); } }

        public string FullName { get { return Getter<string>(); } set { Setter(value); } }

        public uint? Age { get => Getter<uint>(); set => Setter(value); }
    }

    [TestClass]
    public class ValidateListBaseTests
    {


        IValidateList ValidateList;

        [TestInitialize]
        public void TestInitailize()
        {
            ValidateList = AutofacContainer.GetLifetimeScope().Resolve<IValidateList>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Assert.IsFalse(ValidateList.IsBusy);
            Assert.IsFalse(ValidateList.IsSelfBusy);
        }

        [TestMethod]
        public void ValidateList_Constructor()
        {

        }


        [TestMethod]
        public void ValidateList_Set()
        {
            ValidateList.FirstName = "Keith";
        }

        [TestMethod]
        public void ValidateList_SetGet()
        {
            var name = Guid.NewGuid().ToString();
            ValidateList.ShortName = name;
            Assert.AreEqual(name, ValidateList.ShortName);
        }

        //[TestMethod]
        //public void ValidateList_RulesCreated()
        //{
        //    Assert.IsTrue(Core.Factory.StaticFactory.RuleManager.RegisteredRules.ContainsKey(typeof(ValidateList)));
        //    Assert.AreEqual(3, Core.Factory.StaticFactory.RuleManager.RegisteredRules[typeof(ValidateList)].Count);
        //    Assert.IsInstanceOfType(((IRegisteredRuleList<ValidateList>) Core.Factory.StaticFactory.RuleManager.RegisteredRules[typeof(ValidateList)]).First(), typeof(ShortNameCascadeRule));
        //    Assert.IsInstanceOfType(((IRegisteredRuleList<ValidateList>)Core.Factory.StaticFactory.RuleManager.RegisteredRules[typeof(ValidateList)]).Take(2).Last(), typeof(FullNameCascadeRule));
        //    Assert.IsInstanceOfType(((IRegisteredRuleList<ValidateList>)Core.Factory.StaticFactory.RuleManager.RegisteredRules[typeof(ValidateList)]).Take(3).Last(), typeof(FirstNameTargetRule));
        //}

        [TestMethod]
        public void ValidateList_CascadeRule()
        {

            ValidateList.FirstName = "John";
            ValidateList.LastName = "Smith";

            Assert.AreEqual("John Smith", ValidateList.ShortName);

        }

        [TestMethod]
        public void ValidateList_CascadeRule_Recursive()
        {

            ValidateList.Title = "Mr.";
            ValidateList.FirstName = "John";
            ValidateList.LastName = "Smith";

            Assert.AreEqual("John Smith", ValidateList.ShortName);
            Assert.AreEqual("Mr. John Smith", ValidateList.FullName);

        }

        [TestMethod]
        public void ValidateList_CascadeRule_IsValid_True()
        {
            ValidateList.Title = "Mr.";
            ValidateList.FirstName = "John";
            ValidateList.LastName = "Smith";

            Assert.IsTrue(ValidateList.IsValid);
        }

        [TestMethod]
        public void ValidateList_CascadeRule_IsValid_False()
        {
            ValidateList.Title = "Mr.";
            ValidateList.FirstName = "Error";
            ValidateList.LastName = "Smith";

            Assert.IsFalse(ValidateList.IsValid);
            Assert.AreEqual("Error", ValidateList.BrokenRulePropertyMessages(nameof(ValidateList.FirstName)).Single());
        }

        [TestMethod]
        public void ValidateList_CascadeRule_IsValid_False_Fixed()
        {
            ValidateList.Title = "Mr.";
            ValidateList.FirstName = "Error";
            ValidateList.LastName = "Smith";

            Assert.IsFalse(ValidateList.IsValid);

            ValidateList.FirstName = "John";

            Assert.IsTrue(ValidateList.IsValid);
            Assert.AreEqual(0, ValidateList.BrokenRulePropertyMessages(nameof(ValidateList.FirstName)).Count());

        }

        [TestMethod]
        public void ValidateList_TargetRule_IsValid_False()
        {

            ValidateList.Title = "Mr.";
            ValidateList.FirstName = "John";
            ValidateList.LastName = "Smith";
            Assert.IsTrue(ValidateList.IsValid);

            ValidateList.ShortName = "";

            Assert.IsFalse(ValidateList.IsValid);
            Assert.AreEqual(1, ValidateList.BrokenRulePropertyMessages(nameof(ValidateList.ShortName)).Count());

        }

    }
}
