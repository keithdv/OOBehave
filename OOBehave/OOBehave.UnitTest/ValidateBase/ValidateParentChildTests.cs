using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOBehave.Core;
using OOBehave.Portal;
using OOBehave.UnitTest.PersonObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOBehave.UnitTest.ValidateBase
{

    public interface IPerson : IPersonBase
    {
        IPerson Mate { get; }
        IPerson Child { get; }
    }

    public class Person : PersonBase<Person>, IPerson
    {
        public Person(IValidateBaseServices<Person> services) : base(services)
        {
            RuleExecute.AddRule(new ShortNameAsyncRule<Person>());
            RuleExecute.AddRule(new FullNameAsyncRule<Person>());
            RuleExecute.AddRule(new PersonAsyncRule<Person>());
        }

        public IPerson Mate
        {
            get { return ReadProperty<IPerson>(); }
            set { SetProperty(value); }
        }

        public IPerson Child
        {
            get { return ReadProperty<IPerson>(); }
            set { SetProperty(value); }
        }

        [Create]
        public void Create(IObjectPortal<IPerson> portal, IReadOnlyList<PersonDto> personTable)
        {
            var dto = personTable.Single(p => p.FirstName == "Grandpa");
            base.FillFromDto(dto);

            var child = personTable.FirstOrDefault(p => p.FatherId == PersonId);

            if (child != null)
            {
                LoadProperty(portal.CreateChild(child), nameof(Child));
            }
        }


        [CreateChild]
        public void Create(PersonDto dto, IObjectPortal<IPerson> portal, IReadOnlyList<PersonDto> personTable)
        {
            base.FillFromDto(dto);

            var child = personTable.FirstOrDefault(p => p.FatherId == PersonId);

            if (child != null)
            {
                LoadProperty(portal.CreateChild(child), nameof(Child));
            }
        }

    }

    [TestClass]
    public class ValidateParentChildTests
    {
        private ILifetimeScope scope;

        [TestInitialize]
        public void TestInitailize()
        {
            scope = AutofacContainer.GetLifetimeScope();
        }

        [TestMethod]
        public void ValidateAsyncRules_Const()
        {
            scope.Dispose();
        }

        [TestMethod]
        public void ValidateParentChildTests_Create_Parent()
        {
            var parent = scope.Resolve<IObjectPortal<IPerson>>().Create();
        }
    }
}
