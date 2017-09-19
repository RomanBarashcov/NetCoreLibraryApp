using LibraryAppCore.Domain.Entities.MondoDb;
using LibraryAppCore.Domain.Entities.MsSql;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryAppCore.Domain.Entities
{
    public class Book
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public string Description { get; set; }
        public string AuthorId { get; set; }

        private BookMsSql bookMsSql { get; set; }
        private BookMongoDb BookMongoDb { get; set; }
        public Book(BookMsSql book)
        {
            Id = Convert.ToString(book.Id);
            Name = book.Name;
            Year = book.Year;
            Description = book.Description;
            AuthorId = Convert.ToString(book.AuthorId);
        }

        public Book(BookMongoDb book)
        {
            Id = book.Id.ToString();
            Year = book.Year;
            Name = book.Name;
            Description = book.Description;
            AuthorId = book.AuthorId;
            BookMongoDb = book;
        }

        public Book() { }
    }
}
