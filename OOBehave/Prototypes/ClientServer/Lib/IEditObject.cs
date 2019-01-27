using OOBehave;
using System;

namespace Lib
{
    public interface IEditObject : IEditBase
    {
        IEditObject Child { get; set; }
        Guid Id { get; set; }
        string Name { get; set; }
        int? Value { get; set; }
    }
}