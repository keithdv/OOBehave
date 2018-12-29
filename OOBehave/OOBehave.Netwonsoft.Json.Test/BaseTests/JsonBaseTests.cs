﻿using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave.Netwonsoft.Json.Test.BaseTests
{

    [TestClass]
    public class JsonBaseTests
    {
        IServiceScope scope;
        IBaseObject target;
        Guid Id = Guid.NewGuid();
        string Name = Guid.NewGuid().ToString();
        AutofacContractResolver resolver;

        [TestInitialize]
        public void TestInitailize()
        {
            scope = AutofacContainer.GetLifetimeScope().Resolve<IServiceScope>();
            target = scope.Resolve<IBaseObject>();
            target.ID = Id;
            target.Name = Name;
            resolver = scope.Resolve<AutofacContractResolver>();
        }

        [TestMethod]
        public void JsonBaseTests_Serialize()
        {

            var result = JsonConvert.SerializeObject(target);

            Assert.IsTrue(result.Contains(Id.ToString()));
            Assert.IsTrue(result.Contains(Name));
        }

        [TestMethod]
        public void JsonBaseTests_Deserialize()
        {

            var json = JsonConvert.SerializeObject(target);

            // ITaskRespository and ILogger constructor parameters are injected by Autofac 
            var newTarget = JsonConvert.DeserializeObject<IBaseObject>(json, new JsonSerializerSettings
            {
                ContractResolver = resolver
            });

            Assert.AreEqual(target.ID, newTarget.ID);
            Assert.AreEqual(target.Name, newTarget.Name);
        }

        [TestMethod]
        public void JsonBaseTests_Deserialize_Child()
        {

            var child = target.Child = scope.Resolve<IBaseObject>();

            child.ID = Guid.NewGuid();
            child.Name = Guid.NewGuid().ToString();

            var json = JsonConvert.SerializeObject(target);

            // ITaskRespository and ILogger constructor parameters are injected by Autofac 
            var newTarget = JsonConvert.DeserializeObject<IBaseObject>(json, new JsonSerializerSettings
            {
                ContractResolver = resolver
            });


            Assert.IsNotNull(newTarget.Child);
            Assert.AreEqual(child.ID, newTarget.Child.ID);
            Assert.AreEqual(child.Name, newTarget.Child.Name);

        }

    }
}

