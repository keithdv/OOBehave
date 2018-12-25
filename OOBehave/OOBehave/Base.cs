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

        protected IPropertyValueManager<T> PropertyValueManager { get; }


        public Base(IBaseServices<T> services)
        {
            PropertyValueManager = services.PropertyValueManager;
        }

        protected virtual P ReadProperty<P>([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            return PropertyValueManager.Read<P>(propertyName);
        }

        protected virtual void LoadProperty<P>(P value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            PropertyValueManager.Load(propertyName, value);
        }

    }

}
