using System;
using System.Collections.Generic;

namespace OOBehave
{
    public interface IRegisteredPropertyManager
    {
        IRegisteredProperty<P> RegisterProperty<T, P>(string name);
        void RegisterProperty(Type objectType, IRegisteredProperty metaProperty);
        IReadOnlyList<IRegisteredProperty> GetRegisteredPropertiesForType(Type objectType);
    }
}
