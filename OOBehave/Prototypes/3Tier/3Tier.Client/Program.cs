using _3Tier.Lib;
using Autofac;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace _3Tier.Client
{
    class Program
    {

        public static HttpClient httpClient = new HttpClient();

        static async Task Main(string[] args)
        {

            var builder = new ContainerBuilder();

            builder.RegisterType<OOBehave.Newtonsoft.Json.ListBaseCollectionConverter>();
            builder.RegisterType<OOBehave.Newtonsoft.Json.ListBaseSurrogate>();
            builder.RegisterType<OOBehave.Newtonsoft.Json.NewtonsoftJsonSerializer>();

            var container = builder.Build();

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, "http://localhost:53051/api/Portal");
            //httpRequest.Content = new ByteArrayContent(serialized);

            var httpResponse = await httpClient.SendAsync(httpRequest);
            httpResponse.EnsureSuccessStatusCode();
            //serialized = await httpResponse.Content.ReadAsByteArrayAsync();


            Console.WriteLine("Hello World!");
        }
    }
}
