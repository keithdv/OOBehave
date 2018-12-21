using System;

namespace OOBehave
{
    public interface IServiceScope
    {
        T Resolve<T>();

        object Resolve(Type t);

        bool TryResolve<T>(out T result);

        bool TryResolve(Type T, out object result);

        bool IsRegistered<T>();

        bool IsRegistered(Type type);
    }

}