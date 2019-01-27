using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using OOBehave.Newtonsoft.Json;
using OOBehave.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOBehave.Netwonsoft.Json.Test.ValidateTests
{

    [TestClass]
    public class ValidatePopulateTests
    {
        IServiceScope scope;
        IValidateObject target;
        Guid Id = Guid.NewGuid();
        string Name = Guid.NewGuid().ToString();
        private ISerializer serializer;

        [TestInitialize]
        public void TestInitailize()
        {
            scope = AutofacContainer.GetLifetimeScope().Resolve<IServiceScope>();
            target = scope.Resolve<IValidateObject>();
            target.ID = Id;
            target.Name = Name;
            serializer = scope.Resolve<ISerializer>();
        }

        private string Serialize(object target)
        {
            return serializer.Serialize(target);
        }

        private IValidateObject Deserialize(string json)
        {
            return serializer.Deserialize<IValidateObject>(json);
        }

        private void SimulateServerSave(Action<IValidateObject> modify = null)
        {
            var json = Serialize(target);
            var newTarget = Deserialize(json);
            modify?.Invoke(newTarget);
            json = Serialize(newTarget);
            serializer.Populate(json, target);
        }

        [TestMethod]
        public void ValidatePopulate_Deserialize()
        {
            var id = Guid.NewGuid();
            SimulateServerSave(v => v.ID = id);
            Assert.AreEqual(id, target.ID);
        }

        [TestMethod]
        public void ValidatePopulate_Deserialize_Modify()
        {
            SimulateServerSave();
            target.Name = "Error";
            Assert.IsFalse(target.IsValid);
            Assert.IsFalse(target.PropertyIsValid[nameof(IValidateObject.Name)]);
        }

        [TestMethod]
        public void ValidatePopulate_Deserialize_RuleManager()
        {

            SimulateServerSave(v => v.Name = "Error");

            Assert.AreEqual(2, target.RuleRunCount); // Ensure that RuleManager was deserialized, not run
            Assert.AreEqual(1, target.Rules.Count());
            Assert.IsFalse(target.IsValid);

            Assert.IsFalse(target[nameof(IValidateObject.Name)].IsValid);
            Assert.IsFalse(target.PropertyIsValid[nameof(IValidateObject.Name)]);
        }


        [TestMethod]
        public void ValidatePopulate_Deserialize_Child()
        {

            target.Child = scope.Resolve<IValidateObject>();

            var id = Guid.NewGuid();
            var name = Guid.NewGuid().ToString();

            SimulateServerSave(v =>
            {
                v.Child.ID = id;
                v.Child.Name = name;
            });

            Assert.IsNotNull(target.Child);
            Assert.AreSame(target.Child.Parent, target);
            Assert.AreEqual(id, target.Child.ID);
            Assert.AreEqual(name, target.Child.Name);

        }

        [TestMethod]
        public void ValidatePopulate_Deserialize_Child_RuleManager()
        {

            target.Child = scope.Resolve<IValidateObject>();

            var id = Guid.NewGuid();
            var name = "Error";

            SimulateServerSave(v =>
            {
                v.Child.ID = id;
                v.Child.Name = name;
            });

            Assert.IsFalse(target.IsValid);
            Assert.IsTrue(target.IsSelfValid);
            Assert.AreEqual(1, target.RuleRunCount);

            Assert.IsFalse(target.Child.IsValid);
            Assert.IsFalse(target.Child.IsSelfValid);
            Assert.AreEqual(1, target.Child.RuleRunCount);

        }

        [TestMethod]
        public void ValidatePopulate_Deserialize_ValidatePropertyValue_Child()
        {
            // Ensure ValidatePropertyValue.Child is a reference to the Child 

            target.Child = scope.Resolve<IValidateObject>();

            var id = Guid.NewGuid();
            var name = "Error";

            SimulateServerSave(v =>
            {
                v.Child.ID = id;
                v.Child.Name = name;
            });

            Assert.IsFalse(target.IsValid);

            target.Child.Name = "Fine";
            Assert.IsTrue(target.IsValid);

        }

        [TestMethod]
        public void ValidatePopulate_Deserialize_Child_ParentRef()
        {

            var child = target.Child = scope.Resolve<IValidateObject>();

            child.ID = Guid.NewGuid();
            child.Name = Guid.NewGuid().ToString();

            SimulateServerSave();

            Assert.IsNotNull(target.Child);
            Assert.AreEqual(child.ID, target.Child.ID);
            Assert.AreEqual(child.Name, target.Child.Name);
            Assert.AreSame(target.Child.Parent, target);

        }

        [TestMethod]
        public void ValidatePopulate_Deserialize_MarkInvalid()
        {
            // This caught a really critical issue that lead to the RuleManager.TransferredResults logic
            // After being transferred the RuleIndex values would not match up
            // So the object would be stuck in InValid

            target.MarkInvalid(Guid.NewGuid().ToString());


            SimulateServerSave();

            Assert.IsFalse(target.IsValid);
            Assert.IsFalse(target.IsValid);
            Assert.IsNotNull(target.OverrideResult);
        }
    }
}

