using Autofac;
using OOBehave.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace OOBehave.UnitTest
{
    public static class AutofacContainer
    {


        private static IContainer Container;

        public static ILifetimeScope GetLifetimeScope()
        {

                if (Container == null)
                {
                    var builder = new ContainerBuilder();

                    builder.RegisterType<DefaultFactory>().As<IFactory>().SingleInstance();
                    builder.RegisterType<RegisteredPropertyManager>().As<IRegisteredPropertyManager>().SingleInstance();
                    builder.RegisterGeneric(typeof(RegisteredPropertyDataManager<>)).As(typeof(IRegisteredPropertyDataManager<>));
                    builder.RegisterGeneric(typeof(RegisteredPropertyValidateDataManager<>)).As(typeof(IRegisteredPropertyValidateDataManager<>));

                    builder.RegisterGeneric(typeof(BaseServices<>)).As(typeof(IBaseServices<>));
                    builder.RegisterGeneric(typeof(ValidateBaseServices<>)).As(typeof(IValidateBaseServices<>));
                    builder.RegisterGeneric(typeof(EditableBaseServices<>)).As(typeof(IEditableBaseServices<>));


                    builder.RegisterType<Base.Base>();
                    builder.RegisterType<Validate.Validate>();
                    builder.RegisterType<ValidateAsyncRules.ValidateAsyncRules>();


                    builder.RegisterGeneric(typeof(ValidateDependencyRule.ShortNameCascadeRule<>));
                    builder.RegisterGeneric(typeof(ValidateDependencyRule.FullNameCascadeRule<>));
                    builder.RegisterGeneric(typeof(ValidateDependencyRule.FirstNameTargetDependencyRule<>));
                    builder.RegisterType<ValidateDependencyRule.DisposableDependency>().As<ValidateDependencyRule.IDisposableDependency>();
                    builder.RegisterType<ValidateDependencyRule.DisposableDependencyList>().InstancePerLifetimeScope();
                    builder.RegisterType<ValidateDependencyRule.ValidateDependencyRules>();

                    
                    Container = builder.Build();
                }

                return Container.BeginLifetimeScope();

        }

    }
}
