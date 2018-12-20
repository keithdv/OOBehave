using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOBehave.Core;
using OOBehave.UnitTest.PersonObjects;
using System;
using System.Collections.Generic;
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
            var parent = scope.Resolve<IFactory>().Create<IPerson>();


        }
    }
}
