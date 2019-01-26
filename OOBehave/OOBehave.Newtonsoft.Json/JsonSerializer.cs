using Newtonsoft.Json;
using OOBehave.Netwonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave.Newtonsoft.Json
{
    public class NewtonsoftJsonSerializer : INewtonsoftJsonSerializer
    {
        public FatClientContractResolver Resolver { get; }
        public ListBaseCollectionConverter ListBaseCollectionConverter { get; }

        public NewtonsoftJsonSerializer(FatClientContractResolver resolver, ListBaseCollectionConverter listBaseCollectionConverter)
        {
            Resolver = resolver;
            ListBaseCollectionConverter = listBaseCollectionConverter;
        }

        public string Serialize(object target)
        {
            return JsonConvert.SerializeObject(target, new JsonSerializerSettings()
            {
                ContractResolver = Resolver,
                TypeNameHandling = TypeNameHandling.All,
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                Converters = new List<JsonConverter>() { ListBaseCollectionConverter },
                Formatting = Formatting.Indented
            });
        }

        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
            {
                ContractResolver = Resolver,
                TypeNameHandling = TypeNameHandling.All,
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                Converters = new List<JsonConverter>() { ListBaseCollectionConverter }
            });
        }

    }
}
