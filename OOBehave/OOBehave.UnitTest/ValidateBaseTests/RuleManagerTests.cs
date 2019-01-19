using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.ValidateBaseTests
{

    namespace RuleManager
    {

        public class ValidateObject : ValidateBase<ValidateObject>
        {
            public ValidateObject(IValidateBaseServices<ValidateObject> services) : base(services)
            {

            }

            public string FirstName { get => Getter<string>(); set => Setter(value); }

            [Required]
            public string NoInitialValue { get => Getter<string>(); set => Setter(value); }
        }

        public class RuleManagerAsyncTest : AsyncRuleBase<ValidateObject>
        {
            public RuleManagerAsyncTest()
            {
                TriggerProperties.Add(nameof(ValidateObject.FirstName));
            }

            public override async Task<IRuleResult> Execute(ValidateObject target, CancellationToken token)
            {
                await Task.Delay(10);
                if (target.FirstName == "Error")
                {
                    return RuleResult.PropertyError(nameof(ValidateObject.FirstName), "FirstName Error");
                }
                return RuleResult.Empty();
            }
        }

        [TestClass]
        public class RuleManagerTests
        {

            private ILifetimeScope scope;
            private ValidateObject validate;
            private IRuleManager<ValidateObject> ruleManager;

            [TestInitialize]
            public void TestInitailize()
            {
                scope = AutofacContainer.GetLifetimeScope();
                var services = scope.Resolve<IValidateBaseServices<ValidateObject>>();
                validate = new ValidateObject(services);
                ruleManager = services.RuleManager;
                ruleManager.AddRule(new RuleManagerAsyncTest());
            }

            [TestCleanup]
            public void TestCleanup()
            {
                Assert.IsFalse(ruleManager.IsBusy);
            }

            [TestMethod]
            public async Task RuleManager_PropertyValueMeta_IsBusy()
            {
                validate.FirstName = "Value";
                Assert.IsTrue(validate[nameof(ValidateObject.FirstName)].IsBusy);
                await ruleManager.WaitForRules;
            }

            [TestMethod]
            public async Task RuleManager_PropertyValueMeta_IsBusy_False()
            {
                validate.FirstName = "Value";
                await ruleManager.WaitForRules;
                Assert.IsFalse(validate[nameof(ValidateObject.FirstName)].IsBusy);
            }

            [TestMethod]
            public async Task RuleManager_PropertyValueMeta_IsValid()
            {
                validate.FirstName = "Keith";
                await ruleManager.WaitForRules;
                Assert.IsTrue(validate[nameof(ValidateObject.FirstName)].IsValid);
            }

            [TestMethod]
            public async Task RuleManager_PropertyValueMeta_IsValid_False()
            {
                validate.FirstName = "Error";
                await ruleManager.WaitForRules;
                Assert.IsFalse(validate[nameof(ValidateObject.FirstName)].IsValid);
            }

            [TestMethod]
            public async Task RuleManager_PropertyValueMeta_ErrorMessage()
            {
                validate.FirstName = "Error";
                await ruleManager.WaitForRules;
                Assert.IsFalse(string.IsNullOrWhiteSpace(validate[nameof(ValidateObject.FirstName)].ErrorMessage));
            }

            [TestMethod]
            public async Task RuleManager_PropertyValueMeta_NoErrorMessage()
            {
                validate.FirstName = "Value";
                await ruleManager.WaitForRules;
                Assert.IsTrue(string.IsNullOrWhiteSpace(validate[nameof(ValidateObject.FirstName)].ErrorMessage));
            }

            [TestMethod]
            public async Task RuleManager_NoInitialValue()
            {
                // PropertyValues weren't getting created until a load or set. 
                // Now get created if there is a broken rule for them

                await ruleManager.CheckRulesForProperty(nameof(ValidateObject.NoInitialValue));

                Assert.IsFalse(validate[nameof(ValidateObject.NoInitialValue)].IsValid);
            }

        }
    }
}