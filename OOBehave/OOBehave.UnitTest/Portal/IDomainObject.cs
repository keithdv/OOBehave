using System;

namespace OOBehave.UnitTest.ObjectPortal
{
    public interface IDomainObject : IBase<IDomainObject>
    {
        int IntCriteria { get; }
        Guid GuidCriteria { get; }
        bool CreateCalled { get; set; }
        bool CreateChildCalled { get; set; }
        bool DeleteCalled { get; set; }
        bool DeleteChildCalled { get; set; }
        bool UpdateCalled { get; set; }
        bool UpdateChildCalled { get; set; }
    }
}