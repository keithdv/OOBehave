using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using OOBehave.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave.Netwonsoft.Json.Test.BaseTests
{

    [TestClass]
    public class WebClientBaseTests
    {
        IServiceScope scope;
        IBaseObject target;
        Guid Id = Guid.NewGuid();
        string Name = Guid.NewGuid().ToString();

        [TestInitialize]
        public void TestInitailize()
        {
            scope = AutofacContainer.GetLifetimeScope().Resolve<IServiceScope>();
            target = scope.Resolve<IBaseObject>();
            target.ID = Id;
            target.Name = Name;
        }

        private string Serialize(object target)
        {
            return JsonConvert.SerializeObject(target, new JsonSerializerSettings()
            {
                Formatting = Formatting.None
            });
        }

        // Web not meant to deserialize
        //private T Deserialize(string json)
        //{
        //    return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
        //    {
        //        ContractResolver = resolver,
        //        TypeNameHandling = TypeNameHandling.All,
        //        PreserveReferencesHandling = PreserveReferencesHandling.All
        //    });
        //}

        [TestMethod]
        public void WebClientTests_Serialize()
        {

            var result = Serialize(target);

            Assert.IsTrue(result.Contains(Id.ToString()));
            Assert.IsTrue(result.Contains(Name));
            Assert.IsFalse(result.Contains(nameof(PropertyValueManager<BaseObject>)));

        }

        [Ignore("Need to write a web converter")]
        [TestMethod]
        public void WebClientTests_Serialize_Child()
        {

            var child = target.Child = scope.Resolve<IBaseObject>();

            child.ID = Guid.NewGuid();
            child.Name = Guid.NewGuid().ToString();

            var json = Serialize(target);

            Assert.IsTrue(json.Contains(child.ID.ToString()));
            Assert.IsTrue(json.Contains(child.Name));


        }

    }
}

