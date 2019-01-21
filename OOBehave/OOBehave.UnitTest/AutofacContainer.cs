using Autofac;
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
using OOBehave.UnitTest.Portal;
using OOBehave.Autofac;
using System.Reflection;

namespace OOBehave.UnitTest
{

    public static class AutofacContainer
    {


        private static IContainer Container;
        private static IContainer LocalPortalContainer;
        private static object lockContainer = new object();

        public static ILifetimeScope GetLifetimeScope(bool localPortal = false)
        {
            lock (lockContainer)
            {
                if (Container == null)
                {

                    IContainer CreateContainer(Autofac.Portal portal)
                    {
                        var builder = new ContainerBuilder();

                        builder.RegisterModule(new OOBehave.Autofac.OOBehaveCoreModule(portal));


                        if (portal == Autofac.Portal.NoPortal)
                        {
                            builder.RegisterGeneric(typeof(MockReceivePortal<>)).As(typeof(IReceivePortal<>)).AsSelf().InstancePerLifetimeScope();
                            builder.RegisterGeneric(typeof(MockReceivePortalChild<>)).As(typeof(IReceivePortalChild<>)).AsSelf().InstancePerLifetimeScope();
                            builder.RegisterGeneric(typeof(MockSendReceivePortal<>)).As(typeof(ISendReceivePortal<>)).AsSelf().InstancePerLifetimeScope();
                            builder.RegisterGeneric(typeof(MockSendReceivePortalChild<>)).As(typeof(ISendReceivePortalChild<>)).AsSelf().InstancePerLifetimeScope();
                        }


                        builder.AutoRegisterAssemblyTypes(Assembly.GetExecutingAssembly());

                        // Unit Test Library
                        builder.RegisterType<BaseTests.Authorization.AuthorizationGrantedRule>().As<BaseTests.Authorization.IAuthorizationGrantedRule>().InstancePerLifetimeScope(); // Not normal - Lifetimescope so the results can be validated
                        builder.RegisterType<BaseTests.Authorization.AuthorizationGrantedAsyncRule>().As<BaseTests.Authorization.IAuthorizationGrantedAsyncRule>().InstancePerLifetimeScope(); // Not normal - Lifetimescope so the results can be validated
                        builder.RegisterType<BaseTests.Authorization.AuthorizationGrantedDependencyRule>().As<BaseTests.Authorization.IAuthorizationGrantedDependencyRule>().InstancePerLifetimeScope(); // Not normal - Lifetimescope so the results can be validated

                        builder.RegisterType<Objects.ConstructorDisposableDependency>().As<Objects.IConstructorDisposableDependency>();
                        builder.RegisterType<Objects.ConstructorDisposableDependencyList>().InstancePerLifetimeScope();

                        builder.RegisterType<Objects.PortalOperationDisposableDependency>().As<Objects.IPortalOperationDisposableDependency>();
                        builder.RegisterType<Objects.PortalOperationDisposableDependencyList>().InstancePerLifetimeScope();

                        builder.Register<MethodObject.Execute>(cc =>
                        {
                            var dd = cc.Resolve<Func<Objects.IPortalOperationDisposableDependency>>();
                            return i => MethodObject.ExecuteServer(i, dd());
                        });

                        builder.Register<IReadOnlyList<PersonObjects.PersonDto>>(cc =>
                        {
                            return PersonObjects.PersonDto.Data();
                        }).SingleInstance();

                        return builder.Build();
                    }

                    Container = CreateContainer(Autofac.Portal.NoPortal);
                    LocalPortalContainer = CreateContainer(Autofac.Portal.Local);

                }

                if (!localPortal)
                {
                    return Container.BeginLifetimeScope("Target");
                }
                else
                {
                    return LocalPortalContainer.BeginLifetimeScope("Target");
                }
            }
        }

    }
}
