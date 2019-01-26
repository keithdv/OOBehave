using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOBehave.Portal;
using OOBehave.UnitTest.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.ObjectPortal
{

    [TestClass]
    public class ReceivePortalTests
    {
        private ILifetimeScope scope = AutofacContainer.GetLifetimeScope(Autofac.Portal.UnitTest);
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
            var cddl = scope.Resolve<ConstructorDisposableDependencyList>();
            var pddl = scope.Resolve<PortalOperationDisposableDependencyList>();
            scope.Dispose();
            Assert.IsFalse(cddl.Any(i => !i.IsDisposed));
            Assert.IsFalse(pddl.Any(i => !i.IsDisposed));
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
            domainObject = await portal.Create(1, 10d);
            CollectionAssert.AreEquivalent(new object[] { 1, 10d }, domainObject.MultipleCriteria.Take(2).ToList());


        }

        [TestMethod]
        public async Task ReceivePortal_DependencyDisposed()
        {
            // This was hard to get right and should use some more testing
            domainObject = await portal.Create(1, 10d);
            var disposableDependency = domainObject.MultipleCriteria.Last() as IPortalOperationDisposableDependency;
            Assert.IsNotNull(disposableDependency);
            // Portal Operation Method Injected dependencies should be disposed at the end of the operation
            Assert.IsTrue(disposableDependency.IsDisposed);
        }

        [TestMethod]
        public async Task ReceivePortal_CreateMultipleCriteria_NullIncluded()
        {
            // Null created the need for generic criteria method
            // If a null criteria value is sent no longer have information
            // to know what method to connect up to

            var param1 = new List<int> { 10, 20 };
            List<int> param2 = null;

            domainObject = await portal.Create(param1, param2);
            CollectionAssert.AreEquivalent(new object[] { param1, param2 }, domainObject.MultipleCriteria);
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
