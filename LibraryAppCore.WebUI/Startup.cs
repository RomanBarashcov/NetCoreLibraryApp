using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LibraryAppCore.Domain.Concrete;
using Microsoft.EntityFrameworkCore;
using LibraryAppCore.Domain.Abstracts;
using LibraryAppCore.Domain.Concrete.MsSql;
using LibraryAppCore.Domain.Concrete.ConvertData;
using LibraryAppCore.Domain.Entities.MsSql;
using LibraryAppCore.Domain.Entities;
using LibraryAppCore.Domain.Concrete.DataRequired;
using LibraryAppCore.WebUI.Services;

namespace LibraryAppCore_WebUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IServiceCollection servicesCollection;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<LibraryContext>(options => options.UseSqlServer(connection, b => b.MigrationsAssembly("LibraryAppCore.WebUI")));

            //DI
            services.AddConnection("MongoDbConnection");
            //
            services.AddMvc(options =>
            {
                options.RespectBrowserAcceptHeader = true;
            });

            servicesCollection = services;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" });


                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
                
            });
            

        }
        public void Conf(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                if (context.Request.Query["ConnectionStrong"] == "MongoDbConnection")
                {
                    servicesCollection.AddConnection("MongoDbConnection");
                }
                else
                {
                    servicesCollection.AddConnection("DefaultConnection");
                    
                }
            });
        }
    }
}
