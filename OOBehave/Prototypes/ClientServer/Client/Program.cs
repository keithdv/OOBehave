using Autofac;
using Lib;
using OOBehave;
using OOBehave.Netwonsoft.Json;
using OOBehave.Newtonsoft.Json;
using OOBehave.Portal;
using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {

        public static HttpClient httpClient = new HttpClient();

        static async Task Main(string[] args)
        {

            var builder = new ContainerBuilder();
            builder.RegisterModule(new OOBehave.Autofac.OOBehaveCoreModule(OOBehave.Autofac.Portal.Client));
            builder.RegisterType<OOBehave.Newtonsoft.Json.NewtonsoftJsonSerializer>().As<ISerializer>().SingleInstance();
            builder.RegisterType<FatClientContractResolver>();
            builder.RegisterType<ListBaseCollectionConverter>();
            builder.RegisterType<EditObject>().As<IEditObject>().AsSelf();
            var container = builder.Build();

            var scope = container.BeginLifetimeScope().Resolve<IServiceScope>();

            var portal = scope.Resolve<IReceivePortal<IEditObject>>();

            var editObject = await portal.Create("Keith", 10);

            await editObject.Save();

            //var editObject = scope.Resolve<IEditObject>();
            //editObject.Id = Guid.NewGuid();
            //editObject.Name = Guid.NewGuid().ToString();
            //editObject.Value = 10;


        }


    }
}
