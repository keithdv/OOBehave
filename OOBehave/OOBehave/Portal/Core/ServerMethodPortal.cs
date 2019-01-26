using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.Portal.Core
{
    public class ServerMethodPortal<D> : IRemoteMethodPortal<D> where D : Delegate
    {

        public ServerMethodPortal(D method)
        {
            Method = method;
        }

        public D Method { get; }

        public async Task<T> Execute<T>(params object[] p)
        {
            var result = Method.Method.Invoke(Method.Target, p);

            if (result is Task<T> resultTask)
            {
                return await resultTask.ConfigureAwait(false);
            }
            else if (result is T resultT)
            {
                return resultT;
            }

            throw new Exception($"The return value {result.GetType()} is not {typeof(T).GetType()}.");
        }

    }
}
