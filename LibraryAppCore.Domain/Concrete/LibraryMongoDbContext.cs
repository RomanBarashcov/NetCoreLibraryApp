using LibraryAppCore.Domain.Entities.MondoDb;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MongoDB.Driver;

namespace LibraryAppCore.Domain.Concrete
{
    public class LibraryMongoDbContext : IdentityDbContext
    {
        MongoClient client;
        IMongoDatabase database;

        public LibraryMongoDbContext() : base()
        {
            string connectionString = "mongodb://localhost:27017/Library";
            var connection = new MongoUrlBuilder(connectionString);
            client = new MongoClient(connectionString);
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
