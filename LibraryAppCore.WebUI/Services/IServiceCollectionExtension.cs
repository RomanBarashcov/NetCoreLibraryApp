using LibraryAppCore.Domain.Abstracts;
using LibraryAppCore.Domain.Concrete;
using LibraryAppCore.Domain.Concrete.ConvertData;
using LibraryAppCore.Domain.Concrete.DataRequired;
using LibraryAppCore.Domain.Concrete.MongoDb;
using LibraryAppCore.Domain.Concrete.MsSql;
using LibraryAppCore.Domain.Entities;
using LibraryAppCore.Domain.Entities.MondoDb;
using LibraryAppCore.Domain.Entities.MsSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryAppCore.WebUI.Services
{
    public static class IServiceCollectionExtension  
    {
        public static string cString { get; set; }
        public static IServiceCollection servicesCollection { get; set; }
        public static IConfiguration Configuration { get; set; }
        public static IApplicationBuilder app { get; set; }
        public static IHostingEnvironment env { get; set; }

        public static IServiceCollection AddPostgreSqlConcreate(this IServiceCollection services)
        {
            services.AddScoped<IAuthorRepository, AuthorPostgreSqlConcrete>().AddDbContext<LibraryPostgreSqlContext>();
            services.AddScoped<IBookRepository, BookPostgreSqlConcrete>().AddDbContext<LibraryPostgreSqlContext>();
            services.AddScoped<IConvertDataHelper<AuthorPostgreSql, Author>, AuthorPostgreSqlConvert>();
            services.AddScoped<IConvertDataHelper<BookPostgreSql, Book>, BookPostgreSqlConvert>();
            services.AddScoped<IDataRequired<Author>, AuthorDataRequired>();
            services.AddScoped<IDataRequired<Book>, BookDataRequired>();
            return services;
        }

        public static IServiceCollection AddMongoDbConcreate(this IServiceCollection services)
        {
            services.AddScoped<IAuthorRepository, AuthorMongoDbConcrete>().AddDbContext<LibraryMongoDbContext>();
            services.AddScoped<IBookRepository, BookMongoDbConcrete>().AddDbContext<LibraryMongoDbContext>();
            services.AddScoped<IConvertDataHelper<AuthorMongoDb, Author>, AuthorMongoDbConvert>();
            services.AddScoped<IConvertDataHelper<BookMongoDb, Book>, BookMongoDbConvert>();
            services.AddScoped<IDataRequired<Author>, AuthorDataRequired>();
            services.AddScoped<IDataRequired<Book>, BookDataRequired>();
            return services;
        }

        public static IServiceCollection AddConcreate(this IServiceCollection services)
        {
            if(cString == "DefaultConnection")
            {
                services = AddPostgreSqlConcreate(services);
            }
            else
            {
                services.AddDbContext<LibraryMongoDbContext>();
                services = AddMongoDbConcreate(services);
            }

            services.AddScoped<IDataRequired<Author>, AuthorDataRequired>();
            services.AddScoped<IDataRequired<Book>, BookDataRequired>();

            return services;
        }



    }
}
