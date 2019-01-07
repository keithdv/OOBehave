using Autofac;
using OOBehave.Rules;
using System;
using System.Linq;
using System.Reflection;

namespace OOBehave.Autofac
{
    public static class AutoRegisterTypesExtension
    {

        /// <summary>
        /// Auto register every type that has a corresponding interface linked by name in the same namespace
        /// Example MyObject will be linked to IMyObject 
        /// If it is a rule with no constructor parameters it will be registered as single instance
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="assembly"></param>
        public static void AutoRegisterAssemblyTypes(this ContainerBuilder builder, Assembly assembly)
        {

            var types = assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract).ToList();
            var interfaces = assembly.GetTypes().Where(t => t.IsInterface).ToDictionary(x => x.FullName);

            foreach (var t in types)
            {
                if (interfaces.TryGetValue($"{t.Namespace}.I{t.Name}", out var i))
                {
                    var singleConstructor = t.GetConstructors().SingleOrDefault();
                    var zeroConstructorParams = singleConstructor != null && !singleConstructor.GetParameters().Any();


                    if (!t.IsGenericType)
                    {
                        // AsSelf required for Deserialization
                        var reg = builder.RegisterType(t).As(i).AsSelf();

                        // If it is a RULE
                        // and has zero constructor parameters
                        // assume no dependencies
                        // so it can be SingleInstance
                        if (typeof(IRule).IsAssignableFrom(t) && zeroConstructorParams)
                        {
                            reg.SingleInstance();
                        }
                    }
                    else
                    {

                        // AsSelf required for Deserialization
                        // If it is a RULE
                        // and has zero constructor parameters
                        // assume no dependencies
                        // so it can be SingleInstance
                        var reg = builder.RegisterGeneric(t).As(i).AsSelf();
                        if (typeof(IRule).IsAssignableFrom(t) && zeroConstructorParams)
                        {
                            reg.SingleInstance();
                        }
                    }
                }
            }
        }

    }
}
