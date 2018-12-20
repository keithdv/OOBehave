using Autofac;
using OOBehave.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave.UnitTest
{
    public class ServiceScope : IServiceScope
    {
        private ILifetimeScope scope { get; }
        public ServiceScope(ILifetimeScope scope)
        {
            this.scope = scope;
        }

        public T Resolve<T>()
        {
            return scope.Resolve<T>();
        }

        public object Resolve(Type t)
        {
            return scope.Resolve(t);
        }

        public bool TryResolve<T>(out T result)
        {
            return scope.TryResolve<T>(out result);
        }

        public bool TryResolve(Type T, out object result)
        {
            return scope.TryResolve(T, out result);
        }

        public bool IsRegistered(Type type)
        {
            return scope.IsRegistered(type);
        }

        public bool IsRegistered<T>()
        {
            return scope.IsRegistered<T>();
        }
    }
}
