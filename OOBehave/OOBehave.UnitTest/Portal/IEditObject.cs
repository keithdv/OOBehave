using System;

namespace OOBehave.UnitTest.ObjectPortal
{
    public interface IEditObject : IEditBase
    {
        string Name { get; set; }
        int IntCriteria { get; }
        Guid GuidCriteria { get; }
        bool CreateCalled { get; set; }
        bool CreateChildCalled { get; set; }
        bool FetchCalled { get; set; }
        bool FetchChildCalled { get; set; }
        bool DeleteCalled { get; set; }
        bool DeleteChildCalled { get; set; }
        bool UpdateCalled { get; set; }
        bool UpdateChildCalled { get; set; }
    }
}