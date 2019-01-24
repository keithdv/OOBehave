using OOBehave.AuthorizationRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OOBehave.Portal.Core
{
    public interface IPortalScope : IDisposable
    {
        IServiceScope DependencyScope { get; set; }
        IServiceScope TargetScope { get; set; }
        void BeginDependencyScope();
        uint UniqueId { get; }
    }

    public class PortalScope : IPortalScope
    {
        private static uint uniqueIdCount = 0;

        public PortalScope(IServiceScope scope)
        {
            if (scope.Tag.ToString() != "Target")
            {
                //throw new Exception(scope.Tag.ToString());
            }

            TargetScope = scope;
            UniqueId = uniqueIdCount++;

        }

        private IServiceScope dependencyScope;
        /// <summary>
        /// Used for Method Injected Dependencies
        /// Gets Disposed at end of Portal Operation
        /// </summary>
        public IServiceScope DependencyScope
        {
            get
            {
                if (dependencyScope == null)
                {
                    throw new Exception("BegineDependencyScope should be called before DependencyScope is used");
                }
                return dependencyScope;
            }set
            {
                dependencyScope = value;
            }
        }

        /// <summary>
        /// Used to resolve Target so that the Constructor Injection dependencies are not 
        /// disposed at the end of the Portal Operation
        /// </summary>
        public IServiceScope TargetScope { get; set; }
        public uint UniqueId { get; }

        public void BeginDependencyScope()
        {
            if(dependencyScope != null)
            {
                throw new Exception("PortalScope should be disposed before being used again.");
            }

            dependencyScope = TargetScope.BeginNewScope("DependencyScope");
            var ps = dependencyScope.Resolve<IPortalScope>();
            ps.TargetScope = TargetScope;
            ps.DependencyScope = dependencyScope; // Feels weird
        }
        public void Dispose()
        {

            // TODO : Make PortalScope not externally owned for safety or don't allow it to be used outside of OOBehave
            if (dependencyScope == null) { throw new Exception("PortalScope should not be disposed before being used in a using(PortalScope). For example in AutoFac use ExternallyOwned"); }
            dependencyScope.Dispose();
            dependencyScope = null;
        }

    }

    public class ObjectPortalBase
    {
        public ObjectPortalBase(IPortalScope scope, Type type)
        {
            PortalScope = scope;

            if (type.IsInterface)
            {
                // To find the static method this needs to be the concrete type
                ConcreteType = scope.TargetScope.ConcreteType(type) ?? throw new Exception($"Type {type.FullName} is not registered");
            }
            else
            {
                ConcreteType = type;
            }
        }

        /// <summary>
        /// Resolve the Target Objects - Never Dispose
        /// </summary>
        protected IPortalScope PortalScope { get; private set; }
        public Func<IPortalScope> CreatePortalScope { get; }
        public Type ConcreteType { get; }

        protected IPortalOperationManager OperationManager
        {
            get
            {
                return (IPortalOperationManager)PortalScope.DependencyScope.Resolve(typeof(IPortalOperationManager<>).MakeGenericType(ConcreteType));
            }
        }

        /// <summary>
        /// Dispose Portal Operation Scope at the right Moment
        /// Null except for the parent operation
        /// </summary>
        public IPortalScope PortalOperationScope()
        {
            // Difficult Situation
            // Do no want the constructor injected services to be disposed at the end of the portal methods
            // But we do want the method injected services to the portal methods to be disposed
            // So pass along the target scope that will to resolve new instances of Base<> objects

            // Difficult Situation - Using Constructor Injected IObjectPortal's in the Portal Method
            // Need to be aware of both


            PortalScope.BeginDependencyScope();
            return PortalScope;

            //if (alwaysNewScope)
            //{

            //    var scope = PortalScope.TargetScope.BeginNewScope("DependencyScope");
            //    var newPortalScope = scope.Resolve<IPortalScope>();

            //    // Scope used to create targets and do constructur injection cannot be disposed
            //    // Must be passed along
            //    newPortalScope.TargetScope = PortalScope.TargetScope;

            //    PortalScope.DependencyScope = scope;

            //    return newPortalScope;
            //}
        }
    }

    /// <summary>
    /// Provide Authorization Check
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectPortalBase<T> : ObjectPortalBase
        where T : IPortalTarget
    {


        public ObjectPortalBase(IPortalScope portalScope) : base(portalScope, typeof(T))
        {

        }

    }

}
