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
        }

        protected P ReadProperty<P>([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            return FieldDataManager.Read<P>(propertyName);
        }

        protected void LoadProperty<P>(P value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            FieldDataManager.Load(propertyName, value);
        }



    }

}
