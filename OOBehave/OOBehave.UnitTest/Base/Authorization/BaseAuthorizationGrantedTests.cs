using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOBehave.AuthorizationRules;
using OOBehave.Portal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.Base.Authorization
{

    public class AuthorizationGrantedRule : AuthorizationRule
    {
        public int Criteria { get; set; }
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
            Criteria = criteria;
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
            Criteria = criteria;
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
            authorizationRuleManager.AddRule<AuthorizationGrantedRule>();
        }

        [Create]
        public void Create(int criteria) { }

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
            scope = AutofacContainer.GetLifetimeScope();
            portal = scope.Resolve<IReceivePortal<IBaseAuthorizationGrantedObject>>();
        }

        [TestMethod]
        public async Task BaseAuthorization_Create()
        {
            var obj = await portal.Create();
            var authRule = scope.Resolve<AuthorizationGrantedRule>();
            Assert.IsTrue(authRule.ExecuteCreateCalled);
        }

        [TestMethod]
        public async Task BaseAuthorization_Create_Criteria()
        {
            var criteria = DateTime.Now.Millisecond;
            var obj = await portal.Create(criteria);
            var authRule = scope.Resolve<AuthorizationGrantedRule>();
            Assert.IsTrue(authRule.ExecuteCreateCalled);
            Assert.AreEqual(criteria, authRule.Criteria);
        }

        [TestMethod]
        public async Task BaseAuthorization_Fetch()
        {
            var obj = await portal.Fetch();
            var authRule = scope.Resolve<AuthorizationGrantedRule>();
            Assert.IsTrue(authRule.ExecuteFetchCalled);
        }

        [TestMethod]
        public async Task BaseAuthorization_Fetch_Criteria()
        {
            var criteria = DateTime.Now.Millisecond;
            var obj = await portal.Fetch(criteria);
            var authRule = scope.Resolve<AuthorizationGrantedRule>();
            Assert.IsTrue(authRule.ExecuteFetchCalled);
            Assert.AreEqual(criteria, authRule.Criteria);
        }
    }
}
