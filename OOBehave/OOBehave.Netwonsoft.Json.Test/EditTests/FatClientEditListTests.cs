using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using OOBehave.Newtonsoft.Json;
using OOBehave.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOBehave.Netwonsoft.Json.Test.EditTests
{

    [TestClass]
    public class FatClientEditListTests
    {
        IServiceScope scope;
        IEditObjectList target;
        Guid Id = Guid.NewGuid();
        string Name = Guid.NewGuid().ToString();
        private INewtonsoftJsonSerializer serializer;

        [TestInitialize]
        public void TestInitailize()
        {
            scope = AutofacContainer.GetLifetimeScope().Resolve<IServiceScope>();
            target = scope.Resolve<IEditObjectList>();
            target.ID = Id;
            target.Name = Name;
            serializer = scope.Resolve<INewtonsoftJsonSerializer>();

        }

        private string Serialize(object target)
        {
            return serializer.Serialize(target);
        }

        private IEditObjectList Deserialize(string json)
        {
            return serializer.Deserialize<IEditObjectList>(json);
        }

        [TestMethod]
        public void FatClientEditList_Serialize()
        {

            var result = Serialize(target);

            Assert.IsTrue(result.Contains(Id.ToString()));
            Assert.IsTrue(result.Contains(Name));
        }

        [TestMethod]
        public void FatClientEditList_Deserialize()
        {

            var json = Serialize(target);

            var newTarget = Deserialize(json);

            Assert.AreEqual(target.ID, newTarget.ID);
            Assert.AreEqual(target.Name, newTarget.Name);
        }

        //[TestMethod]
        //public void FatClientEditList_Deserialize_Child()
        //{

        //    var child = target.Child = scope.Resolve<IEditObject>();

        //    child.ID = Guid.NewGuid();
        //    child.Name = Guid.NewGuid().ToString();

        //    var json = Serialize(target);

        //    var newTarget = Deserialize(json);

        //    Assert.IsNotNull(newTarget.Child);
        //    Assert.AreEqual(child.ID, newTarget.Child.ID);
        //    Assert.AreEqual(child.Name, newTarget.Child.Name);

        //}

        //[TestMethod]
        //public void FatClientEditList_Deserialize_Child_ParentRef()
        //{

        //    var child = target.Child = scope.Resolve<IEditObject>();

        //    child.ID = Guid.NewGuid();
        //    child.Name = Guid.NewGuid().ToString();
        //    child.Parent = target;

        //    var json = Serialize(target);

        //    // ITaskRespository and ILogger constructor parameters are injected by Autofac 
        //    var newTarget = Deserialize(json);

        //    Assert.IsNotNull(newTarget.Child);
        //    Assert.AreEqual(child.ID, newTarget.Child.ID);
        //    Assert.AreEqual(child.Name, newTarget.Child.Name);
        //    Assert.AreSame(newTarget.Child.Parent, newTarget);
             
        //}

        [TestMethod]
        public void FatClientEditList_IsModified()
        {

            var json = Serialize(target);

            var newTarget = Deserialize(json);

            Assert.IsTrue(newTarget.IsModified);
            Assert.IsTrue(newTarget.IsSelfModified);

        }

        [TestMethod]
        public void FatClientEditList_IsModified_False()
        {

            target.MarkUnmodified();
            var json = Serialize(target);

            var newTarget = Deserialize(json);

            Assert.IsFalse(newTarget.IsModified);
            Assert.IsFalse(newTarget.IsSelfModified);

        }

        [TestMethod]
        public void FatClientEditList_IsNew()
        {

            target.MarkNew();
            var json = Serialize(target);

            var newTarget = Deserialize(json);

            Assert.IsTrue(newTarget.IsNew);

        }

        [TestMethod]
        public void FatClientEditList_IsNew_False()
        {

            target.MarkOld();

            var json = Serialize(target);

            var newTarget = Deserialize(json);

            Assert.IsFalse(newTarget.IsNew);

        }

        [TestMethod]
        public void FatClientEditList_IsChild()
        {

            target.MarkAsChild();

            var json = Serialize(target);

            var newTarget = Deserialize(json);

            Assert.IsTrue(newTarget.IsChild);

        }

        [TestMethod]
        public void FatClientEditList_IsChild_False()
        {

            var json = Serialize(target);

            var newTarget = Deserialize(json);

            Assert.IsFalse(newTarget.IsChild);

        }

        [TestMethod]
        public void FatClientEditList_ModifiedProperties()
        {

            var orig = target.ModifiedProperties.ToList();

            var json = Serialize(target);

            var newTarget = Deserialize(json);

            var result = newTarget.ModifiedProperties.ToList();

            CollectionAssert.AreEquivalent(orig, result);

        }

        [TestMethod]
        public void FatClientEditList_IsDeleted()
        {
            target.Delete();

            var json = Serialize(target);

            var newTarget = Deserialize(json);

            Assert.IsTrue(target.IsDeleted);
            Assert.IsTrue(target.IsModified);
            Assert.IsTrue(target.IsSelfModified);
            Assert.IsTrue(newTarget.IsDeleted);
            Assert.IsTrue(newTarget.IsModified);
            Assert.IsTrue(newTarget.IsSelfModified);
        }
    }
}

