using LibraryAppCore.Domain.Abstracts;
using LibraryAppCore.Domain.Entities;
using LibraryAppCore.Domain.Entities.MsSql;
using LibraryAppCore.Domain.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryAppCore.Domain.Concrete.ConvertData
{
    public class BookPostgreSqlConvert : IConvertDataHelper<BookPostgreSql, Book>
    {
        private List<BookPostgreSql> Books = new List<BookPostgreSql>();
        private Book formatedBooks = new Book();
        private List<Book> ListBooks = new List<Book>();
        private PagedResults<Book> pagedResultFormated = new PagedResults<Book>();

        public void InitData(PagedResults<BookPostgreSql> books)
        {
            Books = books.Results.ToList();
            initPaginationInfo(books);
        }

        private void initPaginationInfo(PagedResults<BookPostgreSql> booksPagInfo)
        {
            pagedResultFormated.PageNumber = booksPagInfo.PageNumber;
            pagedResultFormated.PageSize = booksPagInfo.PageSize;
            pagedResultFormated.TotalNumberOfPages = booksPagInfo.TotalNumberOfPages;
            pagedResultFormated.TotalNumberOfRecords = booksPagInfo.TotalNumberOfRecords;
        }

        public PagedResults<Book> GetFormatedPagedResults()
        {
            foreach (BookPostgreSql bPostgreSql in Books)
            {
                formatedBooks = new Book(bPostgreSql);
                ListBooks.Add(formatedBooks);
            }

            pagedResultFormated.Results = ListBooks;
            return pagedResultFormated;
        }

        public void InitData(List<BookPostgreSql> data)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Book> GetFormatedEnumResult()
        {
            throw new NotImplementedException();
        }
    }
}
