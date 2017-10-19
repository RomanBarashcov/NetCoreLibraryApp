using LibraryAppCore.Domain.Entities.MondoDb;
using LibraryAppCore.Domain.Entities.PostgreSql;
using System;

namespace LibraryAppCore.Domain.Entities
{
    public class Book
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public string Language { get; set; }
        public string Binding { get; set; }
        public int Weight { get; set; }
        public int Pages { get; set; }
        public string Subscription { get; set; }
        public string Price { get; set; }
        public string Description { get; set; }
        public byte[] ImageBook { get; set; }
        public string AuthorId { get; set; }

        private BookPostgreSql bookPostgreSql { get; set; }
        private BookMongoDb BookMongoDb { get; set; }

        public Book(BookPostgreSql book)
        {
            Id = Convert.ToString(book.Id);
            Name = book.Name;
            Year = book.Year;
            Language = book.Language;
            Binding = book.Binding;
            Pages = book.Pages;
            Weight = book.Weight;

            if(!DBNull.Value.Equals(book.Subscription))
                Subscription = Convert.ToString(book.Subscription);
            if (!DBNull.Value.Equals(book.Price))
                Subscription = Convert.ToString(book.Price);

            Description = book.Description;
            ImageBook = book.ImageBook;
            AuthorId = Convert.ToString(book.AuthorId);
            bookPostgreSql = book;
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

        public Book() { }
    }
}
