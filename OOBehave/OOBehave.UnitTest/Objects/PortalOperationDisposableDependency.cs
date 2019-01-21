using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave.UnitTest.Objects
{

    public class PortalOperationDisposableDependencyList : List<PortalOperationDisposableDependency> { }

    public interface IPortalOperationDisposableDependency : IDisposable
    {
        Guid UniqueId { get; }
        bool IsDisposed { get; set; }
    }

    public class PortalOperationDisposableDependency : IPortalOperationDisposableDependency
    {
        public PortalOperationDisposableDependency(IServiceScope scope, PortalOperationDisposableDependencyList list)
        {
            if (scope.Tag.ToString() != "DependencyScope")
            {
                throw new Exception("Yes!");
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
