using OOBehave.Portal;
using Moq;
using System.Threading.Tasks;

namespace OOBehave.UnitTest
{
    public class MockSendReceivePortal<T> : ISendReceivePortal<T>
        where T : IPortalEditTarget, IEditMetaProperties
    {
        public MockSendReceivePortal()
        {
            MockPortal = new Mock<ISendReceivePortal<T>>(MockBehavior.Strict);
        }

        public Mock<ISendReceivePortal<T>> MockPortal { get; }

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

        public Task Update(T target, params object[] criteria)
        {
            return MockPortal.Object.Update(target, criteria);
        }

        public Task Update(T target)
        {
            return MockPortal.Object.Update(target);
        }
    }
}
