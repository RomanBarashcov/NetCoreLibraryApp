using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LibraryAppCore.Domain.Concrete;
using Microsoft.EntityFrameworkCore;
using LibraryAppCore.WebApi.Services;
using LibraryAppCore.Domain.Abstracts;
using LibraryAppCore.Domain.Concrete.MsSql;
using LibraryAppCore.Domain.Concrete.MongoDb;
using LibraryAppCore.Domain.Entities.MsSql;
using LibraryAppCore.Domain.Entities;
using LibraryAppCore.Domain.Concrete.ConvertData;
using LibraryAppCore.Domain.Concrete.DataRequired;
using LibraryAppCore.Domain.Entities.MondoDb;
using Microsoft.AspNetCore.Identity;
using IdentityServer4.AccessTokenValidation;

namespace LibraryAppCore.WebApi
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
            //optionsBuilder.UseNpgsql(connection);
            optionsBuilder.UseNpgsql(connection);

            services.AddDbContext<LibraryPostgreSqlContext>(options => options.UseNpgsql(connection));

            services.AddIdentity<User, IdentityRole>(opts => {
                opts.Password.RequiredLength = 6;   
                opts.Password.RequireNonAlphanumeric = false;   
                opts.Password.RequireLowercase = false; 
                opts.Password.RequireUppercase = false; 
                opts.Password.RequireDigit = false; 
            }).AddEntityFrameworkStores<LibraryPostgreSqlContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                    .AddIdentityServerAuthentication(options =>
                    {
                        options.Authority = "http://localhost:52659/"; // Auth Server address 
                        options.RequireHttpsMetadata = false;
                        options.ApiName = "library_app_core_wep_api"; // WebAPI Resource Id
                    });

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //        .AddJwtBearer(options =>
            //        {
            //            options.RequireHttpsMetadata = false;
            //            options.TokenValidationParameters = new TokenValidationParameters
            //            {
            //                ValidateIssuer = true,
            //                ValidIssuer = AuthOptions.ISSUER,
            //                ValidateAudience = true,
            //                ValidAudience = AuthOptions.AUDIENCE,
            //                ValidateLifetime = true,
            //                IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            //                ValidateIssuerSigningKey = true,
            //            };
            //        });

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

            services.AddTransient<IDataRequired<Author>, AuthorDataRequired>();
            services.AddTransient<IDataRequired<Book>, BookDataRequired>();

            services.AddTransient<IBookRepository>(provider =>
            {
                if (ConnectionDB.ConnectionString == "DefaultConnection")
                {
                    services.AddTransient<IConvertDataHelper<BookPostgreSql, Book>, BookPostgreSqlConvert>();
                    return new BookPostgreSqlConcrete(new LibraryPostgreSqlContext(optionsBuilder.Options), new BookPostgreSqlConvert(), new BookDataRequired());
                }
                else
                {
                    services.AddTransient<IConvertDataHelper<BookMongoDb, Book>, BookMongoDbConvert>();
                    return new BookMongoDbConcrete(new LibraryMongoDbContext(), new BookMongoDbConvert(), new BookDataRequired());
                }
            });

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
            }

            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}");
            }); 

        }
    }
}
