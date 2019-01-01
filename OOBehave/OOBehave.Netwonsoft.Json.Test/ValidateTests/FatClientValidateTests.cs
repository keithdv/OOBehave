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
    public class FatClientValidateTests
    {
        IServiceScope scope;
        IValidateObject target;
        Guid Id = Guid.NewGuid();
        string Name = Guid.NewGuid().ToString();
        FatClientContractResolver resolver;

        [TestInitialize]
        public void TestInitailize()
        {
            scope = AutofacContainer.GetLifetimeScope().Resolve<IServiceScope>();
            target = scope.Resolve<IValidateObject>();
            target.ID = Id;
            target.Name = Name;
            resolver = scope.Resolve<FatClientContractResolver>();
        }

        [TestMethod]
        public void FatClientValidate_Serialize()
        {

            var result = Serialize(target);

            Assert.IsTrue(result.Contains(Id.ToString()));
            Assert.IsTrue(result.Contains(Name));
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

        private IValidateObject Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<IValidateObject>(json, new JsonSerializerSettings
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

            // ITaskRespository and ILogger constructor parameters are injected by Autofac 
            var newTarget = Deserialize(json);

            Assert.AreEqual(target.ID, newTarget.ID);
            Assert.AreEqual(target.Name, newTarget.Name);
        }

        [TestMethod]
        public void FatClientValidate_Deserialize_Modify()
        {

            var json = Serialize(target);

            // ITaskRespository and ILogger constructor parameters are injected by Autofac 
            var newTarget = Deserialize(json);

            newTarget.Name = "Error";
            Assert.IsFalse(newTarget.IsValid);
        }

        [TestMethod]
        public void FatClientValidate_Deserialize_RuleExecute()
        {
            target.Name = "Error";
            Assert.IsFalse(target.IsValid);

            var json = Serialize(target);

            // ITaskRespository and ILogger constructor parameters are injected by Autofac 
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

            var child = target.Child = scope.Resolve<IValidateObject>();

            child.ID = Guid.NewGuid();
            child.Name = Guid.NewGuid().ToString();

            var json = Serialize(target);

            var newTarget = Deserialize(json);

            Assert.IsNotNull(newTarget.Child);
            Assert.AreSame(newTarget.Child.Parent, newTarget.Parent);
            Assert.AreEqual(child.ID, newTarget.Child.ID);
            Assert.AreEqual(child.Name, newTarget.Child.Name);

        }

        [TestMethod]
        public void FatClientValidate_Deserialize_Child_RuleExecute()
        {

            var child = target.Child = scope.Resolve<IValidateObject>();

            child.ID = Guid.NewGuid();
            child.Name = "Error";
            Assert.IsFalse(child.IsValid);
            var json = Serialize(target);

            var newTarget = Deserialize(json);

            Assert.IsFalse(newTarget.IsValid);
            Assert.IsTrue(newTarget.IsSelfValid);
            Assert.AreEqual(1, newTarget.RuleRunCount);

            Assert.IsFalse(newTarget.Child.IsValid);
            Assert.IsFalse(newTarget.Child.IsSelfValid);
            Assert.AreEqual(1, newTarget.Child.RuleRunCount);

        }

        [TestMethod]
        public void FatClientValidate_Deserialize_ValidatePropertyValue_Child()
        {
            // Ensure ValidatePropertyValue.Child is a reference to the Child 

            var child = target.Child = scope.Resolve<IValidateObject>();

            child.ID = Guid.NewGuid();
            child.Name = "Error";

            Assert.IsFalse(child.IsValid);

            var json = Serialize(target);
            var newTarget = Deserialize(json);

            Assert.IsFalse(newTarget.IsValid);

            newTarget.Child.Name = "Fine";
            Assert.IsTrue(newTarget.IsValid);

        }

        [TestMethod]
        public void FatClientValidate_Deserialize_Child_ParentRef()
        {

            var child = target.Child = scope.Resolve<IValidateObject>();

            child.ID = Guid.NewGuid();
            child.Name = Guid.NewGuid().ToString();

            var json = Serialize(target);

            // ITaskRespository and ILogger constructor parameters are injected by Autofac 
            var newTarget = Deserialize(json);

            Assert.IsNotNull(newTarget.Child);
            Assert.AreEqual(child.ID, newTarget.Child.ID);
            Assert.AreEqual(child.Name, newTarget.Child.Name);
            Assert.AreSame(newTarget.Child.Parent, newTarget);

        }

    }
}

