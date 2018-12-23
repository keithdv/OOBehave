using OOBehave.AuthorizationRules;
using OOBehave.Core;
using OOBehave.Portal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace OOBehave
{

    public interface IBase : IOOBehaveObject
    {

    }

    public interface IBase<T> : IBase
    {

    }

    public abstract class Base<T> : IOOBehaveObject<T>, IBase<T>
        where T : Base<T>
    {

        protected IRegisteredPropertyDataManager<T> FieldDataManager { get; }


        public Base(IBaseServices<T> services)
        {
            FieldDataManager = services.RegisteredPropertyDataManager;
            RegisterPortalOperations(services.RegisteredOperationManager);
        }

        protected P ReadProperty<P>([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            return FieldDataManager.Read<P>(propertyName);
        }

        protected void LoadProperty<P>(P value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            FieldDataManager.Load(propertyName, value);
        }

        // TODO: This should be handled by ObjectPortal
        protected virtual void RegisterPortalOperations(IRegisteredOperationManager registeredOperationManager)
        {
            if (!registeredOperationManager.TypeRegistered<T>())
            {
                var methods = typeof(T).GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic)
                    .Where(m => m.GetCustomAttribute<OperationAttribute>() != null);

                foreach (var m in methods)
                {
                    var attribute = m.GetCustomAttribute<OperationAttribute>();
                    registeredOperationManager.RegisterOperation<T>(attribute.Operation, m);
                }
            }
        }

    }

}
