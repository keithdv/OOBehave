using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOBehave.Portal;
using OOBehave.Rules;
using OOBehave.UnitTest.PersonObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.ValidateBase
{
    public class ShortNameRule : Rules.SharedCascadeAsyncRule
    {
        private readonly IRegisteredProperty<string> shortName;
        private readonly IRegisteredProperty<string> firstName;
        private readonly IRegisteredProperty<string> lastName;

        public ShortNameRule(IRegisteredProperty<string> shortName, IRegisteredProperty<string> firstName, IRegisteredProperty<string> lastName) : base(shortName, firstName, lastName)
        {
            this.shortName = shortName;
            this.firstName = firstName;
            this.lastName = lastName;
        }

        protected override async Task<IRuleResult> Execute(CancellationToken token)
        {
            await Task.Delay(10);

            var sn = $"{ReadProperty(firstName)} {ReadProperty(lastName)}";

            SetProperty(shortName, sn);

            return RuleResult.Empty();

        }
    }

    public interface ISharedCascadeAsyncRuleObject : IPersonBase { }

    public class SharedCascadeAsyncRuleObject : PersonValidateBase<SharedCascadeAsyncRuleObject>, ISharedCascadeAsyncRuleObject
    {

        public SharedCascadeAsyncRuleObject(IValidateBaseServices<SharedCascadeAsyncRuleObject> services) : base(services)
        {

            var fn = services.RegisteredPropertyManager.RegisterProperty<string>(nameof(FirstName));
            var ln = services.RegisteredPropertyManager.RegisterProperty<string>(nameof(LastName));
            var sn = services.RegisteredPropertyManager.RegisterProperty<string>(nameof(ShortName));

            RuleExecute.AddRule(new ShortNameRule(sn, fn, ln));
            
        }

    }

    [TestClass]
    public class SharedCascadeAsyncRuleTests
    {

        private ILifetimeScope scope;
        private ISharedCascadeAsyncRuleObject target;

        [TestInitialize]
        public void TestInitailize()
        {
            scope = AutofacContainer.GetLifetimeScope();
            target = scope.Resolve<IReceivePortal<ISharedCascadeAsyncRuleObject>>().Create().Result;

        }


        [TestCleanup]
        public void TestInitalize()
        {
            scope.Dispose();
        }

        [TestMethod]
        public async Task SharedCascadeAsyncRuleTests_ShortName()
        {
            target.FirstName = "John";
            target.LastName = "Smith";

            await target.WaitForRules();

            Assert.AreEqual("John Smith", target.ShortName);

        }
    }
}
