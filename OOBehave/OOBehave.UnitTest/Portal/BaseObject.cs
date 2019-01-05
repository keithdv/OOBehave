using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOBehave.Portal;
using OOBehave.UnitTest.Objects;
using System;

namespace OOBehave.UnitTest.ObjectPortal
{
    public class BaseObject : Base<BaseObject>, IBaseObject
    {

        public BaseObject(IBaseServices<BaseObject> baseServices) : base(baseServices)
        {
        }

        public Guid GuidCriteria { get; set; } = Guid.Empty;
        public int IntCriteria { get; set; } = -1;
        public (int, string) TupleCriteria { get; set; }

        public bool CreateCalled { get; set; } = false;

        [Create]
        private void Create()
        {
            CreateCalled = true;
        }

        [Create]
        private void Create(int criteria)
        {
            IntCriteria = criteria;
        }

        [Create]
        private void CreateTuple((int, string) tuple)
        {
            TupleCriteria = tuple;
        }


        [Create]
        private void Create(Guid criteria, IDisposableDependency dependency)
        {
            Assert.IsNotNull(dependency);
            GuidCriteria = criteria;
        }

        public bool CreateChildCalled { get; set; } = false;

        [CreateChild]
        private void CreateChild()
        {
            CreateChildCalled = true;
        }

        [CreateChild]
        private void CreateChild(int criteria)
        {
            IntCriteria = criteria;
        }

        [CreateChild]
        private void CreateChild(Guid criteria, IDisposableDependency dependency)
        {
            Assert.IsNotNull(dependency);
            GuidCriteria = criteria;
        }

        public bool FetchCalled { get; set; } = false;

        [Fetch]
        private void Fetch()
        {
            FetchCalled = true;
        }

        [Fetch]
        private void Fetch(int criteria)
        {
            IntCriteria = criteria;
        }

        [Fetch]
        private void Fetch(Guid criteria, IDisposableDependency dependency)
        {
            Assert.IsNotNull(dependency);
            GuidCriteria = criteria;
        }

        public bool FetchChildCalled { get; set; } = false;

        [FetchChild]
        private void FetchChild()
        {
            FetchChildCalled = true;
        }

        [FetchChild]
        private void FetchChild(int criteria)
        {
            IntCriteria = criteria;
        }

        [FetchChild]
        private void FetchChild(Guid criteria, IDisposableDependency dependency)
        {
            Assert.IsNotNull(dependency);
            GuidCriteria = criteria;
        }

    }
}
