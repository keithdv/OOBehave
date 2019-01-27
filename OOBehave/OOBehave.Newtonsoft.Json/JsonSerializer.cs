using Newtonsoft.Json;
using OOBehave.Netwonsoft.Json;
using OOBehave.Portal;
using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave.Newtonsoft.Json
{
    public class NewtonsoftJsonSerializer : INewtonsoftJsonSerializer, ISerializer
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

        public object Deserialize(Type type, string json)
        {
            return JsonConvert.DeserializeObject(json, type, new JsonSerializerSettings
            {
                ContractResolver = Resolver,
                TypeNameHandling = TypeNameHandling.All,
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                Converters = new List<JsonConverter>() { ListBaseCollectionConverter }
            });
        }

        public void Populate(string json, object obj)
        {
            JsonConvert.PopulateObject(json, obj, new JsonSerializerSettings
            {
                ContractResolver = Resolver,
                TypeNameHandling = TypeNameHandling.All,
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                Converters = new List<JsonConverter>() { ListBaseCollectionConverter }
            });
        }
    }
}
