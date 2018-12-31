using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        private T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
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

            var newTarget = Deserialize<IEditObject>(json);

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

            var newTarget = Deserialize<IEditObject>(json);

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
            var newTarget = Deserialize<IEditObject>(json);

            Assert.IsNotNull(newTarget.Child);
            Assert.AreEqual(child.ID, newTarget.Child.ID);
            Assert.AreEqual(child.Name, newTarget.Child.Name);
            Assert.AreSame(newTarget.Child.Parent, newTarget);
             
        }

    }
}

