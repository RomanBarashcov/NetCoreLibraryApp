using LibraryAppCore.Domain.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;
using LibraryAppCore.Domain.Entities;
using System.Threading.Tasks;
using LibraryAppCore.Domain.Entities.MsSql;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using LibraryAppCore.Domain.Pagination;

namespace LibraryAppCore.Domain.Concrete.MsSql
{
    public class BookPostgreSqlConcrete : IBookRepository
    {
        private PagedResults<Book> result = null;
        private IConvertDataHelper<BookPostgreSql, Book> PostgreSqlDataConvert;
        private IDataRequired<Book> dataReqiered;
        private LibraryPostgreSqlContext db;
        private IPagination<BookPostgreSql> pagination;

        public BookPostgreSqlConcrete(LibraryPostgreSqlContext context, IConvertDataHelper<BookPostgreSql, Book> pSqlDataConvert, IDataRequired<Book> dReqiered, IPagination<BookPostgreSql> pagin)
        {
            this.db = context;
            this.PostgreSqlDataConvert = pSqlDataConvert;
            this.dataReqiered = dReqiered;
            this.pagination = pagin;
        }

        public async Task<PagedResults<Book>> GetAllBooks(int page, int pageSize, string orderBy, bool ascending)
        {
            IQueryable<BookPostgreSql> BookQuery =  db.Books.AsQueryable();

            PagedResults<BookPostgreSql> BookPagedResult = await pagination.CreatePagedResultsAsync(BookQuery, page, pageSize, orderBy, ascending);

            if (BookPagedResult != null)
            {
                PostgreSqlDataConvert.InitData(BookPagedResult);
                result = PostgreSqlDataConvert.GetFormatedPagedResults();
            }

            return result;
        }

        public async Task<int> CreateBook(Book book)
        {
            int DbResult = 0;
            int authorId = 0;

            if (dataReqiered.IsDataNoEmpty(book))
            {
                bool isNewBook = await CheckingBookOnDuplicate(book);

                if (isNewBook)
                {
                    authorId = Convert.ToInt32(book.AuthorId);
                    BookPostgreSql newBook = new BookPostgreSql { Name = book.Name, Description = book.Description, Year = book.Year, AuthorId = authorId };

                    db.Books.Add(newBook);

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

        private async Task<bool> CheckingBookOnDuplicate(Book book)
        {
            int authorId = 0;
            bool isNewBook = false;

            if (dataReqiered.IsDataNoEmpty(book))
            {
                authorId = Convert.ToInt32(book.AuthorId);

                var createdBook = await db.Books.Where(b => b.AuthorId == authorId && (b.Name == book.Name && b.Description == book.Description)).ToListAsync();

                if (createdBook.Count > 0)
                {
                    isNewBook = false;
                }
                else
                {
                    isNewBook = true;
                }
            }

            return isNewBook;
        }

        public async Task<int> UpdateBook(string id, Book book)
        {
            int DbResult = 0;
            int oldDataBookId, newDataBookId, authorId = 0;

            if (!String.IsNullOrEmpty(id) && dataReqiered.IsDataNoEmpty(book))
            {
                try
                {
                    oldDataBookId = Convert.ToInt32(id);
                    newDataBookId = Convert.ToInt32(book.Id);
                    authorId = Convert.ToInt32(book.AuthorId);
                }
                catch
                {
                    return DbResult;
                }

                BookPostgreSql updatingBook = null;
                updatingBook = await db.Books.FindAsync(oldDataBookId);

                if (oldDataBookId == newDataBookId)
                {
                    updatingBook.Year = book.Year;
                    updatingBook.Name = book.Name;
                    updatingBook.Description = book.Description;
                    updatingBook.AuthorId = authorId;

                    db.Entry(updatingBook).State = EntityState.Modified;

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

        public async Task<int> DeleteBook(string id)
        {
            int DbResult = 0;
            BookPostgreSql book = null;

            if (!String.IsNullOrEmpty(id))
            {
                int delBookId = Convert.ToInt32(id);
                book = db.Books.Find(delBookId);

                if (book != null)
                {
                    db.Books.Remove(book);
                    DbResult = await db.SaveChangesAsync();
                }
            }

            return DbResult;
        }

        public async Task<PagedResults<Book>> GetBookByAuthorId(string authorId, int page, int pageSize, string orderBy, bool ascending)
        {
            if (!String.IsNullOrEmpty(authorId))
            {
                int author_Id = Convert.ToInt32(authorId);

                IQueryable<BookPostgreSql> bookQueryResult = db.Books.Where(x => x.AuthorId == author_Id).AsQueryable();

                PagedResults<BookPostgreSql> bookPagedResult = await pagination.CreatePagedResultsAsync(bookQueryResult, page, pageSize, orderBy, ascending);

                if (bookPagedResult != null)
                {
                    PostgreSqlDataConvert.InitData(bookPagedResult);
                    result = PostgreSqlDataConvert.GetFormatedPagedResults();
                }
            }

            return result;
        }
    }
}
