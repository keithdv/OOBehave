using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.AuthorizationRules
{

    public interface IRegisteredAuthorizationRuleManager
    {
        bool IsRegistered { get; }
        IEnumerable<IAuthorizationRule> AllAuthorizationRules { get; }
        void AddRule<R>() where R : IAuthorizationRule;
        IEnumerable<AuthorizationRuleMethod> AuthorizationRules(AuthorizeOperation operation);
    }

    /// <summary>
    /// Generic Interface so that DI gives us back a different instance per type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRegisteredAuthorizationRuleManager<T> : IRegisteredAuthorizationRuleManager
    {
    }

    public class AuthorizationRuleMethod
    {
        public MethodInfo Method { get; set; }
        public IAuthorizationRule AuthorizationRule { get; set; }

    }

    public class RegisteredAuthorizationRuleManager<T> : IRegisteredAuthorizationRuleManager<T>
        where T : IBase
    {

        private IDictionary<AuthorizeOperation, IList<AuthorizationRuleMethod>> AuthorizationMethods = new ConcurrentDictionary<AuthorizeOperation, IList<AuthorizationRuleMethod>>();
        private IProducerConsumerCollection<IAuthorizationRule> AuthorizationRuleList { get; }

        private bool IsRegistered { get; set; }
        bool IRegisteredAuthorizationRuleManager.IsRegistered => IsRegistered;

        private IServiceScope Scope { get; }

        public RegisteredAuthorizationRuleManager(IServiceScope scope)
        {
            AuthorizationRuleList = new ConcurrentBag<IAuthorizationRule>();
            Scope = scope;
            CallRegisterAuthorizationRules();
        }

        IEnumerable<IAuthorizationRule> IRegisteredAuthorizationRuleManager.AllAuthorizationRules
        {
            get
            {
                return AuthorizationRuleList.AsEnumerable();
            }
        }

        /// <summary>
        /// CT is the concrete type
        /// </summary>
        /// <typeparam name="CT"></typeparam>
        public void CallRegisterAuthorizationRules()
        {

            /// Find the AuthorizationAttribute method; if any
            var methods = typeof(T).GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic)
                .Where(m => m.GetCustomAttribute<AuthorizationRulesAttribute>() != null).ToList();

            if(methods.Count > 1)
            {
                throw new AuthorzationRulesMethodException($"Only one [{nameof(AuthorizationRulesAttribute)}] allowed per type {typeof(T).FullName}");
            }

            if (methods.Count == 1)
            {
                var method = methods.Single();

                if (!method.IsStatic)
                {
                    throw new AuthorzationRulesMethodException($"AuthorizationRules method is not static on {typeof(T).FullName}");
                }

                var parameters = method.GetParameters().ToList();

                if (parameters.Count != 1)
                {
                    throw new AuthorzationRulesMethodException($"AuthorizationRules method {typeof(T).FullName} can only have one parameter of type IRegisteredAuthorizationRuleManager");
                }

                if (parameters.Single().ParameterType != typeof(IRegisteredAuthorizationRuleManager))
                {
                    throw new AuthorzationRulesMethodException($"AuthorizationRules method {typeof(T).FullName} can only have one parameter of type IRegisteredAuthorizationRuleManager");
                }

                IsRegistered = true;

                method.Invoke(null, new object[] { this });
            }
        }

        /// <summary>
        /// Authorization rules should have no dependencies or anything on their constructor
        /// </summary>
        /// <typeparam name="R"></typeparam>
        public void AddRule<R>() where R : IAuthorizationRule
        {
            R rule;
            if (AuthorizationRuleList.TryAdd(rule = Scope.Resolve<R>()) == false)
            {
                throw new Exception("Cannot add rule to rule collection (Totally unexpected)");
            }

            IsRegistered = true;


            var methods = rule.GetType().GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic)
                .Where(m => m.GetCustomAttribute<ExecuteAttribute>() != null);

            foreach (var m in methods)
            {
                var operation = m.GetCustomAttribute<ExecuteAttribute>().AuthorizeOperation;

                if (!(m.ReturnType == typeof(Task<IAuthorizationRuleResult>)
                    || m.ReturnType == typeof(IAuthorizationRuleResult)))
                {
                    throw new AuthorizationRuleWrongTypeException($"Execute method must return IAuthorizationRuleResult or Task<IAuthorizationRuleResult> not {m.ReturnType.FullName}");
                }

                if (!AuthorizationMethods.TryGetValue(operation, out var methodInfoList))
                {
                    AuthorizationMethods.Add(operation, methodInfoList = new List<AuthorizationRuleMethod>());
                }

                methodInfoList.Add(new AuthorizationRuleMethod() { Method = m, AuthorizationRule = rule });
            }

        }

        public IEnumerable<AuthorizationRuleMethod> AuthorizationRules(AuthorizeOperation operation)
        {
            if (!AuthorizationMethods.TryGetValue(operation, out var methodInfoList))
            {
                AuthorizationMethods.Add(operation, methodInfoList = new List<AuthorizationRuleMethod>());
            }
            return methodInfoList;
        }

    }


    [Serializable]
    public class AuthorizationRuleWrongTypeException : Exception
    {
        public AuthorizationRuleWrongTypeException() { }
        public AuthorizationRuleWrongTypeException(string message) : base(message) { }
        public AuthorizationRuleWrongTypeException(string message, Exception inner) : base(message, inner) { }
        protected AuthorizationRuleWrongTypeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }


    [Serializable]
    public class AuthorzationRulesMethodException : Exception
    {
        public AuthorzationRulesMethodException() { }
        public AuthorzationRulesMethodException(string message) : base(message) { }
        public AuthorzationRulesMethodException(string message, Exception inner) : base(message, inner) { }
        protected AuthorzationRulesMethodException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
