using OOBehave.Portal;
using Moq;
using System.Threading.Tasks;

namespace OOBehave.UnitTest
{
    public class MockReceivePortal<T> : IReceivePortal<T>
        where T : IPortalTarget
    {
        public MockReceivePortal()
        {
            MockPortal = new Mock<IReceivePortal<T>>(MockBehavior.Strict);
        }

        public Mock<IReceivePortal<T>> MockPortal { get; }

        public Task<T> Create()
        {
            return MockPortal.Object.Create();
        }

        public Task<T> Create(params object[] criteria)
        {
            return MockPortal.Object.Create(criteria);
        }

        public Task<T> Fetch()
        {
            return MockPortal.Object.Fetch();
        }

        public Task<T> Fetch(params object[] criteria)
        {
            return MockPortal.Object.Fetch(criteria);
        }

    }
}
