using LibraryAppCore.Domain.Abstracts;
using System;
using LibraryAppCore.Domain.Entities;
using System.Threading.Tasks;
using LibraryAppCore.Domain.Entities.MsSql;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using LibraryAppCore.Domain.Pagination;
using LibraryAppCore.Domain.QueryResultObjects;

namespace LibraryAppCore.Domain.Concrete.MsSql
{
    public class BookPostgreSqlConcrete : IBookRepository
    {
        private IConvertDataHelper<BookPostgreSqlQueryResult, Book> PostgreSqlDataConvert;
        private IDataRequired<Book> dataReqiered;
        private LibraryPostgreSqlContext db;
        private IPagination<BookPostgreSqlQueryResult> pagination;

        public BookPostgreSqlConcrete(LibraryPostgreSqlContext context, IConvertDataHelper<BookPostgreSqlQueryResult, Book> pSqlDataConvert, IDataRequired<Book> dReqiered, IPagination<BookPostgreSqlQueryResult> pagin)
        {
            this.db = context;
            this.PostgreSqlDataConvert = pSqlDataConvert;
            this.dataReqiered = dReqiered;
            this.pagination = pagin;
        }

        public async Task<PagedResults<Book>> GetAllBooks(int page, int pageSize, string orderBy, bool ascending)
        {
            PagedResults<Book> result = null;
            IQueryable<BookPostgreSqlQueryResult> BookQuery = db.Books.Join(db.Authors, b => b.AuthorId, a => a.Id, (b,a) => new BookPostgreSqlQueryResult
            {
                Id = b.Id,
                Year = b.Year,
                Name = b.Name,
                Description = b.Description,
                AuthorId = a.Id.ToString(),
                AuthorName = a.Name + " " + a.Surname

            }).AsQueryable();

            PagedResults<BookPostgreSqlQueryResult> BookPagedResult = await pagination.CreatePagedResultsAsync(BookQuery, page, pageSize, orderBy, ascending);

            if(BookPagedResult != null)
            {
                PostgreSqlDataConvert.InitData(BookPagedResult);
                result = PostgreSqlDataConvert.GetFormatedPagedResults();  
            }

            return result;
        }

        public async Task<Book> GetBookById(string bookId)
        {
            Book result = null;

            if (!String.IsNullOrEmpty(bookId))
            {
                int bId = Convert.ToInt16(bookId);

                IQueryable<Book> BookQuery = db.Books.Where(b => b.Id == bId)
                    .Join(db.Authors, b => b.AuthorId, a => a.Id, (b, a) => new Book
                    {
                        Id = b.Id.ToString(),
                        Year = b.Year,
                        Name = b.Name,
                        Description = b.Description,
                        AuthorId = a.Id.ToString(),
                        AuthorName = a.Name + " " + a.Surname

                    });

                result = new Book(BookQuery);
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
            PagedResults<Book> result = null;
            PagedResults<BookPostgreSqlQueryResult> bookPagedResult = null;

            if (!String.IsNullOrEmpty(authorId))
            {
                int author_Id = Convert.ToInt32(authorId);

                IQueryable<BookPostgreSqlQueryResult> BookQuery = db.Books.Where(b => b.AuthorId == author_Id)
                    .Join(db.Authors, b => b.AuthorId, a => a.Id, (b, a) => new BookPostgreSqlQueryResult
                    {
                         Id = b.Id,
                         Year = b.Year,
                         Name = b.Name,
                         Description = b.Description,
                         AuthorId = a.Id.ToString(),
                         AuthorName = a.Name + " " + a.Surname

                    }).AsQueryable();

                bookPagedResult = await pagination.CreatePagedResultsAsync(BookQuery, page, pageSize, orderBy, ascending);

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
