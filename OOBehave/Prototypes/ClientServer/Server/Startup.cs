using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Lib;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OOBehave.Portal;

namespace Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you. If you
        // need a reference to the container, you need to use the
        // "Without ConfigureContainer" mechanism shown later.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<OOBehave.Newtonsoft.Json.NewtonsoftJsonSerializer>().As<ISerializer>();
            builder.RegisterType<OOBehave.Newtonsoft.Json.ListBaseCollectionConverter>();
            builder.RegisterType<OOBehave.Netwonsoft.Json.FatClientContractResolver>();
            builder.RegisterType<Zip>().As<IZip>().SingleInstance();

            builder.RegisterGeneric(typeof(Server<>));

            builder.RegisterType<EditObject>().As<IEditObject>().AsSelf();
            builder.RegisterModule(new OOBehave.Autofac.OOBehaveCoreModule(OOBehave.Autofac.Portal.Server));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
