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

        // Static so this can't be mixed with DependencyInjection
        public static IRegisteredPropertyManager RegisteredPropertyManager
        {
            get
            {
                return Core.Factory.StaticFactory.RegisteredPropertyManager;
            }
        }

        protected static IRegisteredProperty<P> RegisterProperty<P>(string name)
        {
            return RegisteredPropertyManager.RegisterProperty<T, P>(name);
        }

        protected P ReadProperty<P>(IRegisteredProperty<P> property)
        {
            return FieldDataManager.Read(property);
        }

        protected void LoadProperty<P>(IRegisteredProperty<P> property, P value)
        {
            FieldDataManager.Load(property, value);
        }

    }

}
