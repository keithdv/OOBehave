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
        private static AsyncLocal<ILifetimeScope> Scopes = new AsyncLocal<ILifetimeScope>();

        public static ILifetimeScope GetLifetimeScope()
        {
            if (Scopes.Value == null)
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


                    Container = builder.Build();
                }

                Scopes.Value = Container.BeginLifetimeScope();

            }

            return Scopes.Value;

        }

        public static T Resolve<T>()
        {
            return GetLifetimeScope().Resolve<T>();
        }

    }
}
