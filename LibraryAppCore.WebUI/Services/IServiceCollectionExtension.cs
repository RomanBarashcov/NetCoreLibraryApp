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

namespace LibraryAppCore.WebUI.Services
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddMsSqlConcreate(this IServiceCollection services)
        {
            services.AddTransient<IAuthorRepository, AuthorMsSqlConcrete>().AddDbContext<LibraryContext>();
            services.AddTransient<IBookRespository, BookMsSqlConcrete>().AddDbContext<LibraryContext>();
            services.AddTransient<IConvertDataHelper<AuthorMsSql, Author>, AuthorMsSqlConvert>();
            services.AddTransient<IConvertDataHelper<BookMsSql, Book>, BookMsSqlConvert>();
            services.AddTransient<IDataRequired<Author>, AuthorDataRequired>();
            services.AddTransient<IDataRequired<Book>, BookDataRequired>();
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
            return services;
        }

        public static IServiceCollection AddConnection(this IServiceCollection services, string cString)
        {
            if (cString == "DefaultConnection")
            {
                services.AddTransient<IAuthorRepository, AuthorMsSqlConcrete>().AddDbContext<LibraryContext>();
                services.AddTransient<IBookRespository, BookMsSqlConcrete>().AddDbContext<LibraryContext>();
                services.AddTransient<IConvertDataHelper<AuthorMsSql, Author>, AuthorMsSqlConvert>();
                services.AddTransient<IConvertDataHelper<BookMsSql, Book>, BookMsSqlConvert>();
                services.AddTransient<IDataRequired<Author>, AuthorDataRequired>();
                services.AddTransient<IDataRequired<Book>, BookDataRequired>();
            }
            else
            {
                services.AddTransient<IAuthorRepository, AuthorMongoDbConcrete>().AddDbContext<LibraryMongoDbContext>();
                services.AddTransient<IBookRespository, BookMongoDbConcrete>().AddDbContext<LibraryMongoDbContext>();
                services.AddTransient<IConvertDataHelper<AuthorMongoDb, Author>, AuthorMongoDbConvert>();
                services.AddTransient<IConvertDataHelper<BookMongoDb, Book>, BookMongoDbConvert>();
                services.AddTransient<IDataRequired<Author>, AuthorDataRequired>();
                services.AddTransient<IDataRequired<Book>, BookDataRequired>();
            }
            return services;
        }
    }
}
