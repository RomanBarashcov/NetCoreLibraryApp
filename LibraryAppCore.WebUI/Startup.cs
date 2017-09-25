using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LibraryAppCore.Domain.Concrete;
using Microsoft.EntityFrameworkCore;
using LibraryAppCore.WebUI.Services;
using LibraryAppCore.Domain.Abstracts;
using LibraryAppCore.Domain.Concrete.MsSql;
using LibraryAppCore.Domain.Concrete.MongoDb;
using LibraryAppCore.Domain.Entities.MsSql;
using LibraryAppCore.Domain.Entities;
using LibraryAppCore.Domain.Concrete.ConvertData;
using LibraryAppCore.Domain.Concrete.DataRequired;
using LibraryAppCore.Domain.Entities.MondoDb;

namespace LibraryAppCore_WebUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ConnectionDB.ConnectionString = "DefaultConnection";
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");
            var optionsBuilder = new DbContextOptionsBuilder<LibraryPostgreSqlContext>();
            optionsBuilder.UseNpgsql(connection);

            services.AddTransient<IAuthorRepository>(provider =>
            {
                if (ConnectionDB.ConnectionString == "DefaultConnection")
                {
                    services.AddTransient<IConvertDataHelper<AuthorPostgreSql, Author>, AuthorPostgreSqlConvert>();
                    return new AuthorPostgreSqlConcrete(new LibraryPostgreSqlContext(optionsBuilder.Options), new AuthorPostgreSqlConvert(), new AuthorDataRequired());
                }
                else
                {
                    services.AddTransient<IConvertDataHelper<AuthorMongoDb, Author>, AuthorMongoDbConvert>();
                    return new AuthorMongoDbConcrete(new LibraryMongoDbContext(), new AuthorMongoDbConvert(), new AuthorDataRequired());
                }
            });

            services.AddTransient<IBookRepository>(provider =>
            {
                if (ConnectionDB.ConnectionString == "DefaultConnection")
                {
                    services.AddScoped<IDataRequired<Book>, BookDataRequiredPSql>();
                    services.AddTransient<IConvertDataHelper<BookPostgreSql, Book>, BookPostgreSqlConvert>();
                    return new BookPostgreSqlConcrete(new LibraryPostgreSqlContext(optionsBuilder.Options), new BookPostgreSqlConvert(), new BookDataRequiredPSql());
                }
                else
                {
                    services.AddScoped<IDataRequired<Book>, BookDataRequiredMDb>();
                    services.AddTransient<IConvertDataHelper<BookMongoDb, Book>, BookMongoDbConvert>();
                    return new BookMongoDbConcrete(new LibraryMongoDbContext(), new BookMongoDbConvert(), new BookDataRequiredPSql());
                }
            });

            services.AddScoped<IDataRequired<Author>, AuthorDataRequired>();
            

            services.AddMvc(options =>
            {
                options.RespectBrowserAcceptHeader = true;
            });

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
    }
}
