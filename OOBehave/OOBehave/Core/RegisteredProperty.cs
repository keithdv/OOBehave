using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave.Core
{

    public class RegisteredProperty<T> : IRegisteredProperty, IRegisteredProperty<T>
    {
        private static uint index = 0;
        private static uint NextIndex() { index++; return index; } // This may be overly simple and in the wrong spot
        public RegisteredProperty(string name)
        {
            this.Name = name;
            this.Index = NextIndex();
        }

        public string Name { get; private set; }

        public Type Type { get { return typeof(T); } }
        public uint Index { get; }
        public string Key => Name;
    }
}
