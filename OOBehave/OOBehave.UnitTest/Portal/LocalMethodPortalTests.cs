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
        private IRemoteMethodPortal<Execute> Method { get; }

        public MethodObject(IRemoteMethodPortal<Execute> method)
        {
            Method = method;
        }

        // This entire approach is driven off of the fact that while the Delegate is not serializable
        // The Type definition of the Delegate is and we can resolve that Type from the container
        public delegate Task<int> Execute(int number);

        /// <summary>
        /// This will be called on the server (when not a unit test)
        /// </summary>
        /// <param name="number"></param>
        /// <param name="dependency"></param>
        /// <returns></returns>
        internal static Task<int> ExecuteServer(int number, IPortalOperationDisposableDependency dependency)
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
        public async Task LocalMethodPortal_ExecuteServer()
        {
            // Hide the fact that there is a remote call from the client
            var methodObject = scope.Resolve<IMethodObject>();

            var result = await methodObject.DoRemoteWork(20);

            Assert.AreEqual(200, result);
        }

        [TestMethod]
        public async Task LocalMethodPortal_Execute()
        {
            var portal = scope.Resolve<LocalMethodPortal<MethodObject.Execute>>();

            var result = await portal.Execute<int>(10);

            Assert.AreEqual(100, result);
        }



    }
}
