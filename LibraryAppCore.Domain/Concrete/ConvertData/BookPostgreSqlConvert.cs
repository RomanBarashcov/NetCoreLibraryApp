using LibraryAppCore.Domain.Abstracts;
using LibraryAppCore.Domain.Entities;
using LibraryAppCore.Domain.Entities.MsSql;
using LibraryAppCore.Domain.Pagination;
using LibraryAppCore.Domain.QueryResultObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryAppCore.Domain.Concrete.ConvertData
{
    public class BookPostgreSqlConvert : IConvertDataHelper<BookPostgreSqlQueryResult, Book>
    {
        private List<BookPostgreSqlQueryResult> Books = new List<BookPostgreSqlQueryResult>();
        private Book formatedBooks = new Book();
        private List<Book> ListBooks = new List<Book>();
        private PagedResults<Book> pagedResultFormated = new PagedResults<Book>();

        public void InitData(PagedResults<BookPostgreSqlQueryResult> books)
        {
            Books = books.Results.ToList();
            initPaginationInfo(books);
        }

        private void initPaginationInfo(PagedResults<BookPostgreSqlQueryResult> booksPagInfo)
        {
            pagedResultFormated.PageNumber = booksPagInfo.PageNumber;
            pagedResultFormated.PageSize = booksPagInfo.PageSize;
            pagedResultFormated.TotalNumberOfPages = booksPagInfo.TotalNumberOfPages;
            pagedResultFormated.TotalNumberOfRecords = booksPagInfo.TotalNumberOfRecords;
        }

        public PagedResults<Book> GetFormatedPagedResults()
        {
            foreach (BookPostgreSqlQueryResult bPostgreSql in Books)
            {
                formatedBooks = new Book(bPostgreSql);
                ListBooks.Add(formatedBooks);
            }

            pagedResultFormated.Results = ListBooks;
            return pagedResultFormated;
        }

        public IEnumerable<Book> GetFormatedEnumResult()
        {
            throw new NotImplementedException();
        }

        public void InitData(List<BookPostgreSqlQueryResult> data)
        {
            throw new NotImplementedException();
        }
    }
}
