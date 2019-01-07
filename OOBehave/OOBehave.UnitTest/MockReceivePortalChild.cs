using OOBehave.Portal;
using Moq;
using System.Threading.Tasks;

namespace OOBehave.UnitTest
{
    public class MockReceivePortalChild<T> : IReceivePortalChild<T>
        where T : IPortalTarget
    {
        public MockReceivePortalChild()
        {
            MockPortal = new Mock<IReceivePortalChild<T>>(MockBehavior.Strict);
        }

        public Mock<IReceivePortalChild<T>> MockPortal { get; }

        public Task<T> CreateChild()
        {
            return MockPortal.Object.CreateChild();
        }

        public Task<T> CreateChild(object criteria)
        {
            return MockPortal.Object.CreateChild(criteria);
        }

        public Task<T> FetchChild()
        {
            return MockPortal.Object.FetchChild();
        }

        public Task<T> FetchChild(object criteria)
        {
            return MockPortal.Object.FetchChild(criteria);
        }

    }
}
