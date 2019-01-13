using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOBehave.Portal;
using OOBehave.UnitTest.Objects;
using System;
using System.Collections;
using System.Collections.Generic;

namespace OOBehave.UnitTest.ObjectPortal
{
    public class BaseObject : Base<BaseObject>, IBaseObject
    {

        public BaseObject(IBaseServices<BaseObject> baseServices) : base(baseServices)
        {
        }

        public Guid GuidCriteria { get; set; } = Guid.Empty;
        public int IntCriteria { get; set; } = -1;
        public object[] MultipleCriteria { get; set; }

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
        private void CreateMultiple(int i, string s)
        {
            MultipleCriteria = new object[] { i, s };
        }

        [Create]
        private void CreateMultiple(int i, double d, IDisposableDependency dep)
        {
            Assert.IsNotNull(dep);
            MultipleCriteria = new object[] { i, d };
        }

        [Create]
        private void CreateErrorDuplicate(uint i)
        {
            Assert.Fail("Should not have reached");
        }
        [Create]
        private void CreateErrorDuplicate(uint i, IDisposableDependency dep)
        {
            Assert.Fail("Should not have reached");
        }

        [Create]
        private void CreateInfertype(ICollection collection)
        {
            Assert.IsNotNull(collection);
            CreateCalled = true;
        }

        [Create]
        private void CreateNullCriteria(List<int> a, List<int> b, IDisposableDependency dep)
        {
            Assert.IsNotNull(dep);
            CreateCalled = true;
            MultipleCriteria = new object[] { a, b };
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
