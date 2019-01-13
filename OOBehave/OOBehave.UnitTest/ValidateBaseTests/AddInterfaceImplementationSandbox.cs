using Castle.DynamicProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOBehave.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OOBehave.UnitTest.ValidateBaseTests
{
    namespace AddInterfaceImplementation
    {



        public interface IShortNameRule
        {
            string FirstName { get; }
            string LastName { get; }
            string ShortName { get; set; }
        }

        public class ShortNameRule : RuleBase<IShortNameRule>
        {

            public override IRuleResult Execute(IShortNameRule target)
            {

                if (string.IsNullOrWhiteSpace(target.FirstName))
                {
                    return RuleResult.PropertyError(nameof(IShortNameRule.FirstName), "FirstName is required");
                }

                if (string.IsNullOrWhiteSpace(target.LastName))
                {
                    return RuleResult.PropertyError(nameof(IShortNameRule.LastName), "LastName is required");
                }

                target.ShortName = $"{target.FirstName} {target.LastName}";

                return RuleResult.Empty();

            }

        }

        public interface IValidateObject
        {

        }

        public class ValidateObject : ValidateBase<ValidateObject>, IValidateObject
        {
            public ValidateObject(IValidateBaseServices<ValidateObject> services) : base(services)
            {
            }

            public string FirstName { get => Getter<string>(); set => Setter(value); }
            public string LastName { get => Getter<string>(); set => Setter(value); }
            public string FullName { get => Getter<string>(); set => Setter(value); }

        }


        [TestClass]
        public class AddInterfaceImplementationSandbox
        {
            [TestMethod]

            public void AddInterfaceImplementationSandbox_Test()
            {
                var t = typeof(ValidateObject);

                t.
            }

        }
    }
}
