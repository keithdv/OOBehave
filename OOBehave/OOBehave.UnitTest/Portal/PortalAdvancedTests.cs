using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOBehave.Portal;
using OOBehave.Portal.Core;
using OOBehave.UnitTest.Objects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.ObjectPortal
{

    public interface IAdvancedObject : IValidateBase
    {
        IServiceScope MethodPortalScope { get; }
        Task<IAdvancedObject> FetchChild();
        IPortalScope ConstPortalScope { get; }
        IAdvancedObject Child { get; }
        IPortalOperationDisposableDependency PortalOperationDisposableDependency { get; }
        IConstructorDisposableDependency ConstructorDisposableDependency { get; }
    }

    public class AdvancedObject : ValidateBase<AdvancedObject>, IAdvancedObject
    {
        public AdvancedObject(IValidateBaseServices<AdvancedObject> services, IReceivePortalChild<IAdvancedObject> constPortal, IPortalScope constPs, IConstructorDisposableDependency constructorDisposableDependency) : base(services)
        {
            ConstPortal = constPortal;
            ConstPortalScope = constPs;
            ConstructorDisposableDependency = constructorDisposableDependency;
        }

        public IReceivePortalChild<IAdvancedObject> ConstPortal { get; }
        public IPortalScope ConstPortalScope { get; }
        public IConstructorDisposableDependency ConstructorDisposableDependency { get; }
        public IServiceScope MethodPortalScope { get; private set; }

        public IAdvancedObject Child { get; private set; }

        public IPortalOperationDisposableDependency PortalOperationDisposableDependency { get; set; }
        [Fetch]
        public async Task Fetch(IPortalScope methodPs, IPortalOperationDisposableDependency portalOperationDisposableDependency)
        {
            MethodPortalScope = methodPs.DependencyScope;
            // Need MethodPortal and ConstPortal to have the same DependencyScope
            Assert.AreEqual(methodPs.DependencyScope.UniqueId, ConstPortalScope.DependencyScope.UniqueId);
            PortalOperationDisposableDependency = portalOperationDisposableDependency;

            // This is how Lists work
            Child = await ConstPortal.FetchChild();
        }

        [FetchChild]
        public async Task FetchChild(IPortalScope methodPs, IPortalOperationDisposableDependency portalOperationDisposableDependency)
        {
            MethodPortalScope = methodPs.DependencyScope;
            Assert.AreEqual(methodPs.DependencyScope.UniqueId, ConstPortalScope.DependencyScope.UniqueId);
            PortalOperationDisposableDependency = portalOperationDisposableDependency;

            await Task.Delay(10);
        }

        public async Task<IAdvancedObject> FetchChild()
        {
            // using(PortalOpertaionScope) required for child operations outside of a non-child operation
            // This is required because "no sope create one" creates hidden issues
            // More scope than you mean to create get created by simple mistakes
            // ViewModels and services should always use the non-child operation anyways
            using (ConstPortal.PortalOperationScope())
            {
                return await ConstPortal.FetchChild();
            }
        }
    }

    [TestClass]
    public class PortalAdvancedTests
    {
        [TestMethod]
        public async Task PortalAdvanced_Child()
        {
            var scope = AutofacContainer.GetLifetimeScope(true).Resolve<IServiceScope>();
            var portal = scope.Resolve<IReceivePortal<IAdvancedObject>>();
            var obj = await portal.Fetch();

            // TargetScope should be the scope that was used at the top operation
            Assert.AreEqual(scope.UniqueId, obj.ConstPortalScope.TargetScope.UniqueId);
            Assert.AreEqual(scope.UniqueId, obj.Child.ConstPortalScope.TargetScope.UniqueId);

            Assert.IsFalse(obj.ConstPortalScope.TargetScope.IsDisposed);
            Assert.IsFalse(obj.ConstPortalScope.TargetScope.IsDisposed);

            Assert.IsFalse(obj.ConstructorDisposableDependency.IsDisposed);
            Assert.IsFalse(obj.Child.ConstructorDisposableDependency.IsDisposed);

            Assert.IsTrue(obj.MethodPortalScope.IsDisposed);
            Assert.IsTrue(obj.Child.MethodPortalScope.IsDisposed);

            Assert.IsTrue(obj.PortalOperationDisposableDependency.IsDisposed);
            Assert.IsTrue(obj.Child.PortalOperationDisposableDependency.IsDisposed);

        }

        [TestMethod]
        public async Task PortalAdvanced_2Tier_Parrallel()
        {
            var scope = AutofacContainer.GetLifetimeScope(true);

            var portal = scope.Resolve<IReceivePortal<IAdvancedObject>>();

            Task<IAdvancedObject>[] tasks = new Task<IAdvancedObject>[4];

            tasks[0] = portal.Fetch();
            tasks[1] = portal.Fetch();
            tasks[2] = portal.Fetch();
            tasks[3] = portal.Fetch();

            await Task.WhenAll(tasks);

            foreach (var t in tasks)
            {
                Assert.IsTrue(t.Result.MethodPortalScope.IsDisposed);
            }
        }

        [TestMethod]
        public async Task PortalAdvanced_FetchChild()
        {
            var scope = AutofacContainer.GetLifetimeScope(true);
            var portal = scope.Resolve<IReceivePortal<IAdvancedObject>>();
            var obj = await portal.Fetch();

            var childObj = await obj.FetchChild();
            Assert.IsTrue(obj.MethodPortalScope.IsDisposed);
            Assert.IsTrue(childObj.MethodPortalScope.IsDisposed);
            Assert.AreNotEqual(obj.MethodPortalScope.UniqueId, childObj.MethodPortalScope.UniqueId);
        }

    }
}
