using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOBehave.Core;
using OOBehave.Portal;
using OOBehave.Rules;
using OOBehave.UnitTest.PersonObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.ValidateBase
{

    public interface IParentChildList : IValidateListBase<IParentChild>
    {
        string FirstName { get; set; }
    }

    public class ParentChildList : ValidateListBase<ParentChildList, IParentChild>, IParentChildList
    {
        public ParentChildList(IValidateListBaseServices<ParentChildList, IParentChild> services) : base(services)
        {
            RuleExecute.AddRule(nameof(FirstName), target =>
            {
                if (target.FirstName == "Error")
                {
                    return RuleResult.PropertyError(nameof(FirstName), "Error");
                }
                return RuleResult.Empty();
            });
        }

        public string FirstName { get { return Getter<string>(); } set { Setter(value); } }

        [Fetch]
        public async Task Fetch(Guid parentId, IReadOnlyList<PersonDto> personTable)
        {

            var children = personTable.Where(p => p.FatherId == parentId);

            foreach (var child in children)
            {
                Add(await ItemPortal.FetchChild(child));
            }

        }


    }

    [TestClass]
    public class ValidateParentChildListTests
    {
        private ILifetimeScope scope;
        private IParentChildList parent;
        private IPersonBase child;

        [TestInitialize]
        public void TestInitailize()
        {
            scope = AutofacContainer.GetLifetimeScope();
            var parentDto = scope.Resolve<IReadOnlyList<PersonDto>>().Where(p => !p.FatherId.HasValue && !p.MotherId.HasValue).First();
            parent = scope.Resolve<IReceivePortal<IParentChildList>>().Fetch(parentDto.PersonId).Result;
            child = parent.First();
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
        public void ValidateParentChildListTests_Fetch()
        {
            Assert.IsTrue(parent.IsValid);
        }

        [TestMethod]
        public async Task ValidateParentChildListTests_ParentInvalid()
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
        public async Task ValidateParentChildListTests_ChildInvalid()
        {
            child.FirstName = "Error";
            await parent.WaitForRules();
            Assert.IsFalse(child.IsValid);
            Assert.IsFalse(child.IsSelfValid);
            Assert.IsFalse(parent.IsBusy);
            Assert.IsFalse(parent.IsValid);
            Assert.IsTrue(parent.IsSelfValid);
        }

        [TestMethod]
        public async Task ValidateParentChildListTests_Child_IsBusy()
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

