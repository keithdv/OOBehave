using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace OOBehave.Core
{

    public delegate IRegisteredProperty CreateRegisteredProperty(PropertyInfo property);

    internal static class RegisteredPropertyIndex
    {
        private static uint staticIndex = 1;
        internal static uint StaticIndex
        {
            get
            {
                staticIndex++;
                return staticIndex;
            }
        }
    }

    public class RegisteredProperty<T> : IRegisteredProperty, IRegisteredProperty<T>
    {

        public RegisteredProperty(PropertyInfo propertyInfo)
        {
            this.PropertyInfo = propertyInfo;
            this.Index = RegisteredPropertyIndex.StaticIndex;
#if DEBUG
            if (propertyInfo.PropertyType != typeof(T)) { throw new Exception($"{propertyInfo.PropertyType.FullName} is not {typeof(T).FullName}"); }
#endif
        }

        public PropertyInfo PropertyInfo { get; }

        public string Name => PropertyInfo.Name;

        public Type Type => PropertyInfo.PropertyType;
        public uint Index { get; }
        public string Key => Name;

    }
}
