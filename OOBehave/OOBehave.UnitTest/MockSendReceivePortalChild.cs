using OOBehave.Portal;
using Moq;
using System.Threading.Tasks;
using System;
using OOBehave.Portal.Core;

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
        public Task<T> FetchChild()
        {
            return MockPortal.Object.FetchChild();
        }
        public Task UpdateChild(T target)
        {
            return MockPortal.Object.UpdateChild(target);
        }

        public Task<T> CreateChild<C0>(C0 criteria0)
        {
            return MockPortal.Object.CreateChild(new object[] { criteria0 }, new Type[] { typeof(C0) });
        }
        public Task<T> CreateChild<C0, C1>(C0 criteria0, C1 criteria1)
        {
            return MockPortal.Object.CreateChild(new object[] { criteria0, criteria1 }, new Type[] { typeof(C0), typeof(C1) });
        }
        public Task<T> CreateChild<C0, C1, C2>(C0 criteria0, C1 criteria1, C2 criteria2)
        {
            return MockPortal.Object.CreateChild(new object[] { criteria0, criteria1, criteria2 }, new Type[] { typeof(C0), typeof(C1), typeof(C2) });
        }
        public Task<T> CreateChild<C0, C1, C2, C3>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3)
        {
            return MockPortal.Object.CreateChild(new object[] { criteria0, criteria1, criteria2, criteria3 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3) });
        }
        public Task<T> CreateChild<C0, C1, C2, C3, C4>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4)
        {
            return MockPortal.Object.CreateChild(new object[] { criteria0, criteria1, criteria2, criteria3, criteria4 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4) });
        }
        public Task<T> CreateChild<C0, C1, C2, C3, C4, C5>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5)
        {
            return MockPortal.Object.CreateChild(new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5) });
        }
        public Task<T> CreateChild<C0, C1, C2, C3, C4, C5, C6>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6)
        {
            return MockPortal.Object.CreateChild(new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6) });
        }
        public Task<T> CreateChild<C0, C1, C2, C3, C4, C5, C6, C7>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6, C7 criteria7)
        {
            return MockPortal.Object.CreateChild(new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6, criteria7 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6), typeof(C7) });
        }
        public Task<T> FetchChild<C0>(C0 criteria0)
        {
            return MockPortal.Object.FetchChild(new object[] { criteria0 }, new Type[] { typeof(C0) });
        }
        public Task<T> FetchChild<C0, C1>(C0 criteria0, C1 criteria1)
        {
            return MockPortal.Object.FetchChild(new object[] { criteria0, criteria1 }, new Type[] { typeof(C0), typeof(C1) });
        }
        public Task<T> FetchChild<C0, C1, C2>(C0 criteria0, C1 criteria1, C2 criteria2)
        {
            return MockPortal.Object.FetchChild(new object[] { criteria0, criteria1, criteria2 }, new Type[] { typeof(C0), typeof(C1), typeof(C2) });
        }
        public Task<T> FetchChild<C0, C1, C2, C3>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3)
        {
            return MockPortal.Object.FetchChild(new object[] { criteria0, criteria1, criteria2, criteria3 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3) });
        }
        public Task<T> FetchChild<C0, C1, C2, C3, C4>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4)
        {
            return MockPortal.Object.FetchChild(new object[] { criteria0, criteria1, criteria2, criteria3, criteria4 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4) });
        }
        public Task<T> FetchChild<C0, C1, C2, C3, C4, C5>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5)
        {
            return MockPortal.Object.FetchChild(new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5) });
        }
        public Task<T> FetchChild<C0, C1, C2, C3, C4, C5, C6>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6)
        {
            return MockPortal.Object.FetchChild(new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6) });
        }
        public Task<T> FetchChild<C0, C1, C2, C3, C4, C5, C6, C7>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6, C7 criteria7)
        {
            return MockPortal.Object.FetchChild(new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6, criteria7 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6), typeof(C7) });
        }

        public Task UpdateChild<C0>(T target, C0 criteria0)
        {
            return MockPortal.Object.UpdateChild(target, new object[] { criteria0 }, new Type[] { typeof(C0) });
        }

        public Task UpdateChild<C0, C1>(T target, C0 criteria0, C1 criteria1)
        {
            return MockPortal.Object.UpdateChild(target, new object[] { criteria0, criteria1 }, new Type[] { typeof(C0), typeof(C1) });
        }

        public Task UpdateChild<C0, C1, C2>(T target, C0 criteria0, C1 criteria1, C2 criteria2)
        {
            return MockPortal.Object.UpdateChild(target, new object[] { criteria0, criteria1, criteria2 }, new Type[] { typeof(C0), typeof(C1), typeof(C2) });
        }

        public Task UpdateChild<C0, C1, C2, C3>(T target, C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3)
        {
            return MockPortal.Object.UpdateChild(target, new object[] { criteria0, criteria1, criteria2, criteria3 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3) });
        }

        public Task UpdateChild<C0, C1, C2, C3, C4>(T target, C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4)
        {
            return MockPortal.Object.UpdateChild(target, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4) });
        }

        public Task UpdateChild<C0, C1, C2, C3, C4, C5>(T target, C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5)
        {
            return MockPortal.Object.UpdateChild(target, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5) });
        }

        public Task UpdateChild<C0, C1, C2, C3, C4, C5, C6>(T target, C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6)
        {
            return MockPortal.Object.UpdateChild(target, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6) });
        }

        public Task UpdateChild<C0, C1, C2, C3, C4, C5, C6, C7>(T target, C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6, C7 criteria7)
        {
            return MockPortal.Object.UpdateChild(target, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6, criteria7 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6), typeof(C7) });
        }

        public IServiceScope UsingOperationScope()
        {
            return null;
        }
    }
}
