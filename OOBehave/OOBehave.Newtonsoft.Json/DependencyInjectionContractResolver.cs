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
    public class DependencyInjectionContractResolver : DefaultContractResolver
    {
        private readonly IServiceScope _container;

        public DependencyInjectionContractResolver(IServiceScope container)
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

    }


}
