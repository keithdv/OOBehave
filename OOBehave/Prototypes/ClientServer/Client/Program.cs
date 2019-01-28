using Autofac;
using Lib;
using Microsoft.Extensions.Configuration;
using OOBehave;
using OOBehave.Netwonsoft.Json;
using OOBehave.Newtonsoft.Json;
using OOBehave.Portal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
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
            Dictionary<string, string> config = new Dictionary<string, string>() { { "OOBehave:PortalURL", "http://localhost:52985/api/portal" } };
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddInMemoryCollection(config);

            var builder = new ContainerBuilder();
            builder.RegisterModule(new OOBehave.Autofac.OOBehaveCoreModule(OOBehave.Autofac.Portal.Client));
            builder.RegisterType<OOBehave.Newtonsoft.Json.NewtonsoftJsonSerializer>().As<ISerializer>().SingleInstance();
            builder.RegisterType<FatClientContractResolver>();
            builder.RegisterType<ListBaseCollectionConverter>();

            builder.RegisterType<EditObject>().As<IEditObject>().AsSelf();
            builder.RegisterType<EditObjectList>().As<IEditObjectList>().AsSelf();

            builder.RegisterInstance<IConfiguration>(configurationBuilder.Build());

            var container = builder.Build();

            var scope = container.BeginLifetimeScope().Resolve<IServiceScope>();

            var portal = scope.Resolve<IReceivePortal<IEditObject>>();

            var editObject = await portal.Create("Keith", 10);

            var child = await editObject.Children.CreateAdd();

            Debug.Assert(!editObject.IsValid);
            Debug.Assert(editObject.IsSelfValid);
            //Debug.Assert(!editObject.Child.IsValid);
            //Debug.Assert(!editObject.Child.IsSelfValid);
            //Debug.Assert(!editObject.Child.PropertyIsValid[nameof(IEditObject.Name)]);
            //Debug.Assert(!editObject.Child.PropertyIsValid[nameof(IEditObject.Value)]);

            child.Name = "Bill";
            child.Value = 10;

            child = await editObject.Children.CreateAdd("John", 10);

            Debug.Assert(await editObject.IsSavableAsync());

            await editObject.Save();

            if (editObject.IsModified) { throw new Exception("Failure: EditObject IsModified true"); }
            if (!editObject.Id.HasValue) { throw new Exception("Failure: ID is null"); }

            var id = editObject.Id;
            editObject.Name = Guid.NewGuid().ToString();
            child = editObject.Children.First();
            var childId = child.Id;
            child.Name = Guid.NewGuid().ToString();

            await editObject.Save();

            if (editObject.IsModified) { throw new Exception("Failure: EditObject IsModified true"); }
            if (id == editObject.Id) { throw new Exception("Failure: EditObject Update failed"); }
            if (childId == editObject.Children.First().Id) { throw new Exception("Failur: EditObject Child Update Failed."); }
            //editObject.Delete();
            //await editObject.Save();
            //Debug.Assert(!editObject.Id.HasValue);

            //var listPortal = scope.Resolve<IReceivePortal<IEditObjectList>>();

            //var list = await listPortal.Create();

            //await list.CreateAdd("Keith", 10);
            //await list.CreateAdd("John", 10);

            //await list.Save();
        }


    }
}
