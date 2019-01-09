using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOBehave.Portal;
using OOBehave.UnitTest.Objects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.ObjectPortal
{

    [TestClass]
    public class ReceivePortalTests
    {
        private ILifetimeScope scope = AutofacContainer.GetLifetimeScope(true);
        private IReceivePortal<IBaseObject> portal;
        private IBaseObject domainObject;

        [TestInitialize]
        public void TestInitialize()
        {
            portal = scope.Resolve<IReceivePortal<IBaseObject>>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            scope.Dispose();
        }

        [TestMethod]
        public async Task ReceivePortal_Create()
        {
            domainObject = await portal.Create();
            Assert.IsTrue(domainObject.CreateCalled);
        }

        [TestMethod]
        public async Task ReceivePortal_CreateGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            domainObject = await portal.Create(crit);
            Assert.AreEqual(crit, domainObject.GuidCriteria);
        }

        [TestMethod]
        public async Task ReceivePortal_CreateIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            domainObject = await portal.Create(crit);
            Assert.AreEqual(crit, domainObject.IntCriteria);
        }

        [TestMethod]
        public async Task ReceivePortal_CreateInferredCriteriaCalled()
        {
            var crit = new List<int>() { 0, 1, 2 };
            domainObject = await portal.Create(crit);
            Assert.IsTrue(domainObject.CreateCalled);
        }


        [TestMethod]
        public async Task ReceivePortal_CreateMultipleCriteriaCalled()
        {
            domainObject = await portal.Create(10, "String");
            CollectionAssert.AreEquivalent(new object[] { 10, "String" }, domainObject.MultipleCriteria);
        }

        [TestMethod]
        public void ReceivePortal_CreateMultipleCriteria_Missing_Fail()
        {
            // No such method exists

            Assert.ThrowsException<AggregateException>(() => domainObject = portal.Create(Guid.NewGuid(), "String").Result);

        }

        [TestMethod]
        public void ReceivePortal_CreateMultipleCriteria_Duplicate_Fail()
        {
            // Two possibilities exist due to one with a dependency and one without
            Assert.ThrowsException<AggregateException>(() => domainObject = portal.Create(1u).Result);

        }

        [TestMethod]
        public async Task ReceivePortal_CreateMultipleCriteriaCalled_Double()
        {
            var r = scope.IsRegistered(typeof(int));

            domainObject = await portal.Create(1, 10d);
            CollectionAssert.AreEquivalent(new object[] { 1, 10d }, domainObject.MultipleCriteria);
        }

        [TestMethod]
        public async Task ReceivePortal_Fetch()
        {
            domainObject = await portal.Fetch();
            Assert.IsTrue(domainObject.FetchCalled);
        }

        [TestMethod]
        public async Task ReceivePortal_FetchGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            domainObject = await portal.Fetch(crit);
            Assert.AreEqual(crit, domainObject.GuidCriteria);
        }

        [TestMethod]
        public async Task ReceivePortal_FetchIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            domainObject = await portal.Fetch(crit);
            Assert.AreEqual(crit, domainObject.IntCriteria);
        }

    }
}
