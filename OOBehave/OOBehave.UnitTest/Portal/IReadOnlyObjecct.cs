using System;

namespace OOBehave.UnitTest.ObjectPortal
{
    public interface IReadOnlyObject : IBase<IReadOnlyObject>
    {
        int IntCriteria { get; }
        Guid GuidCriteria { get; }
        bool CreateCalled { get; set; }
        bool CreateChildCalled { get; set; }
        bool FetchCalled { get; set; }
        bool FetchChildCalled { get; set; }

    }
}