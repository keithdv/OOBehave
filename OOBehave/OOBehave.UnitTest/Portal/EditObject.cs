using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOBehave.Portal;
using OOBehave.UnitTest.Objects;
using System;

namespace OOBehave.UnitTest.ObjectPortal
{
    public class EditObject : EditBase<EditObject>, IEditObject
    {

        public EditObject(IEditBaseServices<EditObject> baseServices) : base(baseServices)
        {
        }

        public string Name { get => Getter<string>(); set => Setter(value); }
        public Guid GuidCriteria { get; set; } = Guid.Empty;
        public int IntCriteria { get; set; } = -1;

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
        private void Create(Guid criteria)
        {
            GuidCriteria = criteria;
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
        private void CreateChild(Guid criteria)
        {
            GuidCriteria = criteria;
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
        private void Fetch(Guid criteria)
        {
            GuidCriteria = criteria;
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
        private void FetchChild(Guid criteria)
        {
            GuidCriteria = criteria;
        }

        [FetchChild]
        private void FetchChild(Guid criteria, IDisposableDependency dependency)
        {
            Assert.IsNotNull(dependency);
            GuidCriteria = criteria;
        }

        public bool UpdateCalled { get; set; } = false;

        [Update]
        private void Update()
        {
            UpdateCalled = true;
        }


        [Update]
        private void Update(int criteria)
        {
            IntCriteria = criteria;
        }


        [Update]
        private void Update(Guid criteria)
        {
            GuidCriteria = criteria;
        }

        [Update]
        private void Update(Guid criteria, IDisposableDependency dependency)
        {
            Assert.IsNotNull(dependency);
            GuidCriteria = criteria;
        }

        public bool UpdateChildCalled { get; set; } = false;

        [UpdateChild]
        private void UpdateChild()
        {
            UpdateChildCalled = true;
        }

        [UpdateChild]
        private void UpdateChild(int criteria)
        {
            IntCriteria = criteria;
        }


        [UpdateChild]
        private void UpdateChild(Guid criteria)
        {
            GuidCriteria = criteria;
        }

        [UpdateChild]
        private void UpdateChild(Guid criteria, IDisposableDependency dependency)
        {
            Assert.IsNotNull(dependency);
            GuidCriteria = criteria;
        }

        public bool DeleteCalled { get; set; } = false;

        [Delete]
        private void Delete()
        {
            DeleteCalled = true;
        }

        [Delete]
        private void Delete(int criteria)
        {
            IntCriteria = criteria;
        }

        [Delete]
        private void Delete(Guid criteria)
        {
            GuidCriteria = criteria;
        }

        [Delete]
        private void Delete(Guid criteria, IDisposableDependency dependency)
        {
            Assert.IsNotNull(dependency);
            GuidCriteria = criteria;
        }

        public bool DeleteChildCalled { get; set; } = false;

        [DeleteChild]
        private void DeleteChild()
        {
            DeleteChildCalled = true;
        }

        [DeleteChild]
        private void DeleteChild(int criteria)
        {
            IntCriteria = criteria;
        }

        [DeleteChild]
        private void DeleteChild(Guid criteria)
        {
            GuidCriteria = criteria;
        }

        [DeleteChild]
        private void DeleteChild(Guid criteria, IDisposableDependency dependency)
        {
            Assert.IsNotNull(dependency);
            GuidCriteria = criteria;
        }
    }
}
