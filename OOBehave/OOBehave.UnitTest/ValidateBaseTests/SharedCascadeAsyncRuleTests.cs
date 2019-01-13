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

namespace OOBehave.UnitTest.ValidateBaseTests
{
    public class ShortNameRule : Rules.AsyncRuleBase<IBase>
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

        public override async Task<IRuleResult> Execute(IBase target, CancellationToken token)
        {
            await Task.Delay(10);

            var sn = $"{ReadProperty(target, firstName)} {ReadProperty(target, lastName)}";

            SetProperty(target, shortName, sn);

            return RuleResult.Empty();

        }
    }

    public interface ISharedAsyncRuleObject : IPersonBase { }

    public class SharedAsyncRuleObject : PersonValidateBase<SharedAsyncRuleObject>, ISharedAsyncRuleObject
    {

        public SharedAsyncRuleObject(IValidateBaseServices<SharedAsyncRuleObject> services) : base(services)
        {

            var fn = services.RegisteredPropertyManager.GetRegisteredProperty<string>(nameof(FirstName));
            var ln = services.RegisteredPropertyManager.GetRegisteredProperty<string>(nameof(LastName));
            var sn = services.RegisteredPropertyManager.GetRegisteredProperty<string>(nameof(ShortName));

            RuleManager.AddRule(new ShortNameRule(sn, fn, ln));

        }

    }

    [TestClass]
    public class SharedAsyncRuleTests
    {

        private ILifetimeScope scope;
        private ISharedAsyncRuleObject target;

        [TestInitialize]
        public void TestInitailize()
        {
            scope = AutofacContainer.GetLifetimeScope();
            target = scope.Resolve<ISharedAsyncRuleObject>();

        }


        [TestCleanup]
        public void TestInitalize()
        {
            scope.Dispose();
        }

        [TestMethod]
        public async Task SharedAsyncRuleTests_ShortName()
        {
            target.FirstName = "John";
            target.LastName = "Smith";

            await target.WaitForRules();

            Assert.AreEqual("John Smith", target.ShortName);

        }
    }
}
