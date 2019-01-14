using Autofac;
using Castle.DynamicProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.ValidateBaseTests
{
    namespace AddInterfaceImplementation
    {

        /// <summary>
        /// When a getter or setter is called on the Interface
        /// Call the same method on the Target based on the assumption that it's there
        /// </summary>
        /// <typeparam name="T"></typeparam>
        internal class GetInterceptor<T> : IInterceptor
            where T : IValidateBase
        {

            public GetInterceptor(T target)
            {
                RegisterMethods();
                Target = target;
            }

            private IDictionary<string, MethodInfo> Methods { get; } = new Dictionary<string, MethodInfo>();
            public T Target { get; }

            private static Type[] ooBehaveTypes = new Type[] { typeof(Base<>), typeof(ListBase<,>), typeof(ValidateBase<>), typeof(ValidateListBase<,>), typeof(EditBase<>), typeof(EditListBase<,>) };

            private void RegisterMethods()
            {
                var type = typeof(T);

                // If a type does a 'new' on the property you will have duplicate MethodNames
                // So honor to top-level type that has that MethodName

                do
                {
                    var methods = type.GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.SetProperty).ToList();

                    foreach (var m in methods)
                    {
                        // This is a pretty simple approach
                        // Doesn't account for overloaded methods
                        // Honestly, the second there is any complicated factors the class should inherit the interface
                        if (!Methods.ContainsKey(m.Name))
                        {
                            Methods.Add(m.Name, m);
                        }
                    }

                    type = type.BaseType;

                } while (type != null && (!type.IsGenericType || !ooBehaveTypes.Contains(type.GetGenericTypeDefinition())));
            }

            public void Intercept(IInvocation invocation)
            {

                var methodName = invocation.Method.Name;

                if (Methods.TryGetValue(methodName, out var method))
                {
                    invocation.ReturnValue = method.Invoke(Target, invocation.Arguments);
                } else
                {
                    throw new Exception($"Method or Property {methodName} not found on {typeof(T).FullName}");
                }

            }
        }

        public class RuleFix<T>
            where T : IValidateBase
        {
            private ProxyGenerator _generator = new ProxyGenerator();

            public IShortNameRuleTarget Fix(T item)
            {
                return (IShortNameRuleTarget)_generator.CreateInterfaceProxyWithoutTarget(typeof(IShortNameRuleTarget), new GetInterceptor<T>(item));
            }
        }


        public interface IShortNameRuleTarget
        {
            string FirstName { get; }
            string LastName { get; }
            string ShortName { get; set; }
            void ToUpperCase();
        }

        public class ShortNameRule : RuleBase<IShortNameRuleTarget>
        {

            public override IRuleResult Execute(IShortNameRuleTarget target)
            {

                if (string.IsNullOrWhiteSpace(target.FirstName))
                {
                    return (IRuleResult)RuleResult.PropertyError(nameof(IShortNameRuleTarget.FirstName), "FirstName is required");
                }

                if (string.IsNullOrWhiteSpace(target.LastName))
                {
                    return (IRuleResult)RuleResult.PropertyError(nameof(IShortNameRuleTarget.LastName), "LastName is required");
                }

                target.ShortName = $"{target.FirstName} {target.LastName}";

                // No support for overloaded methods
                target.ToUpperCase();

                return (IRuleResult)RuleResult.Empty();

            }

        }

        // Key : Neither IValidateObject or ValidateObject implement IShortNameRuleTarget
        public interface IValidateObject : IValidateBase
        {
            string FirstName { get; set; }
            string LastName { get; set; }
            string ShortName { get; set; }
        }

        public class ValidateObject : ValidateBase<ValidateObject>, IValidateObject
        {
            public ValidateObject(IValidateBaseServices<ValidateObject> services) : base(services)
            {
            }

            public string FirstName { get => Getter<string>(); set => Setter(value); }
            public string LastName { get => Getter<string>(); set => Setter(value); }
            public string ShortName { get => Getter<string>(); set => Setter(value); }

            public void ToUpperCase()
            {
                FirstName = FirstName.ToUpper();
                LastName = LastName.ToUpper();
                ShortName = ShortName.ToUpper();
            }
        }


        [TestClass]
        public class AddInterfaceImplementationSandbox
        {
            private ILifetimeScope scope;

            [TestInitialize]
            public void TestInitialize()
            {
                scope = AutofacContainer.GetLifetimeScope();
            }
            [TestMethod]

            public async Task AddInterfaceImplementationSandbox_Test()
            {
                var validateObject = scope.Resolve<ValidateObject>();
                var rule = new ShortNameRule();

                // In practice this will be the concrete type
                var ruleFix = new RuleFix<ValidateObject>();

                validateObject.FirstName = "Keith";
                validateObject.LastName = "Voels";

                await rule.Execute(ruleFix.Fix(validateObject), new CancellationToken());

                Assert.AreEqual("Keith Voels".ToUpper(), validateObject.ShortName);
            }

        }
    }
}
