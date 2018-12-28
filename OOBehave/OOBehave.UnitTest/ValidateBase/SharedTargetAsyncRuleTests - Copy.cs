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
    public class SharedTargetRule : Rules.SharedTargetAsyncRule
    {

        public SharedTargetRule(IRegisteredProperty<string> shortName, IRegisteredProperty<string> title, IRegisteredProperty<string> fullName)
        {
            ShortName = shortName;
            Title = title;
            FullName = fullName;
        }

        public IRegisteredProperty<string> ShortName { get; }
        public IRegisteredProperty<string> Title { get; }
        public IRegisteredProperty<string> FullName { get; }

        protected override async Task<IRuleResult> Execute(CancellationToken token)
        {
            await Task.Delay(10);

            var sn = $"{ReadProperty(Title)} {ReadProperty(ShortName)}";

            SetProperty(FullName, sn);

            return RuleResult.Empty();

        }
    }

    public interface ISharedTargetAsyncRuleObject : IPersonBase { }

    public class SharedTargetAsyncRuleObject : PersonValidateBase<SharedTargetAsyncRuleObject>, ISharedTargetAsyncRuleObject
    {

        public SharedTargetAsyncRuleObject(IValidateBaseServices<SharedTargetAsyncRuleObject> services) : base(services)
        {

            var fn = services.RegisteredPropertyManager.RegisterProperty<string>(nameof(FirstName));
            var ln = services.RegisteredPropertyManager.RegisterProperty<string>(nameof(LastName));
            var sn = services.RegisteredPropertyManager.RegisterProperty<string>(nameof(ShortName));

            RuleExecute.AddRule(new ShortNameRule(sn, fn, ln));

            var t = services.RegisteredPropertyManager.RegisterProperty<string>(nameof(Title));
            var fun = services.RegisteredPropertyManager.RegisterProperty<string>(nameof(FullName));

            RuleExecute.AddRule(new SharedTargetRule(sn, t, fun));
        }

    }

    [TestClass]
    public class SharedTargetAsyncRuleTests
    {

        private ILifetimeScope scope;
        private ISharedTargetAsyncRuleObject target;

        [TestInitialize]
        public void TestInitailize()
        {
            scope = AutofacContainer.GetLifetimeScope();
            target = scope.Resolve<IReceivePortal<ISharedTargetAsyncRuleObject>>().Create().Result;

        }


        [TestCleanup]
        public void TestInitalize()
        {
            scope.Dispose();
        }

        [TestMethod]
        public async Task SharedTargetAsyncRuleTests_ShortName()
        {
            target.Title = "Mr.";
            target.FirstName = "John";
            target.LastName = "Smith";

            await target.WaitForRules();

            Assert.AreEqual("Mr. John Smith", target.FullName);

        }
    }
}
