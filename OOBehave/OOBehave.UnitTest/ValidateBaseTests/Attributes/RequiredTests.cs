using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OOBehave.UnitTest.ValidateBaseTests.Attributes
{
    public interface IRequiredObject : IValidateBase { }
    public class RequiredObject : ValidateBase<RequiredObject>, IRequiredObject
    {
        public RequiredObject(IValidateBaseServices<RequiredObject> services) : base(services)
        {
        }

        [Required]
        public string StringValue { get => Getter<string>(); set => Setter(value); }

        [Required]
        public int IntValue { get => Getter<int>(); set => Setter(value); }

        [Required]
        public int? NullableValue { get => Getter<int?>(); set => Setter(value); }

        [Required]
        public List<int> ObjectValue { get => Getter<List<int>>(); set => Setter(value); }

    }

    [TestClass]
    public class RequiredAttributeTests
    {
        private ILifetimeScope scope;
        private IRequiredObject requiredObject;

        [TestInitialize]
        public void TestInitialize()
        {
            scope = AutofacContainer.GetLifetimeScope();
            requiredObject = scope.Resolve<IRequiredObject>();
        }

        [TestMethod]
        public void RequireAttribute_InValid()
        {
            Assert.IsTrue(requiredObject.IsValid);
        }

    }
}
