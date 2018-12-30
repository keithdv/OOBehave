using Autofac;
using Autofac.Core;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave.Netwonsoft.Json.Test
{
    public class AutofacContractResolver : DefaultContractResolver
    {
        private readonly IServiceScope _container;

        public AutofacContractResolver(IServiceScope container)
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
