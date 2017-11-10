using LibraryAppCore.Domain.Entities.MondoDb;
using LibraryAppCore.Domain.Entities.MsSql;
using LibraryAppCore.Domain.QueryResultObjects;
using LibraryAppCore.Domain.QueryResultObjects.MongoDb;
using System;
using System.Linq;

namespace LibraryAppCore.Domain.Entities
{
    public class Book
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public string Description { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }

        private BookMongoDb BookMongoDb { get; set; }
        private BookMongoDbQueryResult BookMongoDbQueryResult { get; set; }
        private BookPostgreSqlQueryResult BookPostgreSqlQueryResult { get; set; }
        private BookPostgreSql BookPostgreSql { get; set; }

        public Book(BookPostgreSql book)
        {
            Id = Convert.ToString(book.Id);
            Name = book.Name;
            Year = book.Year;
            Description = book.Description;
            AuthorId = Convert.ToString(book.AuthorId);
            BookPostgreSql = book;
        }

        public Book(BookPostgreSqlQueryResult book)
        {
            Id = Convert.ToString(book.Id);
            Name = book.Name;
            Year = book.Year;
            Description = book.Description;
            AuthorId = Convert.ToString(book.AuthorId);
            AuthorName = book.AuthorName;
            BookPostgreSqlQueryResult = book;
        }

        public Book(BookMongoDbQueryResult book)
        {
            Id = Convert.ToString(book.Id);
            Name = book.Name;
            Year = book.Year;
            Description = book.Description;
            AuthorId = Convert.ToString(book.AuthorId);
            AuthorName = book.AuthorName;
            BookMongoDbQueryResult = book;
        }

        public Book(BookMongoDb book)
        {
            Id = book.Id;
            Year = book.Year;
            Name = book.Name;
            Description = book.Description;
            AuthorId = book.AuthorId;
            BookMongoDb = book;
        }

        public Book(IQueryable<Book> book)
        {
            foreach (var b in book)
            {
                Id = b.Id;
                Year = b.Year;
                Name = b.Name;
                Description = b.Description;
                AuthorId = b.AuthorId;
                AuthorName = b.AuthorName;
            }
        }

        public Book() { }
    }
}
