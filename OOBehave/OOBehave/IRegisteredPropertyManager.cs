using System;
using System.Collections.Generic;

namespace OOBehave
{
    public interface IRegisteredPropertyManager
    {
        IRegisteredProperty<P> RegisterProperty<T, P>(string name);
        IEnumerable<IRegisteredProperty> GetRegisteredPropertiesForType(Type objectType);
    }
}
