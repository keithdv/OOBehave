using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOBehave.AuthorizationRules;
using OOBehave.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.BaseTests.Authorization
{

    public interface IAuthorizationGrantedAsyncRule : IAuthorizationRule
    {
        int Criteria { get; set; }
        bool ExecuteCreateCalled { get; }
        bool ExecuteFetchCalled { get; set; }
        bool ExecuteUpdateCalled { get; set; }
        bool ExecuteDeleteCalled { get; set; }
    }

    public class AuthorizationGrantedAsyncRule : AuthorizationRule, IAuthorizationGrantedAsyncRule
    {
        public int Criteria { get; set; }
        public bool ExecuteCreateCalled { get; set; }
        [Execute(AuthorizeOperation.Create)]
        public async Task<IAuthorizationRuleResult> ExecuteCreate()
        {
            await Task.Delay(10);
            ExecuteCreateCalled = true;
            return AuthorizationRuleResult.AccessGranted();
        }

        [Execute(AuthorizeOperation.Create)]
        public async Task<IAuthorizationRuleResult> ExecuteCreate(int criteria)
        {
            await Task.Delay(10);
            ExecuteCreateCalled = true;
            Criteria = criteria;
            return AuthorizationRuleResult.AccessGranted();
        }

        public bool ExecuteFetchCalled { get; set; }
        [Execute(AuthorizeOperation.Fetch)]
        public async Task<IAuthorizationRuleResult> ExecuteFetch()
        {
            await Task.Delay(10);
            ExecuteFetchCalled = true;
            return AuthorizationRuleResult.AccessGranted();
        }

        [Execute(AuthorizeOperation.Fetch)]
        public async Task<IAuthorizationRuleResult> ExecuteFetch(int criteria)
        {
            await Task.Delay(10);
            ExecuteFetchCalled = true;
            Criteria = criteria;
            return AuthorizationRuleResult.AccessGranted();
        }

        public bool ExecuteUpdateCalled { get; set; }
        [Execute(AuthorizeOperation.Update)]
        public async Task<IAuthorizationRuleResult> ExecuteUpdate()
        {
            await Task.Delay(10);
            ExecuteUpdateCalled = true;
            return AuthorizationRuleResult.AccessGranted();
        }

        public bool ExecuteDeleteCalled { get; set; }
        [Execute(AuthorizeOperation.Delete)]
        public async Task<IAuthorizationRuleResult> ExecuteDelete()
        {
            await Task.Delay(10);
            ExecuteDeleteCalled = true;
            return AuthorizationRuleResult.AccessGranted();
        }
    }

    public interface IBaseAuthorizationGrantedAsyncObject : IBase { }

    public class BaseAuthorizationGrantedAsyncObject : Base<BaseAuthorizationGrantedAsyncObject>, IBaseAuthorizationGrantedAsyncObject
    {

        public BaseAuthorizationGrantedAsyncObject(IBaseServices<BaseAuthorizationGrantedAsyncObject> services) : base(services)
        {

        }

        [AuthorizationRules]
        public static void RegisterAuthorizationRules(IAuthorizationRuleManager authorizationRuleManager)
        {
            authorizationRuleManager.AddRule<IAuthorizationGrantedAsyncRule>();
        }

        [Create]
        public void Create(int criteria) { }

        [Fetch]
        private void Fetch() { }

        [Fetch]
        public void Fetch(int criteria) { }

    }

    [TestClass]
    public class BaseAuthorizationGrantedAsyncTests
    {

        ILifetimeScope scope;
        IReceivePortal<IBaseAuthorizationGrantedAsyncObject> portal;

        [TestInitialize]
        public void TestInitialize()
        {
            scope = AutofacContainer.GetLifetimeScope(true);
            portal = scope.Resolve<IReceivePortal<IBaseAuthorizationGrantedAsyncObject>>();
        }

        [TestMethod]
        public async Task BaseAuthorizationGrantedAsync_Create()
        {
            var obj = await portal.Create();
            var authRule = scope.Resolve<IAuthorizationGrantedAsyncRule>();
            Assert.IsTrue(authRule.ExecuteCreateCalled);
        }

        [TestMethod]
        public async Task BaseAuthorizationGrantedAsync_Create_Criteria()
        {
            var criteria = DateTime.Now.Millisecond;
            var obj = await portal.Create(criteria);
            var authRule = scope.Resolve<IAuthorizationGrantedAsyncRule>();
            Assert.IsTrue(authRule.ExecuteCreateCalled);
            Assert.AreEqual(criteria, authRule.Criteria);
        }

        [TestMethod]
        public async Task BaseAuthorizationGrantedAsync_Fetch()
        {
            var obj = await portal.Fetch();
            var authRule = scope.Resolve<IAuthorizationGrantedAsyncRule>();
            Assert.IsTrue(authRule.ExecuteFetchCalled);
        }

        [TestMethod]
        public async Task BaseAuthorizationGrantedAsync_Fetch_Criteria()
        {
            var criteria = DateTime.Now.Millisecond;
            var obj = await portal.Fetch(criteria);
            var authRule = scope.Resolve<IAuthorizationGrantedAsyncRule>();
            Assert.IsTrue(authRule.ExecuteFetchCalled);
            Assert.AreEqual(criteria, authRule.Criteria);
        }
    }
}
