//using Autofac;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using OOBehave.AuthorizationRules;
//using OOBehave.Portal;
//using OOBehave.UnitTest.Objects;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;

//namespace OOBehave.UnitTest.Base.Authorization
//{

//    public class AuthorizationGrantedDependencyRule : AuthorizationRule
//    {
//        public int Criteria { get; set; }
//        public bool ExecuteCreateCalled { get; set; }
//        private IDisposableDependency DisposableDependency { get; }

//        public AuthorizationGrantedDependencyRule(IDisposableDependency disposableDependency)
//        {
//            DisposableDependency = disposableDependency;
//        }

//        [Execute(AuthorizeOperation.Create)]
//        public IAuthorizationRuleResult ExecuteCreate()
//        {
//            ExecuteCreateCalled = true;
//            return AuthorizationRuleResult.AccessGranted();
//        }

//        [Execute(AuthorizeOperation.Create)]
//        public IAuthorizationRuleResult ExecuteCreate(int criteria)
//        {
//            ExecuteCreateCalled = true;
//            Criteria = criteria;
//            return AuthorizationRuleResult.AccessGranted();
//        }

//        public bool ExecuteFetchCalled { get; set; }
//        [Execute(AuthorizeOperation.Fetch)]
//        public IAuthorizationRuleResult ExecuteFetch()
//        {
//            ExecuteFetchCalled = true;
//            return AuthorizationRuleResult.AccessGranted();
//        }

//        [Execute(AuthorizeOperation.Fetch)]
//        public IAuthorizationRuleResult ExecuteFetch(int criteria)
//        {
//            ExecuteFetchCalled = true;
//            Criteria = criteria;
//            return AuthorizationRuleResult.AccessGranted();
//        }

//        public bool ExecuteUpdateCalled { get; set; }
//        [Execute(AuthorizeOperation.Update)]
//        public IAuthorizationRuleResult ExecuteUpdate()
//        {
//            ExecuteUpdateCalled = true;
//            return AuthorizationRuleResult.AccessGranted();
//        }

//        public bool ExecuteDeleteCalled { get; set; }
//        [Execute(AuthorizeOperation.Delete)]
//        public IAuthorizationRuleResult ExecuteDelete()
//        {
//            ExecuteDeleteCalled = true;
//            return AuthorizationRuleResult.AccessGranted();
//        }
//    }

//    public interface IBaseAuthorizationGrantedDependencyObject : IBase { }

//    public class BaseAuthorizationGrantedDependencyObject : Base<BaseAuthorizationGrantedDependencyObject>, IBaseAuthorizationGrantedDependencyObject
//    {

//        public BaseAuthorizationGrantedDependencyObject(IBaseServices<BaseAuthorizationGrantedDependencyObject> services) : base(services)
//        {

//        }

//        [AuthorizationRules]
//        public static void RegisterAuthorizationRules(IAuthorizationRuleManager authorizationRuleManager)
//        {
//            authorizationRuleManager.AddRule<AuthorizationGrantedDependencyRule>();
//        }

//        [Create]
//        public void Create(int criteria) { }

//        [Fetch]
//        private void Fetch() { }

//        [Fetch]
//        public void Fetch(int criteria) { }

//    }

//    [TestClass]
//    public class BaseAuthorizationGrantedDependencyTests
//    {

//        ILifetimeScope scope;
//        IReceivePortal<IBaseAuthorizationGrantedDependencyObject> portal;

//        [TestInitialize]
//        public void TestInitialize()
//        {
//            scope = AutofacContainer.GetLifetimeScope();
//            portal = scope.Resolve<IReceivePortal<IBaseAuthorizationGrantedDependencyObject>>();
//        }

//        [TestMethod]
//        public async Task BaseAuthorization_Create()
//        {
//            var obj = await portal.Create();
//            var authRule = scope.Resolve<AuthorizationGrantedDependencyRule>();
//            Assert.IsTrue(authRule.ExecuteCreateCalled);
//        }

//        [TestMethod]
//        public async Task BaseAuthorization_Create_Criteria()
//        {
//            var criteria = DateTime.Now.Millisecond;
//            var obj = await portal.Create(criteria);
//            var authRule = scope.Resolve<AuthorizationGrantedDependencyRule>();
//            Assert.IsTrue(authRule.ExecuteCreateCalled);
//            Assert.AreEqual(criteria, authRule.Criteria);
//        }

//        [TestMethod]
//        public async Task BaseAuthorization_Fetch()
//        {
//            var obj = await portal.Fetch();
//            var authRule = scope.Resolve<AuthorizationGrantedDependencyRule>();
//            Assert.IsTrue(authRule.ExecuteFetchCalled);
//        }

//        [TestMethod]
//        public async Task BaseAuthorization_Fetch_Criteria()
//        {
//            var criteria = DateTime.Now.Millisecond;
//            var obj = await portal.Fetch(criteria);
//            var authRule = scope.Resolve<AuthorizationGrantedDependencyRule>();
//            Assert.IsTrue(authRule.ExecuteFetchCalled);
//            Assert.AreEqual(criteria, authRule.Criteria);
//        }
//    }
//}
