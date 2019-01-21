using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave.UnitTest.Objects
{

    public class ConstructorDisposableDependencyList : List<ConstructorDisposableDependency> { }

    public interface IConstructorDisposableDependency : IDisposable
    {
        Guid UniqueId { get; }
        bool IsDisposed { get; set; }
    }

    public class ConstructorDisposableDependency : IConstructorDisposableDependency
    {
        public ConstructorDisposableDependency(IServiceScope scope, ConstructorDisposableDependencyList list)
        {
            if (scope.Tag.ToString() != "Target")
            {
                throw new Exception(scope.Tag.ToString());
            }
            list.Add(this);
        }

        public Guid UniqueId { get; } = Guid.NewGuid();
        public bool IsDisposed { get; set; } = false;

        public void Dispose()
        {
            if (IsDisposed) throw new Exception("Already Disposed!");
            IsDisposed = true;
        }
    }
}
