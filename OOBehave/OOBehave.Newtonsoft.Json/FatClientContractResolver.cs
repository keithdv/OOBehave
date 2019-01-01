using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OOBehave.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace OOBehave.Netwonsoft.Json
{
    /// <summary>
    /// The goal of this class is to 
    /// A) Use Autofac to resolve the instances
    /// B) Change the json for RichClient requests
    /// </summary>
    public class FatClientContractResolver : DefaultContractResolver
    {
        private readonly IServiceScope _container;

        public FatClientContractResolver(IServiceScope container)
        {
            _container = container;
        }

        protected override JsonObjectContract CreateObjectContract(Type objectType)
        {

            // use Autofac to create types that have been registered with it
            if (_container.IsRegistered(objectType))
            {
                JsonObjectContract contract = base.CreateObjectContract(_container.ConcreteType(objectType));
                contract.DefaultCreator = () => _container.Resolve(objectType);

                return contract;
            }

            return base.CreateObjectContract(objectType);
        }


        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (member.GetCustomAttribute<PortalDataMemberAttribute>() != null)
            {
                property.Ignored = false;
                property.Readable = true;
                property.Writable = true;
                property.HasMemberAttribute = true;
            }
            else if (member.DeclaringType.GetCustomAttribute<PortalDataContractAttribute>() != null)
            {
                property.Ignored = true;
            }

            return property;
        }

        protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        {

            if (objectType.GetCustomAttribute<PortalDataContractAttribute>() != null)
            {

                var members = objectType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy)
                    .Where(p => p.GetCustomAttribute<PortalDataMemberAttribute>() != null)
                    .Cast<MemberInfo>()
                    .ToList();



                members.AddRange(objectType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy)
                    .Where(p => p.GetCustomAttribute<PortalDataMemberAttribute>() != null)
                    .Cast<MemberInfo>()
                    .ToList());

                return members;

            }


            return base.GetSerializableMembers(objectType); ;


            //if (cache.ContainsKey(objectType))
            //{
            //    return cache[objectType];
            //}
            //else if (objectType.GetCustomAttribute<DataContractAttribute>() != null)
            //{
            //    var members = objectType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            //        .Where(p => p.GetCustomAttribute<DataMemberAttribute>() != null)
            //        .Cast<MemberInfo>()
            //        .ToList();
            //    cache.Add(objectType, members);
            //    return members;
            //}
            //else
            //{
            //    return base.GetSerializableMembers(objectType);
            //}
        }

    }


}
