using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using OOBehave.Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOBehave.Netwonsoft.Json.Test.BaseTests
{

    [TestClass]
    public class FatClientListBaseTests
    {
        IServiceScope scope;
        IBaseObjectList target;
        Guid Id = Guid.NewGuid();
        string Name = Guid.NewGuid().ToString();
        FatClientContractResolver resolver;
        IBaseObject child;

        [TestInitialize]
        public void TestInitailize()
        {
            scope = AutofacContainer.GetLifetimeScope().Resolve<IServiceScope>();
            target = scope.Resolve<IBaseObjectList>();
            target.ID = Id;
            target.Name = Name;
            resolver = scope.Resolve<FatClientContractResolver>();

            child = scope.Resolve<IBaseObject>();
            child.ID = Guid.NewGuid();
            child.Name = Guid.NewGuid().ToString();
            target.Add(child);
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

        private IBaseObjectList Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<IBaseObjectList>(json, new JsonSerializerSettings
            {
                ContractResolver = resolver,
                TypeNameHandling = TypeNameHandling.All,
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                Converters = new List<JsonConverter>() { scope.Resolve<ListBaseCollectionConverter>() }
            });
        }

        [TestMethod]
        public void FatClientListBaseTests_Serialize()
        {

            var result = Serialize(target);

            Assert.IsTrue(result.Contains(Id.ToString()));
            Assert.IsTrue(result.Contains(Name));
            Assert.IsTrue(result.Contains(child.ID.ToString()));
            Assert.IsTrue(result.Contains(child.Name));
        }

        [TestMethod]
        public void FatClientListBaseTests_Deserialize()
        {

            var json = Serialize(target);

            var newTarget = Deserialize(json);

            Assert.AreEqual(target.ID, newTarget.ID);
            Assert.AreEqual(target.Name, newTarget.Name);
        }

        [TestMethod]
        public void FatClientListBaseTests_Deserialize_Child()
        {

            var json = Serialize(target);

            // ITaskRespository and ILogger constructor parameters are injected by Autofac 
            var newTarget = Deserialize(json);


            Assert.IsNotNull(newTarget.SingleOrDefault());
            Assert.AreEqual(child.ID, newTarget.Single().ID);
            Assert.AreEqual(child.Name, newTarget.Single().Name);

        }

        [TestMethod]
        public void FatClientListBaseTests_Deserialize_Modify()
        {

            var json = Serialize(target);

            // ITaskRespository and ILogger constructor parameters are injected by Autofac 
            var newTarget = Deserialize(json);

            var newId = Guid.NewGuid();
            newTarget.ID = newId;
            Assert.AreEqual(newId, newTarget.ID);
        }

    }
}

