﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            CreateCalled = true;
        }

        [Create]
        private void Create(Guid criteria)
        {
            GuidCriteria = criteria;
            CreateCalled = true;
        }


        [Create]
        private void Create(Guid criteria, IDisposableDependency dependency)
        {
            Assert.IsNotNull(dependency);
            GuidCriteria = criteria;
            CreateCalled = true;
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
            CreateChildCalled = true;
        }

        [CreateChild]
        private void CreateChild(Guid criteria)
        {
            GuidCriteria = criteria;
            CreateChildCalled = true;
        }

        [CreateChild]
        private void CreateChild(Guid criteria, IDisposableDependency dependency)
        {
            Assert.IsNotNull(dependency);
            GuidCriteria = criteria;
            CreateChildCalled = true;
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
            FetchCalled = true;
        }

        [Fetch]
        private void Fetch(Guid criteria)
        {
            GuidCriteria = criteria;
            FetchCalled = true;
        }


        [Fetch]
        private void Fetch(Guid criteria, IDisposableDependency dependency)
        {
            Assert.IsNotNull(dependency);
            GuidCriteria = criteria;
            FetchCalled = true;
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
            FetchChildCalled = true;
        }

        [FetchChild]
        private void FetchChild(Guid criteria)
        {
            GuidCriteria = criteria;
            FetchChildCalled = true;
        }

        [FetchChild]
        private void FetchChild(Guid criteria, IDisposableDependency dependency)
        {
            Assert.IsNotNull(dependency);
            GuidCriteria = criteria;
            FetchChildCalled = true;
        }

        public bool InsertCalled { get; set; } = false;

        [Insert]
        private void Insert()
        {
            InsertCalled = true;
        }


        [Insert]
        private void Insert(int criteria)
        {
            InsertCalled = true;
            IntCriteria = criteria;
        }


        [Insert]
        private void Insert(Guid criteria)
        {
            InsertCalled = true;
            GuidCriteria = criteria;
        }

        [Insert]
        private void Insert(Guid criteria, IDisposableDependency dependency)
        {
            Assert.IsNotNull(dependency);
            InsertCalled = true;
            GuidCriteria = criteria;
        }

        public bool InsertChildCalled { get; set; } = false;

        [InsertChild]
        private void InsertChild()
        {
            InsertChildCalled = true;
        }

        [InsertChild]
        private void InsertChild(int criteria)
        {
            IntCriteria = criteria;
            InsertChildCalled = true;
        }


        [InsertChild]
        private void InsertChild(Guid criteria)
        {
            GuidCriteria = criteria;
            InsertChildCalled = true;
        }

        [InsertChild]
        private void InsertChild(Guid criteria, IDisposableDependency dependency)
        {
            Assert.IsNotNull(dependency);
            GuidCriteria = criteria;
            InsertChildCalled = true;
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
            UpdateCalled = true;
        }


        [Update]
        private void Update(Guid criteria)
        {
            GuidCriteria = criteria;
            UpdateCalled = true;
        }

        [Update]
        private void Update(Guid criteria, IDisposableDependency dependency)
        {
            Assert.IsNotNull(dependency);
            GuidCriteria = criteria;
            UpdateCalled = true;
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
            UpdateChildCalled = true;
        }


        [UpdateChild]
        private void UpdateChild(Guid criteria)
        {
            GuidCriteria = criteria;
            UpdateChildCalled = true;
        }

        [UpdateChild]
        private void UpdateChild(Guid criteria, IDisposableDependency dependency)
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
        private void Delete(Guid criteria)
        {
            GuidCriteria = criteria;
            DeleteCalled = true;
        }

        [Delete]
        private void Delete(Guid criteria, IDisposableDependency dependency)
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
        private void DeleteChild(Guid criteria)
        {
            GuidCriteria = criteria;
            DeleteChildCalled = true;
        }

        [DeleteChild]
        private void DeleteChild(Guid criteria, IDisposableDependency dependency)
        {
            Assert.IsNotNull(dependency);
            GuidCriteria = criteria;
            DeleteChildCalled = true;
        }
    }
}
