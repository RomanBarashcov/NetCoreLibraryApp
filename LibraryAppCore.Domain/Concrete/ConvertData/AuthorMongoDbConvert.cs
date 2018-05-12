using LibraryAppCore.Domain.Abstracts;
using LibraryAppCore.Domain.Entities;
using LibraryAppCore.Domain.Entities.MondoDb;
using LibraryAppCore.Domain.Pagination;
using System.Collections.Generic;

namespace LibraryAppCore.Domain.Concrete.ConvertData
{
    public class AuthorMongoDbConvert : IConvertDataHelper<AuthorMongoDb, Author>
    {
        private List<AuthorMongoDb> Authors = new List<AuthorMongoDb>();
        private Author FormatedAuthors = new Author();
        private List<Author> ListAuthor = new List<Author>();
        private IEnumerable<Author> AuthorsEnum = null;
        private PagedResults<Author> PagedResultFormated = new PagedResults<Author>();

        public void InitData(PagedResults<AuthorMongoDb> authors)
        {
            Authors = authors.Results;
            initPaginationInfo(authors);
        }

        public void InitData(List<AuthorMongoDb> authors)
        {
            Authors = authors;
        }

        private void initPaginationInfo(PagedResults<AuthorMongoDb> authorsPagInfo)
        {
            PagedResultFormated.PageNumber = authorsPagInfo.PageNumber;
            PagedResultFormated.PageSize = authorsPagInfo.PageSize;
            PagedResultFormated.TotalNumberOfPages = authorsPagInfo.TotalNumberOfPages;
            PagedResultFormated.TotalNumberOfRecords = authorsPagInfo.TotalNumberOfRecords;
        }

        public PagedResults<Author> GetFormatedPagedResults()
        {
            foreach (AuthorMongoDb aMongoDb in Authors)
            {
                FormatedAuthors = new Author(aMongoDb);
                ListAuthor.Add(FormatedAuthors);
            }

            PagedResultFormated.Results = ListAuthor;
            return PagedResultFormated;
        }

        public IEnumerable<Author> GetFormatedEnumResult()
        {
            foreach (AuthorMongoDb aMongoDb in Authors)
            {
                FormatedAuthors = new Author(aMongoDb);
                ListAuthor.Add(FormatedAuthors);
            }

            AuthorsEnum = ListAuthor;
            return AuthorsEnum;
        }
    }
}
