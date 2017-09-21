using LibraryAppCore.Domain.Abstracts;
using LibraryAppCore.Domain.Entities;
using LibraryAppCore.Domain.Entities.MsSql;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryAppCore.Domain.Concrete.ConvertData
{
    public class BookPostgreSqlConvert : IConvertDataHelper<BookPostgreSql, Book>
    {
        private List<BookPostgreSql> Books = new List<BookPostgreSql>();
        private BookPostgreSql BookPostgreSql = new BookPostgreSql();
        private Book booksNode = new Book();
        private List<Book> ListBook = new List<Book>();
        private IEnumerable<Book> result = null;

        public void InitData(List<BookPostgreSql> books)
        {
            Books = books;
        }

        public IEnumerable<Book> GetIEnumerubleDbResult()
        {
            foreach (BookPostgreSql b in Books)
            {
                BookPostgreSql = new BookPostgreSql { Id = b.Id, Name = b.Name, Year = b.Year, Description = b.Description, AuthorId = b.AuthorId };
                booksNode = new Book(BookPostgreSql);
                ListBook.Add(booksNode);
                result = ListBook;
            }

            return result;
        }
    }
}
