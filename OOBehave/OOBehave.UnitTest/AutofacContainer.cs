using Autofac;
using OOBehave.AuthorizationRules;
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

                // SingleInstance as long it is isn't modified to accept dependencies
                builder.RegisterType<DefaultFactory>().As<IFactory>().SingleInstance();

                // Scope Wrapper
                builder.RegisterType<ServiceScope>().As<IServiceScope>().InstancePerLifetimeScope();

                // Meta Data about the properties and methods of Classes
                // This will not change during runtime
                // So SingleInstance
                builder.RegisterType<RegisteredOperationManager>().As<IRegisteredOperationManager>().SingleInstance();
                builder.RegisterType<RegisteredPropertyManager>().As<IRegisteredPropertyManager>().SingleInstance();


                // Should not be singleinstance because AuthorizationRules have constructor dependencies
                builder.RegisterGeneric(typeof(RegisteredAuthorizationRuleManager<>)).As(typeof(IRegisteredAuthorizationRuleManager<>)).InstancePerLifetimeScope();

                // Stored values for each class instance
                // MUST BE per instance
                builder.RegisterGeneric(typeof(RegisteredPropertyDataManager<>)).As(typeof(IRegisteredPropertyDataManager<>));
                builder.RegisterGeneric(typeof(RegisteredPropertyValidateDataManager<>)).As(typeof(IRegisteredPropertyValidateDataManager<>));

                // Takes IServiceScope so these need to match it's lifetime
                builder.RegisterGeneric(typeof(LocalReceivePortal<>)).As(typeof(IReceivePortal<>)).InstancePerLifetimeScope();
                builder.RegisterGeneric(typeof(LocalSendReceivePortal<>)).As(typeof(ISendReceivePortal<>)).InstancePerLifetimeScope();

                // Simple wrapper - Always InstancePerDependency
                builder.RegisterGeneric(typeof(BaseServices<>)).As(typeof(IBaseServices<>));
                builder.RegisterGeneric(typeof(ValidateBaseServices<>)).As(typeof(IValidateBaseServices<>));
                builder.RegisterGeneric(typeof(EditableBaseServices<>)).As(typeof(IEditableBaseServices<>));

                // Unit Test Library
                builder.RegisterType<Base.Base>();
                builder.RegisterType<Base.Authorization.BaseAuthorizationObject>().As<Base.Authorization.IBaseAuthorizationObject>();
                builder.RegisterType<Base.Authorization.BaseAuthorizationAsyncObject>().As<Base.Authorization.IBaseAuthorizationAsyncObject>();
                builder.RegisterType<Base.Authorization.AuthorizationGrantedRule>().InstancePerLifetimeScope(); // Not normal - Lifetimescope so the results can be validated
                builder.RegisterType<Base.Authorization.AuthorizationGrantedAsyncRule>().InstancePerLifetimeScope(); // Not normal - Lifetimescope so the results can be validated

                builder.RegisterType<ValidateBase.Validate>();
                builder.RegisterType<ValidateBase.ValidateAsyncRules>();
                builder.RegisterType<ValidateBase.ValidateDependencyRules>();

                // Rules can be InstancePerLifetimeScope -Maybe even single instance
                // Depends on their dependencies
                // So it up to the developer - not controlled by OOBehave
                builder.RegisterGeneric(typeof(PersonObjects.ShortNameDependencyRule<>)).InstancePerLifetimeScope();
                builder.RegisterGeneric(typeof(PersonObjects.FullNameDependencyRule<>)).InstancePerLifetimeScope();
                builder.RegisterGeneric(typeof(PersonObjects.PersonDependencyRule<>)).InstancePerLifetimeScope();

                builder.RegisterType<Objects.DisposableDependency>().As<Objects.IDisposableDependency>();
                builder.RegisterType<Objects.DisposableDependencyList>().InstancePerLifetimeScope();

                builder.RegisterType<ValidateBase.ParentChild>().As<ValidateBase.IParentChild>();

                builder.RegisterType<ReadOnlyObject>().As<IReadOnlyObject>();
                builder.RegisterType<EditObject>().As<IEditObject>();

                builder.Register<IReadOnlyList<PersonObjects.PersonDto>>(cc =>
                {
                    return PersonObjects.PersonDto.Data();
                }).SingleInstance();

                Container = builder.Build();
            }

            return Container.BeginLifetimeScope(Guid.NewGuid());

        }

    }
}
