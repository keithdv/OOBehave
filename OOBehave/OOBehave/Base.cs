using OOBehave.Core;
using System;
using System.ComponentModel;

namespace OOBehave
{

    public interface IBase
    {

    }

    public interface IBase<T> : IBase
    {

    }

    public abstract class Base<T> : IOOBehaveObject<T>, IBase<T>
        where T : Base<T>
    {

        protected readonly IRegisteredPropertyDataManager<T> FieldDataManager;


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
