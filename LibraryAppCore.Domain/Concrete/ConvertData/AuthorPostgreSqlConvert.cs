using LibraryAppCore.Domain.Abstracts;
using LibraryAppCore.Domain.Entities;
using LibraryAppCore.Domain.Entities.MsSql;
using LibraryAppCore.Domain.Pagination;
using System.Collections.Generic;

namespace LibraryAppCore.Domain.Concrete.ConvertData
{
    public class AuthorPostgreSqlConvert : IConvertDataHelper<AuthorPostgreSql, Author>
    {
        private List<AuthorPostgreSql> Authotrs = new List<AuthorPostgreSql>();
        private Author FormatedAuthors = new Author();
        private List<Author> ListAuthor = new List<Author>();
        private IEnumerable<Author> AuthorsEnum = null;
        private PagedResults<Author> PagedResultFormated = new PagedResults<Author>();

        public void InitData(PagedResults<AuthorPostgreSql> authors)
        {
            Authotrs = authors.Results;
            initPaginationInfo(authors);
        }

        public void InitData(List<AuthorPostgreSql> authors)
        {
            Authotrs = authors;
        }

        private void initPaginationInfo(PagedResults<AuthorPostgreSql> authorsPagInfo)
        {
            PagedResultFormated.PageNumber = authorsPagInfo.PageNumber;
            PagedResultFormated.PageSize = authorsPagInfo.PageSize;
            PagedResultFormated.TotalNumberOfPages = authorsPagInfo.TotalNumberOfPages;
            PagedResultFormated.TotalNumberOfRecords = authorsPagInfo.TotalNumberOfRecords;
        }

        public PagedResults<Author> GetFormatedPagedResults()
        {
            foreach (AuthorPostgreSql aPostgreSql in Authotrs)
            {
                FormatedAuthors = new Author(aPostgreSql);
                ListAuthor.Add(FormatedAuthors);
            }

            PagedResultFormated.Results = ListAuthor;
            return PagedResultFormated;
        }

        public IEnumerable<Author> GetFormatedEnumResult()
        {
            foreach (AuthorPostgreSql aPostgreSql in Authotrs)
            {
                FormatedAuthors = new Author(aPostgreSql);
                ListAuthor.Add(FormatedAuthors);
            }

            AuthorsEnum = ListAuthor;
            return AuthorsEnum;
        }
    }
}
