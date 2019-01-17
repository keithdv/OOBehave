using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOBehave.UnitTest.PersonObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace OOBehave.UnitTest.ValidateBaseTests
{

    [TestClass]
    public class ValidateDataBindingTests
    {
        private ILifetimeScope scope;
        private IValidateAsyncObject validate;
        private IValidateAsyncObject child;

        [TestInitialize]
        public void TestInitailize()
        {
            scope = AutofacContainer.GetLifetimeScope();

            validate = scope.Resolve<IValidateAsyncObject>();
            child = scope.Resolve<IValidateAsyncObject>();
            validate.Child = child;
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Assert.IsFalse(validate.IsBusy);
            Assert.IsFalse(validate.IsSelfBusy);
        }

        [TestMethod]
        public async Task ValidateDataBinding_AsyncRule_PropertyHasChanged()
        {
            validate.FirstName = "Error"; ;

            await validate.CheckAllRules();
            Assert.IsFalse(validate.IsValid);

            // Really tricky situation
            // If the rule is Async IDataErrorInfo is checked too early by WPF because we've returned
            // We need to ensure that we call PropertyHasChanged again once the property is marked valid or invalid

            List<string> propertyChanged = new List<string>();

            void ValidateDataBindingTests_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == nameof(IValidateAsyncObject.FirstName))
                {
                    propertyChanged.Add(e.PropertyName);
                }
            }

            ((INotifyPropertyChanged)validate).PropertyChanged += ValidateDataBindingTests_PropertyChanged;

            validate.FirstName = "Keith";

            Assert.AreEqual(1, propertyChanged.Count);
            await validate.WaitForRules();
            Assert.AreEqual(2, propertyChanged.Count);

            Assert.IsTrue(validate.IsValid);


        }


    }
}
