using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using OOBehave.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOBehave.Netwonsoft.Json.Test.EditTests
{

    [TestClass]
    public class FatClientEditTests
    {
        IServiceScope scope;
        IEditObject target;
        Guid Id = Guid.NewGuid();
        string Name = Guid.NewGuid().ToString();
        FatClientContractResolver resolver;

        [TestInitialize]
        public void TestInitailize()
        {
            scope = AutofacContainer.GetLifetimeScope().Resolve<IServiceScope>();
            target = scope.Resolve<IEditObject>();
            target.ID = Id;
            target.Name = Name;
            resolver = scope.Resolve<FatClientContractResolver>();
        }

        private string Serialize(object target)
        {
            return JsonConvert.SerializeObject(target, new JsonSerializerSettings()
            {
                ContractResolver = resolver,
                TypeNameHandling = TypeNameHandling.All,
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                Formatting = Formatting.Indented
            });
        }

        private IEditObject Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<IEditObject>(json, new JsonSerializerSettings
            {
                ContractResolver = resolver,
                TypeNameHandling = TypeNameHandling.All,
                PreserveReferencesHandling = PreserveReferencesHandling.All
            });
        }

        [TestMethod]
        public void FatClientEdit_Serialize()
        {

            var result = Serialize(target);

            Assert.IsTrue(result.Contains(Id.ToString()));
            Assert.IsTrue(result.Contains(Name));
        }

        [TestMethod]
        public void FatClientEdit_Deserialize()
        {

            var json = Serialize(target);

            var newTarget = Deserialize(json);

            Assert.AreEqual(target.ID, newTarget.ID);
            Assert.AreEqual(target.Name, newTarget.Name);
        }

        [TestMethod]
        public void FatClientEdit_Deserialize_Child()
        {

            var child = target.Child = scope.Resolve<IEditObject>();

            child.ID = Guid.NewGuid();
            child.Name = Guid.NewGuid().ToString();

            var json = Serialize(target);

            var newTarget = Deserialize(json);

            Assert.IsNotNull(newTarget.Child);
            Assert.AreEqual(child.ID, newTarget.Child.ID);
            Assert.AreEqual(child.Name, newTarget.Child.Name);

        }

        [TestMethod]
        public void FatClientEdit_Deserialize_Child_ParentRef()
        {

            var child = target.Child = scope.Resolve<IEditObject>();

            child.ID = Guid.NewGuid();
            child.Name = Guid.NewGuid().ToString();
            child.Parent = target;

            var json = Serialize(target);

            // ITaskRespository and ILogger constructor parameters are injected by Autofac 
            var newTarget = Deserialize(json);

            Assert.IsNotNull(newTarget.Child);
            Assert.AreEqual(child.ID, newTarget.Child.ID);
            Assert.AreEqual(child.Name, newTarget.Child.Name);
            Assert.AreSame(newTarget.Child.Parent, newTarget);
             
        }

        [TestMethod]
        public void FatClientEdit_IsModified()
        {

            var json = Serialize(target);

            var newTarget = Deserialize(json);

            Assert.IsTrue(newTarget.IsModified);
            Assert.IsTrue(newTarget.IsSelfModified);

        }

        [TestMethod]
        public void FatClientEdit_IsModified_False()
        {

            target.MarkUnmodified();
            var json = Serialize(target);

            var newTarget = Deserialize(json);

            Assert.IsFalse(newTarget.IsModified);
            Assert.IsFalse(newTarget.IsSelfModified);

        }

        [TestMethod]
        public void FatClientEdit_IsNew()
        {

            target.MarkNew();
            var json = Serialize(target);

            var newTarget = Deserialize(json);

            Assert.IsTrue(newTarget.IsNew);

        }

        [TestMethod]
        public void FatClientEdit_IsNew_False()
        {

            target.MarkOld();

            var json = Serialize(target);

            var newTarget = Deserialize(json);

            Assert.IsFalse(newTarget.IsNew);

        }

        [TestMethod]
        public void FatClientEdit_IsChild()
        {

            target.MarkAsChild();

            var json = Serialize(target);

            var newTarget = Deserialize(json);

            Assert.IsTrue(newTarget.IsChild);

        }

        [TestMethod]
        public void FatClientEdit_IsChild_False()
        {

            var json = Serialize(target);

            var newTarget = Deserialize(json);

            Assert.IsFalse(newTarget.IsChild);

        }

        [TestMethod]
        public void FatClientEdit_ModifiedProperties()
        {

            var orig = target.ModifiedProperties.ToList();

            var json = Serialize(target);

            var newTarget = Deserialize(json);

            var result = newTarget.ModifiedProperties.ToList();

            CollectionAssert.AreEquivalent(orig, result);

        }
    }
}

