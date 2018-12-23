using System;
using System.Collections.Generic;

namespace OOBehave
{

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Generic to ensure that types can only access their properties</typeparam>
    public interface IRegisteredPropertyManager<T>
    {
        IRegisteredProperty<P> RegisterProperty<P>(string name);
        IEnumerable<IRegisteredProperty> GetRegisteredProperties();
    }
}
