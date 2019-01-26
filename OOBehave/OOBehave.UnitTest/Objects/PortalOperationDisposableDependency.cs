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
            list.Add(this);
            ScopeId = scope.UniqueId;
        }

        public Guid UniqueId { get; } = Guid.NewGuid();
        public bool IsDisposed { get; set; } = false;
        public uint ScopeId { get; }

        public void Dispose()
        {
            if (IsDisposed) throw new Exception("Already Disposed!");
            IsDisposed = true;
        }
    }
}
