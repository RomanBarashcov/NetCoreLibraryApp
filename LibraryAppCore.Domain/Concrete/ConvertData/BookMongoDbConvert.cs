using LibraryAppCore.Domain.Abstracts;
using LibraryAppCore.Domain.Entities;
using LibraryAppCore.Domain.Entities.MondoDb;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryAppCore.Domain.Concrete.ConvertData
{
    public class BookMongoDbConvert : IConvertDataHelper<BookMongoDb, Book>
    {
        private List<BookMongoDb> Books = new List<BookMongoDb>();
        private BookMongoDb BookMongoDB = new BookMongoDb();
        private Book booksNode = new Book();
        private List<Book> ListBook = new List<Book>();
        private IEnumerable<Book> result = null;

        public void InitData(List<BookMongoDb> books)
        {
            Books = books;
        }

        public IEnumerable<Book> GetIEnumerubleDbResult()
        {
            foreach (BookMongoDb b in Books)
            {
                BookMongoDB = new BookMongoDb { Id = b.Id, Name = b.Name, Year = b.Year, Description = b.Description, AuthorId = b.AuthorId };
                booksNode = new Book(BookMongoDB);
                ListBook.Add(booksNode);
                result = ListBook;
            }

            return result;
        }
    }
}
