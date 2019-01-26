using OOBehave;
using System;

namespace _3Tier.Lib
{
    public interface IEditObject : IEditBase
    {
        Guid Id { get; set; }
        string Name { get; set; }
        int? Value { get; set; }
    }
}