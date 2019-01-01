using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.Portal.Core
{
    public class LocalMethodPortal<D> : IRemoteMethodPortal<D> where D : Delegate
    {

        public LocalMethodPortal(D method)
        {
            Method = method;
        }

        public D Method { get; }

        public Task<T> Execute<T>(params object[] p)
        {
            return Task.FromResult((T)Method.Method.Invoke(Method.Target, p));
        }

    }
}
