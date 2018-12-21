using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave.Core
{

    public class RegisteredProperty<T> : IRegisteredProperty, IRegisteredProperty<T>
    {

        public RegisteredProperty(string name, uint index)
        {
            this.Name = name;
            this.Index = index;
        }

        public string Name { get; private set; }

        public Type Type { get { return typeof(T); } }
        public uint Index { get; }
        public string Key => Name;
    }
}
