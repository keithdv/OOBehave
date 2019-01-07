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

namespace OOBehave.Autofac
{
    public enum Portal
    {
        NoPortal, Client, Local
    }

    public class OOBehaveCoreModule : Module
    {

        public OOBehaveCoreModule(Portal portal)
        {
            Portal = portal;
        }

        public Portal Portal { get; }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

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
            builder.RegisterGeneric(typeof(RuleExecute<>)).As(typeof(IRuleExecute<>)).AsSelf();

            // Stored values for each Domain Object instance
            // MUST BE per instance
            builder.RegisterGeneric(typeof(PropertyValueManager<>)).As(typeof(IPropertyValueManager<>)).AsSelf();
            builder.RegisterGeneric(typeof(ValidatePropertyValueManager<>)).As(typeof(IValidatePropertyValueManager<>)).AsSelf();
            builder.RegisterGeneric(typeof(EditPropertyValueManager<>)).As(typeof(IEditPropertyValueManager<>)).AsSelf();

            if (Portal == Portal.Local)
            {
                // Takes IServiceScope so these need to match it's lifetime
                builder.RegisterGeneric(typeof(LocalReceivePortal<>))
                    .As(typeof(IReceivePortal<>))
                    .As(typeof(IReceivePortalChild<>))
                    .InstancePerLifetimeScope();

                builder.RegisterGeneric(typeof(LocalSendReceivePortal<>))
                    .As(typeof(ISendReceivePortal<>))
                    .As(typeof(ISendReceivePortalChild<>))
                    .InstancePerLifetimeScope();

                builder.RegisterGeneric(typeof(LocalMethodPortal<>)).As(typeof(IRemoteMethodPortal<>)).AsSelf();
            }

            // Simple wrapper - Always InstancePerDependency
            builder.RegisterGeneric(typeof(BaseServices<>)).As(typeof(IBaseServices<>));
            builder.RegisterGeneric(typeof(ListBaseServices<,>)).As(typeof(IListBaseServices<,>));
            builder.RegisterGeneric(typeof(ValidateBaseServices<>)).As(typeof(IValidateBaseServices<>));
            builder.RegisterGeneric(typeof(ValidateListBaseServices<,>)).As(typeof(IValidateListBaseServices<,>));
            builder.RegisterGeneric(typeof(EditBaseServices<>)).As(typeof(IEditBaseServices<>));
            builder.RegisterGeneric(typeof(EditListBaseServices<,>)).As(typeof(IEditListBaseServices<,>));

        }
    }

}
