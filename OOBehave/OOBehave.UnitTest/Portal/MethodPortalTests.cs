using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave.UnitTest.Portal
{
    
    public class IMethodPortal<S> where S : Delegate
    {


    }

    public class MethodPortal<S> where S : Delegate
    {

        public MethodPortal(S method)
        {
            Method = method;
        }

        public S Method { get; }

        public T Execute<P, T>(P param)
        {
            return (T)Method.Method.Invoke(Method.Target, new object[1] { param });
        }
    }

    [TestClass]
    public class MethodPortalTests
    {
        private IContainer container;

        public delegate bool RemoteMethod(int number);

        public static bool RemoteMethod_(int number)
        {
            return number == 10;
        }


        [TestInitialize]
        public void TestInitialize()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.Register<RemoteMethod>(cc =>
            {
                return i => RemoteMethod_(i);
            });
            containerBuilder.RegisterGeneric(typeof(MethodPortal<>));
            container = containerBuilder.Build();
        }

        [TestMethod]
        public void MethodPortalTest()
        {
            var mp = container.Resolve<MethodPortal<RemoteMethod>>();
            var result = mp.Execute<int, bool>(10);
        }
    }
}

