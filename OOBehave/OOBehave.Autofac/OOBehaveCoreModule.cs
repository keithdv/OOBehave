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
using OOBehave.Rules.Rules;

namespace OOBehave.Autofac
{
    public enum Portal
    {
        NoPortal, Client,

        /// <summary>
        /// Request / Response - Scope per request expected.
        /// No management of Scope - use single scope thru entire Portal operation
        /// </summary>
        Server,

        /// <summary>
        /// Scope per unit test expected
        /// No management of Scope - use single scope thru entire Portal operation
        /// </summary>
        UnitTest, Client2Tier
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

            // Tools - More or less Static classes - but now they can be changed! (For better or worse...)
            builder.RegisterType<Core.ValuesDiffer>().As<IValuesDiffer>().SingleInstance();

            // Scope Wrapper
            builder.RegisterType<ServiceScope>().As<IServiceScope>().InstancePerLifetimeScope();
            builder.RegisterType<OOBehaveConfiguration>().As<IOOBehaveConfiguration>().SingleInstance();

            // Meta Data about the properties and methods of Classes
            // This will not change during runtime
            // So SingleInstance
            builder.RegisterGeneric(typeof(RegisteredPropertyManager<>)).As(typeof(IRegisteredPropertyManager<>)).SingleInstance();


            // This was single instance; but now it resolves the Authorization Rules 
            // When single instance it receives the root scopewhich is no good
            builder.RegisterGeneric(typeof(PortalOperationManager<>)).As(typeof(IPortalOperationManager<>)).InstancePerLifetimeScope();

            // Should not be singleinstance because AuthorizationRules can have constructor dependencies
            builder.RegisterGeneric(typeof(AuthorizationRuleManager<>)).As(typeof(IAuthorizationRuleManager<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(RuleManager<>)).As(typeof(IRuleManager<>)).AsSelf();
            builder.RegisterType<RuleResultList>().As<IRuleResultList>();
            builder.RegisterType<RequiredRule>().As<IRequiredRule>();
            builder.RegisterType<AttributeToRule>().As<IAttributeToRule>(); // SingleInstance is save as long as it only resolves Func<>s

            builder.RegisterGeneric(typeof(RegisteredProperty<>)).As(typeof(IRegisteredProperty<>));

            builder.Register<CreateRegisteredProperty>(cc =>
            {
                var scope = cc.Resolve<Func<ILifetimeScope>>();
                return (propertyInfo) =>
                {
                    return (IRegisteredProperty)scope().Resolve(typeof(IRegisteredProperty<>).MakeGenericType(propertyInfo.PropertyType), new TypedParameter(typeof(System.Reflection.PropertyInfo), propertyInfo));
                };
            });

            builder.RegisterGeneric(typeof(PropertyValue<>)).As(typeof(IPropertyValue<>));

            builder.Register<CreatePropertyValue>(cc =>
            {
                var scope = cc.Resolve<Func<ILifetimeScope>>();
                return (IRegisteredProperty property, object value) =>
                {
                    return (IPropertyValue)scope().Resolve(typeof(IPropertyValue<>).MakeGenericType(property.Type), new NamedParameter("name", property.Name), new NamedParameter("value", value));
                };
            });

            builder.RegisterGeneric(typeof(ValidatePropertyValue<>)).As(typeof(IValidatePropertyValue<>));
            builder.Register<CreateValidatePropertyValue>(cc =>
            {
                var scope = cc.Resolve<Func<ILifetimeScope>>();
                return (IRegisteredProperty property, object value) =>
                {
                    return (IValidatePropertyValue)scope().Resolve(typeof(IValidatePropertyValue<>).MakeGenericType(property.Type), new NamedParameter("name", property.Name), new NamedParameter("value", value));
                };
            });

            builder.RegisterGeneric(typeof(EditPropertyValue<>)).As(typeof(IEditPropertyValue<>));
            builder.Register<CreateEditPropertyValue>(cc =>
            {
                var scope = cc.Resolve<Func<ILifetimeScope>>();
                return (IRegisteredProperty property, object value) =>
                {
                    return (IEditPropertyValue)scope().Resolve(typeof(IEditPropertyValue<>).MakeGenericType(property.Type), new NamedParameter("name", property.Name), new NamedParameter("value", value));
                };
            });

            builder.Register<GetPortalOperationManager>(cc =>
            {
                var scope = cc.Resolve<Func<ILifetimeScope>>();
                return (Type t) => (IPortalOperationManager)scope().Resolve(typeof(IPortalOperationManager<>).MakeGenericType(t));
            });

            // Stored values for each Domain Object instance
            // MUST BE per instance
            builder.RegisterGeneric(typeof(PropertyValueManager<>)).As(typeof(IPropertyValueManager<>)).AsSelf();
            builder.RegisterGeneric(typeof(ValidatePropertyValueManager<>)).As(typeof(IValidatePropertyValueManager<>)).AsSelf();
            builder.RegisterGeneric(typeof(EditPropertyValueManager<>)).As(typeof(IEditPropertyValueManager<>)).AsSelf();

            builder.RegisterType<Zip>().As<IZip>().SingleInstance();

            if (Portal == Portal.Server || Portal == Portal.UnitTest)
            {
                // Takes IServiceScope so these need to match it's lifetime
                builder.RegisterGeneric(typeof(ServerReceivePortal<>))
                    .As(typeof(IReceivePortal<>))
                    .As(typeof(IReceivePortalChild<>))
                    .InstancePerLifetimeScope();

                builder.RegisterGeneric(typeof(ServerSendReceivePortal<>))
                    .As(typeof(ISendReceivePortal<>))
                    .As(typeof(ISendReceivePortalChild<>))
                    .InstancePerLifetimeScope();

                builder.RegisterGeneric(typeof(ServerMethodPortal<>)).As(typeof(IRemoteMethodPortal<>)).AsSelf();

                builder.RegisterType<Server>().As<IServer>();

            }
            else if (Portal == Portal.Client2Tier)
            {

                // Takes IServiceScope so these need to match it's lifetime
                builder.RegisterGeneric(typeof(Client2TierReceivePortal<>))
                    .As(typeof(IReceivePortal<>))
                    .As(typeof(IReceivePortalChild<>))
                    .InstancePerLifetimeScope();

                builder.RegisterGeneric(typeof(Client2TierSendReceivePortal<>))
                    .As(typeof(ISendReceivePortal<>))
                    .As(typeof(ISendReceivePortalChild<>))
                    .InstancePerLifetimeScope();

                // For now
                builder.RegisterGeneric(typeof(Client2TierMethodPortal<>)).As(typeof(IRemoteMethodPortal<>)).AsSelf();
            }
            else if (Portal == Portal.Client)
            {
                builder.RegisterGeneric(typeof(ClientReceivePortal<>))
                    .As(typeof(IReceivePortal<>))
                    .As(typeof(IReceivePortalChild<>))
                    .InstancePerLifetimeScope();

                builder.RegisterGeneric(typeof(ClientSendReceivePortal<>))
                    .As(typeof(ISendReceivePortal<>))
                    .As(typeof(ISendReceivePortalChild<>))
                    .InstancePerLifetimeScope();

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
