using OOBehave.Portal;
using Moq;
using System.Threading.Tasks;

namespace OOBehave.UnitTest
{
    public class MockSendReceivePortalChild<T> : ISendReceivePortalChild<T>
        where T : IPortalEditTarget, IEditMetaProperties
    {
        public MockSendReceivePortalChild()
        {
            MockPortal = new Mock<ISendReceivePortalChild<T>>(MockBehavior.Strict);
        }

        public Mock<ISendReceivePortalChild<T>> MockPortal { get; }

        public Task<T> CreateChild()
        {
            return MockPortal.Object.CreateChild();
        }

        public Task<T> CreateChild(params object[] criteria)
        {
            return MockPortal.Object.CreateChild(criteria);
        }

        public Task<T> FetchChild()
        {
            return MockPortal.Object.FetchChild();
        }

        public Task<T> FetchChild(params object[] criteria)
        {
            return MockPortal.Object.FetchChild(criteria);
        }

        public Task UpdateChild(T target, params object[] criteria)
        {
            return MockPortal.Object.UpdateChild(target, criteria);
        }

        public Task UpdateChild(T target)
        {
            return MockPortal.Object.UpdateChild(target);
        }
    }
}
