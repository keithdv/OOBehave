using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOBehave.Portal;
using OOBehave.Portal.Core;
using OOBehave.UnitTest.Objects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.Portal
{
    public interface IMethodObject
    {
        Task<int> DoRemoteWork(int number);
    }

    public class MethodObject : IMethodObject
    {
        private IRemoteMethodPortal<CommandMethod> Method { get; }

        public MethodObject(IRemoteMethodPortal<CommandMethod> method)
        {
            Method = method;
        }

        public delegate Task<int> CommandMethod(int number);

        /// <summary>
        /// This will be called on the servicer (when not a unit test)
        /// </summary>
        /// <param name="number"></param>
        /// <param name="dependency"></param>
        /// <returns></returns>
        internal static Task<int> CommandMethod_(int number, IDisposableDependency dependency)
        {
            Assert.IsNotNull(dependency);
            return Task.FromResult(number * 10);
        }

        public Task<int> DoRemoteWork(int number)
        {
            return Method.Execute<int>(number);
        }
    }

    [TestClass]
    public class LocalMethodPortalTests
    {
        private ILifetimeScope scope;

        [TestInitialize]
        public void TestInitialize()
        {
            scope = AutofacContainer.GetLifetimeScope(true);
        }

        [TestMethod]
        public async Task LocalMethodPortal_Execute()
        {
            var portal = scope.Resolve<LocalMethodPortal<MethodObject.CommandMethod>>();

            var result = await portal.Execute<int>(10);

            Assert.AreEqual(100, result);
        }

        [TestMethod]
        public async Task LocalMethodPortal_MethodObject()
        {
            var methodObject = scope.Resolve<IMethodObject>();

            var result = await methodObject.DoRemoteWork(20);

            Assert.AreEqual(200, result);
        }

    }
}
