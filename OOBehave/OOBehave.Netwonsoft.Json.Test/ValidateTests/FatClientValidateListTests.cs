using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using OOBehave.Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOBehave.Netwonsoft.Json.Test.ValidateTests
{

    [TestClass]
    public class FatClientValidateListTests
    {
        IServiceScope scope;
        IValidateObjectList target;
        IValidateObject child;
        Guid Id = Guid.NewGuid();
        string Name = Guid.NewGuid().ToString();
        FatClientContractResolver resolver;

        [TestInitialize]
        public void TestInitailize()
        {
            scope = AutofacContainer.GetLifetimeScope().Resolve<IServiceScope>();
            target = scope.Resolve<IValidateObjectList>();
            target.ID = Id;
            target.Name = Name;
            resolver = scope.Resolve<FatClientContractResolver>();

            child = scope.Resolve<IValidateObject>();
            child.ID = Guid.NewGuid();
            child.Name = Guid.NewGuid().ToString();
            target.Add(child);
        }

        [TestMethod]
        public void FatClientValidate_Serialize()
        {

            var result = Serialize(target);

            Assert.IsTrue(result.Contains(Id.ToString()));
            Assert.IsTrue(result.Contains(Name));
        }

        [TestMethod]
        public void FatClientValidate_Serialize_Invalid()
        {
            target.Name = "Error";
            var result = Serialize(target);

            Assert.IsFalse(target.IsValid);
            Assert.IsTrue(result.Contains("Error")); // Weak check
        }

        private string Serialize(object target)
        {
            return JsonConvert.SerializeObject(target, new JsonSerializerSettings()
            {
                ContractResolver = resolver,
                TypeNameHandling = TypeNameHandling.All,
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                Formatting = Formatting.Indented,
                Converters = new List<JsonConverter>() { scope.Resolve<ListBaseCollectionConverter>() }
            });
        }

        private IValidateObjectList Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<IValidateObjectList>(json, new JsonSerializerSettings
            {
                ContractResolver = resolver,
                TypeNameHandling = TypeNameHandling.All,
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                Converters = new List<JsonConverter>() { scope.Resolve<ListBaseCollectionConverter>() }
            });
        }

        [TestMethod]
        public void FatClientValidate_Deserialize()
        {

            var json = Serialize(target);

            var newTarget = Deserialize(json);

            Assert.AreEqual(target.ID, newTarget.ID);
            Assert.AreEqual(target.Name, newTarget.Name);
        }

        [TestMethod]
        public void FatClientValidate_Deserialize_RuleExecute()
        {
            target.Name = "Error";
            Assert.IsFalse(target.IsValid);

            var json = Serialize(target);

            var newTarget = Deserialize(json);

            Assert.AreEqual(2, newTarget.RuleRunCount); // Ensure that RuleExecute was deserialized, not run
            Assert.AreEqual(1, newTarget.Rules.Count());
            Assert.IsFalse(newTarget.IsValid);

            Assert.AreEqual(1, newTarget.BrokenRuleMessages.Count());
            Assert.AreEqual("Error", newTarget.BrokenRuleMessages.Single());

        }


        [TestMethod]
        public void FatClientValidate_Deserialize_Child()
        {

            var json = Serialize(target);

            var newTarget = Deserialize(json);

            Assert.AreEqual(child.ID, newTarget.Single().ID);
            Assert.AreEqual(child.Name, newTarget.Single().Name);

        }

        [TestMethod]
        public void FatClientValidate_Deserialize_Child_RuleExecute()
        {

            child.Name = "Error";

            var json = Serialize(target);
            var newTarget = Deserialize(json);

            Assert.IsFalse(child.IsValid);

            Assert.IsFalse(newTarget.IsValid);
            Assert.IsTrue(newTarget.IsSelfValid);
            Assert.AreEqual(1, newTarget.RuleRunCount);

            Assert.IsFalse(newTarget.Single().IsValid);
            Assert.IsFalse(newTarget.Single().IsSelfValid);
            Assert.AreEqual(child.RuleRunCount, newTarget.Single().RuleRunCount);

        }

        [TestMethod]
        public void FatClientValidate_Deserialize_IsValid_False_Fix()
        {
            // This caught a really critical issue that lead to the RuleExecute.TransferredResults logic
            // After being transferred the RuleIndex values would not match up
            // So the object would be stuck in InValid

            target.Name = "Error";

            var json = Serialize(target);
            var newTarget = Deserialize(json);

            Assert.IsFalse(target.IsValid);
            Assert.IsFalse(newTarget.IsValid);

            newTarget.Name = "Fine";
            Assert.IsTrue(newTarget.IsValid);

        }


    }
}

