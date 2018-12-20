using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOBehave.UnitTest.Objects;
using System;

namespace OOBehave.UnitTest
{

    public class DomainObect
    {

        public DomainObect()
        {
            // This can be done using reflection

            RegisteredOperations.RegisterOperation<DomainObect>(Operation.UpdateChild, nameof(UpdateChildGuidCriteria));
            RegisteredOperations.RegisterOperation<DomainObect>(Operation.UpdateChild, nameof(UpdateChildIntCriteria));
            RegisteredOperations.RegisterOperation<DomainObect>(Operation.UpdateChild, nameof(UpdateChildOnlyDependency));
            RegisteredOperations.RegisterOperation<DomainObect>(Operation.UpdateChild, nameof(UpdateChildCriteriaDepedency));
        }

        private void UpdateChildGuidCriteria(Guid criteria)
        {


        }

        private void UpdateChildIntCriteria(int criteria)
        {


        }

        private void UpdateChildOnlyDependency(IDisposableDependency dependency)
        {

        }

        private void UpdateChildCriteriaDepedency(Decimal criteria, IDisposableDependency dependency)
        {

        }

    }

    [TestClass]
    public class ObjectPortalSandbox
    {
        [TestMethod]
        public void ObjectPortal_Do()
        {

            var scope = AutofacContainer.GetLifetimeScope();
            var portal = scope.Resolve<ObjectPortal>();
            var domainObj = new DomainObect();

           // If each objectportal method is generic
           // Instead of the ObjectPortal<T> object
           // The base class can have these methods for you!

            portal.UpdateChild(domainObj);
            portal.UpdateChild(domainObj, Guid.NewGuid());
            portal.UpdateChild(domainObj, 123);
            portal.UpdateChild(domainObj, 1234m);

        }
    }
}
