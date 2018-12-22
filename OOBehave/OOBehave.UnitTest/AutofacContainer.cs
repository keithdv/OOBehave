using Autofac;
using OOBehave.Core;
using OOBehave.Portal;
using OOBehave.Portal.Core;
using OOBehave.UnitTest.ObjectPortal;
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
                builder.RegisterType<ValidateBase.Validate>();
                builder.RegisterType<ValidateBase.ValidateAsyncRules>();
                builder.RegisterType<ValidateBase.ValidateDependencyRules>();

                builder.RegisterGeneric(typeof(PersonObjects.ShortNameDependencyRule<>));
                builder.RegisterGeneric(typeof(PersonObjects.FullNameDependencyRule<>));
                builder.RegisterGeneric(typeof(PersonObjects.PersonDependencyRule<>));


                builder.RegisterType<Objects.DisposableDependency>().As<Objects.IDisposableDependency>();
                builder.RegisterType<Objects.DisposableDependencyList>().InstancePerLifetimeScope();

                builder.RegisterType<ServiceScope>().As<IServiceScope>().InstancePerLifetimeScope();
                builder.RegisterType<ValidateBase.ParentChild>().As<ValidateBase.IParentChild>();

                builder.RegisterType<RegisteredOperationManager>().As<IRegisteredOperationManager>().SingleInstance();

                builder.RegisterGeneric(typeof(LocalReceivePortal<>)).As(typeof(IReceivePortal<>)).InstancePerLifetimeScope();
                builder.RegisterGeneric(typeof(LocalSendReceivePortal<>)).As(typeof(ISendReceivePortal<>)).InstancePerLifetimeScope();

                builder.RegisterType<ReadOnlyObject>().As<IReadOnlyObject>();
                builder.RegisterType<EditObject>().As<IEditObject>();

                builder.Register<IReadOnlyList<PersonObjects.PersonDto>>(cc =>
                {
                    return PersonObjects.PersonDto.Data();
                }).SingleInstance();

                Container = builder.Build();
            }

            return Container.BeginLifetimeScope();

        }

    }
}
