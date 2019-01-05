using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOBehave.Portal;
using OOBehave.UnitTest.Objects;
using System;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.ObjectPortal
{
    public class BaseObjectList : ListBase<BaseObjectList, IBaseObject>, IBaseObjectList
    {

        public BaseObjectList(IListBaseServices<BaseObjectList, IBaseObject> baseServices) : base(baseServices)
        {
        }

        public Guid GuidCriteria { get; set; } = Guid.Empty;
        public int IntCriteria { get; set; } = -1;

        public bool CreateCalled { get; set; } = false;

        [Create]
        private async Task Create()
        {
            CreateCalled = true;
            Add(await ItemPortal.CreateChild());
        }

        [Create]
        private async Task Create(int criteria)
        {
            IntCriteria = criteria;
            Add(await ItemPortal.CreateChild(criteria));
        }

        [Create]
        private async Task Create(Guid criteria, IDisposableDependency dependency)
        {
            Assert.IsNotNull(dependency);
            GuidCriteria = criteria;
            Add(await ItemPortal.CreateChild(criteria));
        }

        public bool CreateChildCalled { get; set; } = false;

        [CreateChild]
        private async Task CreateChild()
        {
            CreateChildCalled = true;
            Add(await ItemPortal.CreateChild());
        }

        [CreateChild]
        private async Task CreateChild(int criteria)
        {
            IntCriteria = criteria;
            Add(await ItemPortal.CreateChild(criteria));
        }

        [CreateChild]
        private async Task CreateChild(Guid criteria, IDisposableDependency dependency)
        {
            Assert.IsNotNull(dependency);
            GuidCriteria = criteria;
            Add(await ItemPortal.CreateChild(criteria));
        }

        public bool FetchCalled { get; set; } = false;

        [Fetch]
        private async Task Fetch()
        {
            FetchCalled = true;
            Add(await ItemPortal.FetchChild());
        }

        [Fetch]
        private async Task Fetch(int criteria)
        {
            IntCriteria = criteria;
            Add(await ItemPortal.FetchChild(criteria));
        }

        [Fetch]
        private async Task Fetch(Guid criteria, IDisposableDependency dependency)
        {
            Assert.IsNotNull(dependency);
            GuidCriteria = criteria;
            Add(await ItemPortal.FetchChild(criteria));
        }

        public bool FetchChildCalled { get; set; } = false;

        [FetchChild]
        private async Task FetchChild()
        {
            FetchChildCalled = true;
            Add(await ItemPortal.FetchChild());
        }

        [FetchChild]
        private async Task FetchChild(int criteria)
        {
            IntCriteria = criteria;
            Add(await ItemPortal.FetchChild(criteria));
        }


        [FetchChild]
        private async Task FetchChild(Guid criteria, IDisposableDependency dependency)
        {
            Assert.IsNotNull(dependency);
            GuidCriteria = criteria;
            Add(await ItemPortal.FetchChild(criteria));

        }

    }
}
