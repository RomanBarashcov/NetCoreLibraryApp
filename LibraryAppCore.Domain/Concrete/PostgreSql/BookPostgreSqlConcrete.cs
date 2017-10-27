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
    public class BookPostgreSqlConcrete : IBookRepository
    {
        private IEnumerable<Book> result = null;
        private IConvertDataHelper<BookPostgreSql, Book> PostgreSqlDataConvert;
        private IDataRequired<Book> dataReqiered;
        private LibraryPostgreSqlContext db;

        public BookPostgreSqlConcrete(LibraryPostgreSqlContext context, IConvertDataHelper<BookPostgreSql, Book> pSqlDataConvert, IDataRequired<Book> dReqiered)
        {
            this.db = context;
            this.PostgreSqlDataConvert = pSqlDataConvert;
            this.dataReqiered = dReqiered;
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            List<BookPostgreSql> BookList = await db.Books.ToListAsync();

            if (BookList != null)
            {
                PostgreSqlDataConvert.InitData(BookList);
                result = PostgreSqlDataConvert.GetIEnumerubleDbResult();
            }

            return result;
        }

        public async Task<int> CreateBook(Book book)
        {
            int DbResult = 0;
            int authorId = 0;

            if (dataReqiered.IsDataNoEmpty(book))
            {
                bool isNewBook = CheckingBookOnDuplicate(book);

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

        private bool CheckingBookOnDuplicate(Book book)
        {
            int authorId = 0;
            bool isNewBook = false;

            if (dataReqiered.IsDataNoEmpty(book))
            {
                authorId = Convert.ToInt32(book.AuthorId);

                var createdBook = db.Books.Where(b => b.AuthorId == authorId && (b.Name == book.Name && b.Description == book.Description));

                if (createdBook != null)
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

        public async Task<IEnumerable<Book>> GetBookByAuthorId(string authorId)
        {
            if (!String.IsNullOrEmpty(authorId))
            {
                int author_Id = Convert.ToInt32(authorId);
                List<BookPostgreSql> BookList = await db.Books.Where(x => x.AuthorId == author_Id).ToListAsync();

                if (BookList != null)
                {
                    PostgreSqlDataConvert.InitData(BookList);
                    result = PostgreSqlDataConvert.GetIEnumerubleDbResult();
                }
            }

            return result;
        }
    }
}
