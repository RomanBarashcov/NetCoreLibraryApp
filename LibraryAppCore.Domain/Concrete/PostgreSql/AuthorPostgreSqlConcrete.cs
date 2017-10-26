using LibraryAppCore.Domain.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;
using LibraryAppCore.Domain.Entities;
using System.Threading.Tasks;
using LibraryAppCore.Domain.Entities.MsSql;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LibraryAppCore.Domain.Concrete.MsSql
{
    public class AuthorPostgreSqlConcrete : IAuthorRepository
    {
        private IEnumerable<Author> result = null;
        private IConvertDataHelper<AuthorPostgreSql, Author> PostgreSqlDataConvert;
        private IDataRequired<Author> dataReqiered;
        private LibraryPostgreSqlContext db;

        public AuthorPostgreSqlConcrete(LibraryPostgreSqlContext context, IConvertDataHelper<AuthorPostgreSql, Author> pSqlDataConvert, IDataRequired<Author> dReqiered)
        {
            this.db = context;
            this.PostgreSqlDataConvert = pSqlDataConvert;
            this.dataReqiered = dReqiered;
        }

        public async Task<IEnumerable<Author>> GetAllAuthors()
        {
            List<AuthorPostgreSql> AuthorList = await db.Authors.ToListAsync();

            if (AuthorList != null)
            {
                PostgreSqlDataConvert.InitData(AuthorList);
                result = PostgreSqlDataConvert.GetIEnumerubleDbResult();
            }
            return result;
        }

        public async Task<int> GetAuthorIdByFullName(string firstName, string lastName)
        {
            int authorId = 0;
            List<AuthorPostgreSql> author = await db.Authors.Where(a => a.Name == firstName && (a.Surname == lastName)).ToListAsync();
            foreach(AuthorPostgreSql a in author)
            {
                authorId = a.Id;
            }
            return authorId;
        }

        public async Task<Author> GetAuthorById(string authorId)
        {
            Author Author = new Author();

            if (!String.IsNullOrEmpty(authorId))
            {
                AuthorPostgreSql authorDbResult = await db.Authors.FindAsync(authorId);

                Author.Id = authorDbResult.Id.ToString();
                Author.Name = authorDbResult.Name;
                Author.Surname = authorDbResult.Surname;
            }
            return Author;
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
