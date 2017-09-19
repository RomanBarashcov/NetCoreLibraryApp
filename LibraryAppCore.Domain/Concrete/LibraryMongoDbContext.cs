using LibraryAppCore.Domain.Entities.MondoDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryAppCore.Domain.Concrete
{
    public class LibraryMongoDbContext : DbContext
    {
        MongoClient client;
        IMongoDatabase database;

        public LibraryMongoDbContext() : base()
        {
            string connectionString = "mongodb://localhost:27017/Library";
            var connection = new MongoUrlBuilder(connectionString);
            MongoClient client = new MongoClient(connectionString);
            database = client.GetDatabase(connection.DatabaseName);

        }

        public IMongoCollection<AuthorMongoDb> Authors
        {
            get { return database.GetCollection<AuthorMongoDb>("Author"); }
        }

        public IMongoCollection<BookMongoDb> Books
        {
            get { return database.GetCollection<BookMongoDb>("Book"); }
        }
    }
}
