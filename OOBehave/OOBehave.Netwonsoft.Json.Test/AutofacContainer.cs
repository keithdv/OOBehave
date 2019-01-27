using Autofac;
using Autofac.Core;
using OOBehave.AuthorizationRules;
using OOBehave.Core;
using OOBehave.Portal;
using OOBehave.Portal.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;
using Autofac.Builder;
using OOBehave.Rules;
using OOBehave.Netwonsoft.Json.Test.BaseTests;
using OOBehave.Netwonsoft.Json.Test.EditTests;
using OOBehave.Netwonsoft.Json.Test.ValidateTests;
using OOBehave.Newtonsoft.Json;
using OOBehave.Autofac;
using System.Reflection;

namespace OOBehave.Netwonsoft.Json.Test
{

    public static class AutofacContainer
    {

        private static IContainer Container;

        public static ILifetimeScope GetLifetimeScope()
        {

            if (Container == null)
            {
                var builder = new ContainerBuilder();

                // Run first - some of these definition need to be modified
                builder.RegisterModule(new OOBehaveCoreModule(Autofac.Portal.Server));

                builder.AutoRegisterAssemblyTypes(Assembly.GetExecutingAssembly());

                // Newtonsoft.Json
                builder.RegisterType<FatClientContractResolver>();
                builder.RegisterType<ListBaseCollectionConverter>();

                builder.RegisterType<NewtonsoftJsonSerializer>().As<INewtonsoftJsonSerializer>().As<ISerializer>();

                builder.RegisterType<DisposableDependencyList>();
                builder.RegisterType<DisposableDependency>().As<IDisposableDependency>().InstancePerLifetimeScope();

                builder.Register<MethodObject.CommandMethod>(cc =>
                {
                    var dd = cc.Resolve<Func<IDisposableDependency>>();
                    return i => MethodObject.CommendMethod_(i, dd());
                });

                builder.RegisterGeneric(typeof(RemoteMethodCall<,,>)).As(typeof(IRemoteMethod<,,>)).AsSelf();
                builder.RegisterType<MethodObject>();

                Container = builder.Build();
            }

            return Container.BeginLifetimeScope(Guid.NewGuid());

        }

    }
}
