using LibraryAppCore.Domain.Abstracts;
using System;
using System.Collections.Generic;
using LibraryAppCore.Domain.Entities;
using System.Threading.Tasks;
using LibraryAppCore.Domain.Entities.MsSql;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using LibraryAppCore.Domain.Pagination;

namespace LibraryAppCore.Domain.Concrete.MsSql
{
    public class AuthorPostgreSqlConcrete : IAuthorRepository
    {
        private PagedResults<Author> result = null;
        private IConvertDataHelper<AuthorPostgreSql, Author> PostgreSqlDataConvert;
        private IDataRequired<Author> dataReqiered;
        private LibraryPostgreSqlContext db;
        private IPagination<AuthorPostgreSql> pagination;

        public AuthorPostgreSqlConcrete(LibraryPostgreSqlContext context, IConvertDataHelper<AuthorPostgreSql, Author> pSqlDataConvert, IDataRequired<Author> dReqiered, IPagination<AuthorPostgreSql> pagin)
        {
            this.db = context;
            this.PostgreSqlDataConvert = pSqlDataConvert;
            this.dataReqiered = dReqiered;
            this.pagination = pagin;
        }

        public async Task<PagedResults<Author>> GetAllAuthors(int page, int pageSize, string orderBy, bool ascending)
        {
            IQueryable<AuthorPostgreSql> authorsQueryResult = db.Authors.AsQueryable();

            PagedResults<AuthorPostgreSql> authorPagedResult = await pagination.CreatePagedResultsAsync(authorsQueryResult, page, pageSize, orderBy, ascending);

            if (authorPagedResult != null)
            {
                PostgreSqlDataConvert.InitData(authorPagedResult);
                result = PostgreSqlDataConvert.GetFormatedPagedResults();
            }

            return result;
        }

        public async Task<IEnumerable<Author>> GetAllAuthors()
        {
            List<AuthorPostgreSql> authorListResult = await db.Authors.ToListAsync();
            IEnumerable<Author> auhorsEnumResult = null;

            if (authorListResult != null)
            {
                PostgreSqlDataConvert.InitData(authorListResult);
                auhorsEnumResult = PostgreSqlDataConvert.GetFormatedEnumResult();
            }

            return auhorsEnumResult;
        }

        public async Task<Author> GetAuthorById(string authorId)
        {
            Author Author = new Author();

            if (!String.IsNullOrEmpty(authorId))
            {
                AuthorPostgreSql authorDbResult = await db.Authors.FindAsync(authorId);

                if(authorDbResult != null)
                {
                    Author.Id = authorDbResult.Id.ToString();
                    Author.Name = authorDbResult.Name;
                    Author.Surname = authorDbResult.Surname;
                }
                
            }

            return Author;
        }

        public async Task<string> GetAuthorIdByName(string firstName, string surName)
        {
            string authorId = "";
            List<AuthorPostgreSql> author = await db.Authors.Where(a => a.Name == firstName && (a.Surname == surName)).ToListAsync();

            foreach (AuthorPostgreSql a in author)
            {
                authorId = Convert.ToString(a.Id);
                break;
            }

            return authorId;
        }


        public async Task<int> CreateAuthor(Author author)
        {
            int DbResult = 0;

            if (dataReqiered.IsDataNoEmpty(author))
            {
                AuthorPostgreSql newAuthor = new AuthorPostgreSql { Name = author.Name, Surname = author.Surname };

                db.Authors.Add(newAuthor);
                try
                {
                    DbResult = await db.SaveChangesAsync();
                }
                catch
                {
                    return DbResult;
                }
            }

            return DbResult;
        }

        public async Task<int> UpdateAuthor(string authorId, Author author)
        {
            int DbResult = 0;

            if (!String.IsNullOrEmpty(authorId) && dataReqiered.IsDataNoEmpty(author))
            {
                int oldDataAuthorId = Convert.ToInt32(authorId);
                int newDataAuthorId = Convert.ToInt32(author.Id);
                AuthorPostgreSql updatingAuthor = null;

                updatingAuthor = await db.Authors.FindAsync(oldDataAuthorId);

                if (oldDataAuthorId == newDataAuthorId)
                {
                    updatingAuthor.Name = author.Name;
                    updatingAuthor.Surname = author.Surname;

                    db.Entry(updatingAuthor).State = EntityState.Modified;

                    try
                    {
                        DbResult = await db.SaveChangesAsync();
                    }
                    catch
                    {
                        return DbResult;
                    }
                }
            }

            return DbResult;
        }

        public async Task<int> DeleteAuthor(string authorId)
        {
            int DbResult = 0;

            if (!String.IsNullOrEmpty(authorId))
            {
                int delAuthorId = Convert.ToInt32(authorId);

                AuthorPostgreSql author = await db.Authors.FindAsync(delAuthorId);

                if (author != null)
                {
                    db.Authors.Remove(author);
                    DbResult = await db.SaveChangesAsync();
                }
            }

            return DbResult;
        }

    }
}
