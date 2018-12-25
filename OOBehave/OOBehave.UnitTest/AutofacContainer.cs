﻿using Autofac;
using Autofac.Core;
using OOBehave.AuthorizationRules;
using OOBehave.Core;
using OOBehave.Portal;
using OOBehave.Portal.Core;
using OOBehave.UnitTest.ObjectPortal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;
using Autofac.Builder;
using OOBehave.Rules;

namespace OOBehave.UnitTest
{

    public class UnitTestModule : Module
    {

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);


            var types = ThisAssembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract).ToList();
            var interfaces = ThisAssembly.GetTypes().Where(t => t.IsInterface).ToDictionary(x => x.Name);

            foreach (var t in types)
            {
                if (interfaces.TryGetValue($"I{t.Name}", out var i))
                {
                    var singleConstructor = t.GetConstructors().SingleOrDefault();
                    var zeroConstructorParams = singleConstructor != null && !singleConstructor.GetParameters().Any();


                    if (!t.IsGenericType)
                    {
                        var reg = builder.RegisterType(t).As(i);

                        // If it is a RULE
                        // and has zero constructor parameters
                        // assume no dependencies
                        // so it can be SingleInstance
                        if (typeof(IRule).IsAssignableFrom(t) && zeroConstructorParams)
                        {
                            reg.SingleInstance();
                        }
                        else
                        {

                        }
                    }
                    else
                    {


                        // If it is a RULE
                        // and has zero constructor parameters
                        // assume no dependencies
                        // so it can be SingleInstance
                        var reg = builder.RegisterGeneric(t).As(i);
                        if (typeof(IRule).IsAssignableFrom(t) && zeroConstructorParams)
                        {
                            reg.SingleInstance();
                        }
                        else
                        {

                        }
                    }
                }
            }
        }
    }

    public static class AutofacContainer
    {


        private static IContainer Container;

        public static ILifetimeScope GetLifetimeScope()
        {

            if (Container == null)
            {
                var builder = new ContainerBuilder();

                // Run first - some of these definition need to be modified
                builder.RegisterModule<UnitTestModule>();


                // SingleInstance as long it is isn't modified to accept dependencies
                builder.RegisterType<DefaultFactory>().As<IFactory>().SingleInstance();

                // Tools - More or less Static classes - but now they can be changed! (For better or worse...)
                builder.RegisterType<Core.ValuesDiffer>().As<IValuesDiffer>().SingleInstance();

                // Scope Wrapper
                builder.RegisterType<ServiceScope>().As<IServiceScope>().InstancePerLifetimeScope();

                // Meta Data about the properties and methods of Classes
                // This will not change during runtime
                // So SingleInstance
                builder.RegisterGeneric(typeof(RegisteredPropertyManager<>)).As(typeof(IRegisteredPropertyManager<>)).SingleInstance();


                // This was single instance; but now it resolves the Authorization Rules 
                // When single instance it receives the root scopewhich is no good
                builder.RegisterGeneric(typeof(PortalOperationManager<>)).As(typeof(IPortalOperationManager<>)).InstancePerLifetimeScope();

                // Should not be singleinstance because AuthorizationRules can have constructor dependencies
                builder.RegisterGeneric(typeof(AuthorizationRuleManager<>)).As(typeof(IAuthorizationRuleManager<>)).InstancePerLifetimeScope();

                // Stored values for each Domain Object instance
                // MUST BE per instance
                builder.RegisterGeneric(typeof(PropertyValueManager<>)).As(typeof(IPropertyValueManager<>));
                builder.RegisterGeneric(typeof(ValidatePropertyValueManager<>)).As(typeof(IValidatePropertyValueManager<>));
                builder.RegisterGeneric(typeof(EditPropertyValueManager<>)).As(typeof(IEditPropertyValueManager<>));

                // Takes IServiceScope so these need to match it's lifetime
                builder.RegisterGeneric(typeof(LocalReceivePortal<>)).As(typeof(IReceivePortal<>)).InstancePerLifetimeScope();
                builder.RegisterGeneric(typeof(LocalSendReceivePortal<>)).As(typeof(ISendReceivePortal<>)).InstancePerLifetimeScope();

                // Simple wrapper - Always InstancePerDependency
                builder.RegisterGeneric(typeof(BaseServices<>)).As(typeof(IBaseServices<>));
                builder.RegisterGeneric(typeof(ValidateBaseServices<>)).As(typeof(IValidateBaseServices<>));
                builder.RegisterGeneric(typeof(EditableBaseServices<>)).As(typeof(IEditableBaseServices<>));

                // Unit Test Library
                builder.RegisterType<Base.Authorization.AuthorizationGrantedRule>().As<Base.Authorization.IAuthorizationGrantedRule>().InstancePerLifetimeScope(); // Not normal - Lifetimescope so the results can be validated
                builder.RegisterType<Base.Authorization.AuthorizationGrantedAsyncRule>().As<Base.Authorization.IAuthorizationGrantedAsyncRule>().InstancePerLifetimeScope(); // Not normal - Lifetimescope so the results can be validated
                builder.RegisterType<Base.Authorization.AuthorizationGrantedDependencyRule>().As<Base.Authorization.IAuthorizationGrantedDependencyRule>().InstancePerLifetimeScope(); // Not normal - Lifetimescope so the results can be validated

                builder.RegisterType<Objects.DisposableDependency>().As<Objects.IDisposableDependency>();
                builder.RegisterType<Objects.DisposableDependencyList>().InstancePerLifetimeScope();


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
