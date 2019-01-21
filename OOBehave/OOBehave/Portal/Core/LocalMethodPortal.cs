using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.Portal.Core
{
    public class LocalMethodPortal<D> : IRemoteMethodPortal<D> where D : Delegate
    {

        public LocalMethodPortal(IServiceScope scope)
        {
            Scope = scope;
        }

        public IServiceScope Scope { get; }

        public async Task<T> Execute<T>(params object[] p)
        {

            // Execute methods get their own scope
            using (var scope = Scope.BeginNewScope("DependencyScope"))
            {
                var method = (D)scope.Resolve(typeof(D));
                var result = method.Method.Invoke(method.Target, p);

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
}
