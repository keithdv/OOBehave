using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.ValidateAsyncRules
{

    [TestClass]
    public class ValidateAsyncRulesBaseTests
    {


        ValidateAsyncRules validate;

        [TestInitialize]
        public void TestInitailize()
        {
            validate = new ValidateAsyncRules(Core.Factory.StaticFactory.CreateValidateBaseServices<ValidateAsyncRules>());
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

        [TestMethod]
        public void ValidateAsyncRules_RulesCreated()
        {
            Assert.IsTrue(Core.Factory.StaticFactory.RuleManager.RegisteredRules.ContainsKey(typeof(ValidateAsyncRules)));
            Assert.AreEqual(2, Core.Factory.StaticFactory.RuleManager.RegisteredRules[typeof(ValidateAsyncRules)].Count);
            Assert.IsInstanceOfType(((IRegisteredRuleList<ValidateAsyncRules>) Core.Factory.StaticFactory.RuleManager.RegisteredRules[typeof(ValidateAsyncRules)]).First(), typeof(NameCascadeAsyncRule));
            Assert.IsInstanceOfType(((IRegisteredRuleList<ValidateAsyncRules>)Core.Factory.StaticFactory.RuleManager.RegisteredRules[typeof(ValidateAsyncRules)]).Last(), typeof(TitleCascadeAsyncRule));
        }

        [TestMethod]
        public async Task ValidateAsyncRules_NameCascadeRule()
        {

            validate.FirstName = "John";
            validate.LastName = "Smith";

            System.Diagnostics.Debug.WriteLine("WaitForRules");

            await validate.WaitForRules();

            Assert.AreEqual("John Smith", validate.ShortName);

        }

        [TestMethod]
        public async Task ValidateAsyncRules_TitleCascadeRule()
        {

            validate.Title = "Mr.";
            validate.FirstName = "John";
            validate.LastName = "Smith";

            System.Diagnostics.Debug.WriteLine("WaitForRules");

            await validate.WaitForRules();

            Assert.AreEqual("John Smith", validate.ShortName);
            Assert.AreEqual("Mr. John Smith", validate.FullName);

        }
    }
}
