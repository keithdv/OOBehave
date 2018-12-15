using System;

namespace OOBehave
{
    public interface IRegisteredProperty
    {
        string Name { get; }
        Type Type { get; }
        string Key { get; }
        uint Index { get; }
    }

    public interface IRegisteredProperty<T> : IRegisteredProperty
    {

    }
}