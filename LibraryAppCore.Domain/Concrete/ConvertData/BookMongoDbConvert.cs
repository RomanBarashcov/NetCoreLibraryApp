using LibraryAppCore.Domain.Abstracts;
using LibraryAppCore.Domain.Entities;
using LibraryAppCore.Domain.Entities.MondoDb;
using LibraryAppCore.Domain.Pagination;
using System.Collections.Generic;

namespace LibraryAppCore.Domain.Concrete.ConvertData
{
    public class BookMongoDbConvert : IConvertDataHelper<BookMongoDb, Book>
    {
        private List<BookMongoDb> Books = new List<BookMongoDb>();
        private Book formatedBooks = new Book();
        private List<Book> ListBook = new List<Book>();
        private PagedResults<Book> pagedResultFormated = new PagedResults<Book>();

        public void InitData(PagedResults<BookMongoDb> books)
        {
            Books = books.Results;
            initPaginationInfo(books);
        }

        private void initPaginationInfo(PagedResults<BookMongoDb> booksPagInfo)
        {
            pagedResultFormated.PageNumber = booksPagInfo.PageNumber;
            pagedResultFormated.PageSize = booksPagInfo.PageSize;
            pagedResultFormated.TotalNumberOfPages = booksPagInfo.TotalNumberOfPages;
            pagedResultFormated.TotalNumberOfRecords = booksPagInfo.TotalNumberOfRecords;
        }

        public PagedResults<Book> GetFormatedPagedResults()
        {
            foreach (BookMongoDb bMongoDb in Books)
            {
                formatedBooks = new Book(bMongoDb);
                ListBook.Add(formatedBooks);
            }

            pagedResultFormated.Results = ListBook;
            return pagedResultFormated;
        }

        public void InitData(List<BookMongoDb> data)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Book> GetFormatedEnumResult()
        {
            throw new System.NotImplementedException();
        }
    }
}
