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

    public class ParentChild : PersonValidateBase<ParentChild>, IParentChild
    {
        public ParentChild(IValidateBaseServices<ParentChild> services,
            IShortNameAsyncRule<ParentChild> shortNameRule,
            IFullNameAsyncRule<ParentChild> fullNameRule,
            IPersonAsyncRule<ParentChild> personRule) : base(services)
        {
            RuleExecute.AddRules(shortNameRule, fullNameRule, personRule);
        }

        public IPersonBase Child
        {
            get { return Getter<IPersonBase>(); }
            set { Setter(value); }
        }

        [Fetch]
        public async Task Fetch(PersonDto person, IReceivePortalChild<IParentChild> portal, IReadOnlyList<PersonDto> personTable)
        {
            base.FillFromDto(person);

            var childDto = personTable.FirstOrDefault(p => p.FatherId == PersonId);

            if (childDto != null)
            {
                Child = await portal.FetchChild(childDto);
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
        private IParentChild parent;
        private IPersonBase child;

        [TestInitialize]
        public void TestInitailize()
        {
            scope = AutofacContainer.GetLifetimeScope();
            var parentDto = scope.Resolve<IReadOnlyList<PersonDto>>().Where(p => !p.FatherId.HasValue && !p.MotherId.HasValue).First();
            parent = scope.Resolve<IReceivePortal<IParentChild>>().Fetch(parentDto).Result;
            child = parent.Child;
        }


        [TestCleanup]
        public void TestInitalize()
        {
            Assert.IsFalse(parent.IsBusy);
            scope.Dispose();
        }

        [TestMethod]
        public void ValidateAsyncRules_Const()
        {
        }

        [TestMethod]
        public void ValidateParentChildTests_Fetch()
        {
            Assert.IsTrue(parent.IsValid);
        }

        [TestMethod]
        public async Task ValidateParentChildTests_ParentInvalid()
        {
            parent.FirstName = "Error";
            await parent.WaitForRules();
            Assert.IsFalse(parent.IsBusy);
            Assert.IsFalse(parent.IsValid);
            Assert.IsFalse(parent.IsSelfValid);
            Assert.IsTrue(child.IsValid);
            Assert.IsTrue(child.IsSelfValid);
        }

        [TestMethod]
        public async Task ValidateParentChildTests_ChildInvalid()
        {
            child.FirstName = "Error";
            await parent.WaitForRules();
            Assert.IsFalse(parent.IsBusy);
            Assert.IsFalse(parent.IsValid);
            Assert.IsTrue(parent.IsSelfValid);
            Assert.IsFalse(child.IsValid);
            Assert.IsFalse(child.IsSelfValid);
        }

        [TestMethod]
        public async Task ValidateParentChildTests_Child_IsBusy()
        {
            child.FirstName = "Error";

            Assert.IsTrue(parent.IsBusy);
            Assert.IsFalse(parent.IsSelfBusy);
            Assert.IsTrue(child.IsBusy);
            Assert.IsTrue(child.IsSelfBusy);

            await parent.WaitForRules();

            Assert.IsFalse(parent.IsBusy);
            Assert.IsFalse(parent.IsValid);
            Assert.IsTrue(parent.IsSelfValid);
            Assert.IsFalse(child.IsValid);
            Assert.IsFalse(child.IsSelfValid);
        }
    }
}

