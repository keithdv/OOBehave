using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOBehave.Newtonsoft.Json;
using OOBehave.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOBehave.Netwonsoft.Json.Test.EditTests
{
    [TestClass]
    public class EditPopulateTests
    {
        IServiceScope scope;
        IEditObject target;
        Guid Id = Guid.NewGuid();
        string Name = Guid.NewGuid().ToString();
        private ISerializer serializer;

        [TestInitialize]
        public void TestInitailize()
        {
            scope = AutofacContainer.GetLifetimeScope().Resolve<IServiceScope>();
            target = scope.Resolve<IEditObject>();
            target.ID = Id;
            target.Name = Name;
            serializer = scope.Resolve<ISerializer>();
        }

        private string Serialize(object target)
        {
            return serializer.Serialize(target);
        }

        private IEditObject Deserialize(string json)
        {
            return serializer.Deserialize<IEditObject>(json);
        }

        private void SimulateServerSave(Action<IEditObject> modify = null)
        {
            var json = Serialize(target);
            var newTarget = Deserialize(json);
            modify?.Invoke(newTarget);
            json = Serialize(newTarget);
            serializer.Populate(json, target);
        }
        [TestMethod]
        public void EditPopulate_Deserialize_Populate()
        {
            Guid id = Guid.NewGuid();

            SimulateServerSave(e => e.ID = id); ;

            Assert.AreEqual(id, target.ID);
        }


        [TestMethod]
        public void EditPopulate_Deserialize_Modify()
        {
            SimulateServerSave();

            var id = Guid.NewGuid();
            target.ID = id;
            Assert.AreEqual(id, target.ID);

        }

        [TestMethod]
        public void EditPopulate_Deserialize_Child()
        {

            var child = target.Child = scope.Resolve<IEditObject>();

            child.ID = Guid.NewGuid();
            child.Name = Guid.NewGuid().ToString();

            SimulateServerSave();

            Assert.IsNotNull(target.Child);
            Assert.AreEqual(child.ID, target.Child.ID);
            Assert.AreEqual(child.Name, target.Child.Name);

        }

        [TestMethod]
        public void EditPopulate_Deserialize_Child_ParentRef()
        {

            var child = target.Child = scope.Resolve<IEditObject>();

            child.ID = Guid.NewGuid();
            child.Name = Guid.NewGuid().ToString();

            SimulateServerSave();

            Assert.IsNotNull(target.Child);
            Assert.AreEqual(child.ID, target.Child.ID);
            Assert.AreEqual(child.Name, target.Child.Name);
            Assert.AreSame(target.Child.Parent, target);

        }

        [TestMethod]
        public void EditPopulate_IsModified()
        {
            target.MarkUnmodified();
            SimulateServerSave(e => e.ID = Guid.NewGuid());
            Assert.IsTrue(target.IsModified);
            Assert.IsTrue(target.IsSelfModified);

        }

        [TestMethod]
        public void EditPopulate_IsModified_False()
        {
            Assert.IsTrue(target.IsModified);
            Assert.IsTrue(target.IsSelfModified);

            SimulateServerSave(e => e.MarkUnmodified());

            Assert.IsFalse(target.IsModified);
            Assert.IsFalse(target.IsSelfModified);

        }

        [TestMethod]
        public void EditPopulate_IsNew()
        {
            target.MarkOld();
            SimulateServerSave(e => e.MarkNew());
            Assert.IsTrue(target.IsNew);
        }

        [TestMethod]
        public void EditPopulate_IsNew_False()
        {
            target.MarkNew();
            SimulateServerSave(e => e.MarkOld());
            Assert.IsFalse(target.IsNew);
        }

        [TestMethod]
        public void EditPopulate_IsChild()
        {
            SimulateServerSave(e => e.MarkAsChild());
            Assert.IsTrue(target.IsChild);
        }

        [TestMethod]
        public void EditPopulate_IsChild_False()
        {
            var json = Serialize(target);
            var newTarget = Deserialize(json);
            Assert.IsFalse(newTarget.IsChild);
        }

        [TestMethod]
        public void EditPopulate_ModifiedProperties()
        {
            var orig = target.ModifiedProperties.ToList();
            SimulateServerSave();
            var result = target.ModifiedProperties.ToList();
            CollectionAssert.AreEquivalent(orig, result);
        }

        [TestMethod]
        public void EditPopulate_PropertyIsModified()
        {
            target.MarkOld();
            target.MarkUnmodified();
            Assert.IsFalse(target.PropertyIsModified[nameof(IEditObject.Name)]);

            target.Name = $"{target.Name}0";

            Assert.IsTrue(target.PropertyIsModified[nameof(IEditObject.Name)]);

            SimulateServerSave();

            Assert.IsTrue(target.PropertyIsModified[nameof(IEditObject.Name)]);


        }
    }
}
