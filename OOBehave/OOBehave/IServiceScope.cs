using System;

namespace OOBehave
{
    public interface IServiceScope : IDisposable
    {
        IServiceScope BeginNewScope(object tag = null);
        T Resolve<T>();

        object Resolve(Type t);

        bool TryResolve<T>(out T result);

        bool TryResolve(Type T, out object result);

        Type ConcreteType(Type T);
        Type ConcreteType<T>();

        bool IsRegistered<T>();

        bool IsRegistered(Type type);
    }

}