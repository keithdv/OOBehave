using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOBehave.Core;
using OOBehave.Portal;
using OOBehave.UnitTest.PersonObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.ValidateBase
{

    public interface IParentChild : IPersonBase
    {
        IPersonBase Child { get; }
    }

    public class ParentChild : PersonBase<ParentChild>, IParentChild
    {
        public ParentChild(IValidateBaseServices<ParentChild> services,
            IShortNameRule<ParentChild> shortNameRule,
            IFullNameRule<ParentChild> fullNameRule,
            IPersonRule<ParentChild> personRule) : base(services)
        {
            RuleExecute.AddRules(shortNameRule, fullNameRule, personRule);
        }

        public IPersonBase Child
        {
            get { return ReadProperty<IPersonBase>(); }
            set { SetProperty(value); }
        }

        [Fetch]
        public async Task Fetch(PersonDto person, IReceivePortal<IParentChild> portal, IReadOnlyList<PersonDto> personTable)
        {
            base.FillFromDto(person);

            var child = personTable.FirstOrDefault(p => p.FatherId == PersonId);

            if (child != null)
            {
                LoadProperty<IPersonBase>(await portal.FetchChild(child), nameof(Child));
            }
        }


        [FetchChild]
        public void Fetch(PersonDto dto)
        {
            base.FillFromDto(dto);
        }

    }

    [TestClass]
    public class ValidateParentChildTests
    {
        private ILifetimeScope scope;
        private Task<IParentChild> parentTask;
        [TestInitialize]
        public void TestInitailize()
        {
            scope = AutofacContainer.GetLifetimeScope();
            var parentDto = scope.Resolve<IReadOnlyList<PersonDto>>().Where(p => !p.FatherId.HasValue && !p.MotherId.HasValue).First();
            parentTask = scope.Resolve<IReceivePortal<IParentChild>>().Fetch(parentDto);
        }

        [TestCleanup]
        public void TestInitalize()
        {
            scope.Dispose();
        }

        [TestMethod]
        public void ValidateAsyncRules_Const()
        {
        }

        [TestMethod]
        public async Task ValidateParentChildTests_Fetch()
        {
            var parent = await parentTask;
            Assert.IsTrue(parent.IsValid);
        }

        [TestMethod]
        public async Task ValidateParentChildTests_ParentInvalid()
        {
            var parent = await parentTask;
            parent.FirstName = "Error";
            Assert.IsFalse(parent.IsValid);
            Assert.IsFalse(parent.IsSelfValid);
            Assert.IsTrue(parent.Child.IsValid);
            Assert.IsTrue(parent.Child.IsSelfValid);
        }

        [TestMethod]
        public async Task ValidateParentChildTests_ChildInvalid()
        {
            var parent = await parentTask;
            parent.Child.FirstName = "Error";
            Assert.IsFalse(parent.IsValid);
            Assert.IsTrue(parent.IsSelfValid);
            Assert.IsFalse(parent.Child.IsValid);
            Assert.IsFalse(parent.Child.IsSelfValid);
        }
    }
}

