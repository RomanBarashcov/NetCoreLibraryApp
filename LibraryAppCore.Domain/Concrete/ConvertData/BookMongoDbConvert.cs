using LibraryAppCore.Domain.Abstracts;
using LibraryAppCore.Domain.Entities;
using LibraryAppCore.Domain.Entities.MondoDb;
using LibraryAppCore.Domain.Pagination;
using LibraryAppCore.Domain.QueryResultObjects.MongoDb;
using System.Collections.Generic;

namespace LibraryAppCore.Domain.Concrete.ConvertData
{
    public class BookMongoDbConvert : IConvertDataHelper<BookMongoDbQueryResult, Book>
    {
        private List<BookMongoDbQueryResult> Books = new List<BookMongoDbQueryResult>();
        private Book formatedBooks = new Book();
        private List<Book> ListBook = new List<Book>();
        private PagedResults<Book> pagedResultFormated = new PagedResults<Book>();

        public void InitData(PagedResults<BookMongoDbQueryResult> books)
        {
            Books = books.Results;
            initPaginationInfo(books);
        }

        private void initPaginationInfo(PagedResults<BookMongoDbQueryResult> booksPagInfo)
        {
            pagedResultFormated.PageNumber = booksPagInfo.PageNumber;
            pagedResultFormated.PageSize = booksPagInfo.PageSize;
            pagedResultFormated.TotalNumberOfPages = booksPagInfo.TotalNumberOfPages;
            pagedResultFormated.TotalNumberOfRecords = booksPagInfo.TotalNumberOfRecords;
        }

        public PagedResults<Book> GetFormatedPagedResults()
        {
            foreach (BookMongoDbQueryResult bMongoDb in Books)
            {
                formatedBooks = new Book(bMongoDb);
                ListBook.Add(formatedBooks);
            }

            pagedResultFormated.Results = ListBook;
            return pagedResultFormated;
        }

        public IEnumerable<Book> GetFormatedEnumResult()
        {
            throw new System.NotImplementedException();
        }

        public void InitData(List<BookMongoDbQueryResult> data)
        {
            throw new System.NotImplementedException();
        }
    }
}
