using LibraryAppCore.Domain.Abstracts;
using LibraryAppCore.Domain.Entities;
using LibraryAppCore.Domain.Entities.MsSql;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryAppCore.Domain.Concrete.ConvertData
{
    public class BookMsSqlConvert : IConvertDataHelper<BookMsSql, Book>
    {
        private List<BookMsSql> Books = new List<BookMsSql>();
        private BookMsSql BookMssql = new BookMsSql();
        private Book booksNode = new Book();
        private List<Book> ListBook = new List<Book>();
        private IEnumerable<Book> result = null;

        public void InitData(List<BookMsSql> books)
        {
            Books = books;
        }

        public IEnumerable<Book> GetIEnumerubleDbResult()
        {
            foreach (BookMsSql b in Books)
            {
                BookMssql = new BookMsSql { Id = b.Id, Name = b.Name, Year = b.Year, Description = b.Description, AuthorId = b.AuthorId };
                booksNode = new Book(BookMssql);
                ListBook.Add(booksNode);
                result = ListBook;
            }

            return result;
        }
    }
}
