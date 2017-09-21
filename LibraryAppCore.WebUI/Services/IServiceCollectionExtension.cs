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
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryAppCore.WebUI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryAppCore.WebUI.Services
{
    public static class IServiceCollectionExtension  
    {
        public static string cString { get; set; }
        public static IServiceCollection servicesCollection { get; set; }

        public static IServiceCollection AddPostgreSqlConcreate(this IServiceCollection services)
        {
            services.AddTransient<IAuthorRepository, AuthorPostgreSqlConcrete>().AddDbContext<LibraryPostgreSqlContext>();
            services.AddTransient<IBookRespository, BookPostgreSqlConcrete>().AddDbContext<LibraryPostgreSqlContext>();
            services.AddTransient<IConvertDataHelper<AuthorPostgreSql, Author>, AuthorPostgreSqlConvert>();
            services.AddTransient<IConvertDataHelper<BookPostgreSql, Book>, BookPostgreSqlConvert>();
            services.AddTransient<IDataRequired<Author>, AuthorDataRequired>();
            services.AddTransient<IDataRequired<Book>, BookDataRequired>();
            servicesCollection = services;
            return services;
        }

        public static IServiceCollection AddMongoDbConcreate(this IServiceCollection services)
        {
            services.AddTransient<IAuthorRepository, AuthorMongoDbConcrete>().AddDbContext<LibraryMongoDbContext>();
            services.AddTransient<IBookRespository, BookMongoDbConcrete>().AddDbContext<LibraryMongoDbContext>();
            services.AddTransient<IConvertDataHelper<AuthorMongoDb, Author>, AuthorMongoDbConvert>();
            services.AddTransient<IConvertDataHelper<BookMongoDb, Book>, BookMongoDbConvert>();
            services.AddTransient<IDataRequired<Author>, AuthorDataRequired>();
            services.AddTransient<IDataRequired<Book>, BookDataRequired>();
            servicesCollection = services;
            return services;
        }



    }
}
