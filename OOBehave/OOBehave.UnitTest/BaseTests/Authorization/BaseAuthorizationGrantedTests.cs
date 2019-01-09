using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOBehave.AuthorizationRules;
using OOBehave.Portal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.BaseTests.Authorization
{
    public interface IAuthorizationGrantedRule : IAuthorizationRule
    {
        int IntCriteria { get; set; }
        Guid? GuidCriteria { get; set; }
        bool ExecuteCreateCalled { get; }
        bool ExecuteFetchCalled { get; set; }
        bool ExecuteUpdateCalled { get; set; }
        bool ExecuteDeleteCalled { get; set; }
    }
    public class AuthorizationGrantedRule : AuthorizationRule, IAuthorizationGrantedRule
    {
        public int IntCriteria { get; set; }
        public Guid? GuidCriteria { get; set; }
        public bool ExecuteCreateCalled { get; set; }

        [Execute(AuthorizeOperation.Create)]
        public IAuthorizationRuleResult ExecuteCreate()
        {
            ExecuteCreateCalled = true;
            return AuthorizationRuleResult.AccessGranted();
        }

        [Execute(AuthorizeOperation.Create)]
        public IAuthorizationRuleResult ExecuteCreate(int criteria)
        {
            ExecuteCreateCalled = true;
            IntCriteria = criteria;
            return AuthorizationRuleResult.AccessGranted();
        }

        [Execute(AuthorizeOperation.Create)]
        public IAuthorizationRuleResult ExecuteCreate(int intCriteria, Guid? guidCriteria)
        {
            ExecuteCreateCalled = true;
            IntCriteria = intCriteria;
            GuidCriteria = guidCriteria;
            return AuthorizationRuleResult.AccessGranted();
        }

        public bool ExecuteFetchCalled { get; set; }
        [Execute(AuthorizeOperation.Fetch)]
        public IAuthorizationRuleResult ExecuteFetch()
        {
            ExecuteFetchCalled = true;
            return AuthorizationRuleResult.AccessGranted();
        }

        [Execute(AuthorizeOperation.Fetch)]
        public IAuthorizationRuleResult ExecuteFetch(int criteria)
        {
            ExecuteFetchCalled = true;
            IntCriteria = criteria;
            return AuthorizationRuleResult.AccessGranted();
        }

        public bool ExecuteUpdateCalled { get; set; }
        [Execute(AuthorizeOperation.Update)]
        public IAuthorizationRuleResult ExecuteUpdate()
        {
            ExecuteUpdateCalled = true;
            return AuthorizationRuleResult.AccessGranted();
        }

        public bool ExecuteDeleteCalled { get; set; }
        [Execute(AuthorizeOperation.Delete)]
        public IAuthorizationRuleResult ExecuteDelete()
        {
            ExecuteDeleteCalled = true;
            return AuthorizationRuleResult.AccessGranted();
        }
    }

    public interface IBaseAuthorizationGrantedObject : IBase { }

    public class BaseAuthorizationGrantedObject : Base<BaseAuthorizationGrantedObject>, IBaseAuthorizationGrantedObject
    {

        public BaseAuthorizationGrantedObject(IBaseServices<BaseAuthorizationGrantedObject> services) : base(services)
        {

        }

        [AuthorizationRules]
        public static void RegisterAuthorizationRules(IAuthorizationRuleManager authorizationRuleManager)
        {
            authorizationRuleManager.AddRule<IAuthorizationGrantedRule>();
        }

        [Create]
        public void Create(int criteria) { }

        [Create]
        public void Create(int i, Guid? g) { }

        [Fetch]
        private void Fetch() { }

        [Fetch]
        public void Fetch(int criteria) { }

    }

    [TestClass]
    public class BaseAuthorizationGrantedTests
    {

        ILifetimeScope scope;
        IReceivePortal<IBaseAuthorizationGrantedObject> portal;

        [TestInitialize]
        public void TestInitialize()
        {
            scope = AutofacContainer.GetLifetimeScope(true);
            portal = scope.Resolve<IReceivePortal<IBaseAuthorizationGrantedObject>>();
        }

        [TestMethod]
        public async Task BaseAuthorization_Create()
        {
            var obj = await portal.Create();
            var authRule = scope.Resolve<IAuthorizationGrantedRule>();
            Assert.IsTrue(authRule.ExecuteCreateCalled);
        }

        [TestMethod]
        public async Task BaseAuthorization_Create_Criteria()
        {
            var criteria = DateTime.Now.Millisecond;
            var obj = await portal.Create(criteria);
            var authRule = scope.Resolve<IAuthorizationGrantedRule>();
            Assert.IsTrue(authRule.ExecuteCreateCalled);
            Assert.AreEqual(criteria, authRule.IntCriteria);
        }

        [TestMethod]
        public async Task BaseAuthorization_Create_MultipleCriteria()
        {
            var intC = DateTime.Now.Millisecond;
            var guidC = Guid.NewGuid();

            var obj = await portal.Create(intC, guidC);
            var authRule = scope.Resolve<IAuthorizationGrantedRule>();
            Assert.IsTrue(authRule.ExecuteCreateCalled);
            Assert.AreEqual(intC, authRule.IntCriteria);
            Assert.AreEqual(guidC, authRule.GuidCriteria.Value);
        }

        [TestMethod]
        public async Task BaseAuthorization_Fetch()
        {
            var obj = await portal.Fetch();
            var authRule = scope.Resolve<IAuthorizationGrantedRule>();
            Assert.IsTrue(authRule.ExecuteFetchCalled);
        }

        [TestMethod]
        public async Task BaseAuthorization_Fetch_Criteria()
        {
            var criteria = DateTime.Now.Millisecond;
            var obj = await portal.Fetch(criteria);
            var authRule = scope.Resolve<IAuthorizationGrantedRule>();
            Assert.IsTrue(authRule.ExecuteFetchCalled);
            Assert.AreEqual(criteria, authRule.IntCriteria);
        }
    }
}
