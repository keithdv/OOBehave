using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOBehave.Portal;
using OOBehave.UnitTest.Objects;
using System;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.ObjectPortal
{
    public class EditObjectList : EditListBase<EditObjectList, IEditObject>, IEditObjectList
    {

        public EditObjectList(IEditListBaseServices<EditObjectList, IEditObject> baseServices) : base(baseServices)
        {
        }

        private IRegisteredProperty<Guid?> IDProperty => GetRegisteredProperty<Guid?>(nameof(ID));
        public Guid? ID { get => Getter<Guid?>(); set => Setter(value); }
        public Guid GuidCriteria { get => Getter<Guid>(); set => Setter(value); }
        public int IntCriteria { get => Getter<int>(); set => Setter(value); }

        public bool CreateCalled { get; set; } = false;

        [Create]
        private void Create()
        {
            ID = Guid.NewGuid();
            CreateCalled = true;
        }

        [Create]
        private void Create(int criteria)
        {
            IntCriteria = criteria;
            CreateCalled = true;
        }


        [Create]
        private void Create(Guid criteria, IPortalOperationDisposableDependency dependency)
        {
            Assert.IsNotNull(dependency);
            GuidCriteria = criteria;
            CreateCalled = true;
        }

        public bool CreateChildCalled { get; set; } = false;

        [CreateChild]
        private void CreateChild()
        {
            ID = Guid.NewGuid();
            CreateChildCalled = true;
        }

        [CreateChild]
        private void CreateChild(int criteria)
        {
            IntCriteria = criteria;
            CreateChildCalled = true;
        }

        [CreateChild]
        private void CreateChild(Guid criteria, IPortalOperationDisposableDependency dependency)
        {
            Assert.IsNotNull(dependency);
            GuidCriteria = criteria;
            CreateChildCalled = true;
        }

        public bool FetchCalled { get; set; } = false;

        [Fetch]
        private void Fetch()
        {
            ID = Guid.NewGuid();
            FetchCalled = true;
        }

        [Fetch]
        private void Fetch(int criteria)
        {
            IntCriteria = criteria;
            FetchCalled = true;
        }


        [Fetch]
        private void Fetch(Guid criteria, IPortalOperationDisposableDependency dependency)
        {
            Assert.IsNotNull(dependency);
            GuidCriteria = criteria;
            FetchCalled = true;
        }

        public bool FetchChildCalled { get; set; } = false;

        [FetchChild]
        private void FetchChild()
        {
            ID = Guid.NewGuid();
            FetchChildCalled = true;
        }

        [FetchChild]
        private void FetchChild(int criteria)
        {
            IntCriteria = criteria;
            FetchChildCalled = true;
        }

        [FetchChild]
        private void FetchChild(Guid criteria, IPortalOperationDisposableDependency dependency)
        {
            Assert.IsNotNull(dependency);
            GuidCriteria = criteria;
            FetchChildCalled = true;
        }

        public bool InsertCalled { get; set; } = false;

        [Insert]
        private void Insert()
        {
            ID = Guid.NewGuid();
            InsertCalled = true;
        }


        [Insert]
        private void Insert(int criteria)
        {
            InsertCalled = true;
            IntCriteria = criteria;
        }


        [Insert]
        private void Insert(Guid criteria, IPortalOperationDisposableDependency dependency)
        {
            Assert.IsNotNull(dependency);
            InsertCalled = true;
            GuidCriteria = criteria;
        }

        public bool InsertChildCalled { get; set; } = false;

        [InsertChild]
        private void InsertChild()
        {
            ID = Guid.NewGuid();
            InsertChildCalled = true;
        }

        [InsertChild]
        private void InsertChild(int criteria)
        {
            IntCriteria = criteria;
            InsertChildCalled = true;
        }


        [InsertChild]
        private void InsertChild(Guid criteria, IPortalOperationDisposableDependency dependency)
        {
            Assert.IsNotNull(dependency);
            GuidCriteria = criteria;
            InsertChildCalled = true;
        }

        public bool UpdateCalled { get; set; } = false;

        protected override async Task Update()
        {
            if (IsSelfModified)
            {
                ID = Guid.NewGuid();
                UpdateCalled = true;
            }
            await UpdateList();
        }

        [Update]
        private void Update(int criteria)
        {
            IntCriteria = criteria;
            UpdateCalled = true;
        }


        [Update]
        private void Update(Guid criteria, IPortalOperationDisposableDependency dependency)
        {
            Assert.IsNotNull(dependency);
            GuidCriteria = criteria;
            UpdateCalled = true;
        }

        public bool UpdateChildCalled { get; set; } = false;

        [UpdateChild]
        private void UpdateChild()
        {
            ID = Guid.NewGuid();
            UpdateChildCalled = true;
        }

        [UpdateChild]
        private void UpdateChild(int criteria)
        {
            IntCriteria = criteria;
            UpdateChildCalled = true;
        }


        [UpdateChild]
        private void UpdateChild(Guid criteria, IPortalOperationDisposableDependency dependency)
        {
            Assert.IsNotNull(dependency);
            GuidCriteria = criteria;
            UpdateChildCalled = true;
        }

        public bool DeleteCalled { get; set; } = false;

        [Delete]
        private void Delete_()
        {
            DeleteCalled = true;
        }

        [Delete]
        private void Delete(int criteria)
        {
            IntCriteria = criteria;
            DeleteCalled = true;
        }

        [Delete]
        private void Delete(Guid criteria, IPortalOperationDisposableDependency dependency)
        {
            Assert.IsNotNull(dependency);
            GuidCriteria = criteria;
            DeleteCalled = true;
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
            DeleteChildCalled = true;
        }

        [DeleteChild]
        private void DeleteChild(Guid criteria, IPortalOperationDisposableDependency dependency)
        {
            Assert.IsNotNull(dependency);
            GuidCriteria = criteria;
            DeleteChildCalled = true;
        }

    }
}
