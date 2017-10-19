using LibraryAppCore.Domain.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;
using LibraryAppCore.Domain.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using LibraryAppCore.Domain.Entities.PostgreSql;

namespace LibraryAppCore.Domain.Concrete.PostgreSql
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

        public async Task<IEnumerable<Book>> GetBookById(string bookId)
        {
            if (!String.IsNullOrEmpty(bookId))
            {
                int bId = Convert.ToInt32(bookId);
                List<BookPostgreSql> Book = await db.Books.Where(b => b.Id == bId).ToListAsync();
                if(Book != null)
                {
                    PostgreSqlDataConvert.InitData(Book);
                    result = PostgreSqlDataConvert.GetIEnumerubleDbResult();
                }
            }
            return result;
        }

        public async Task<IEnumerable<Section>> GetSectionsByBookId(string bookId)
        {
            Section sections = new Section();
            List<Section> sectionList = new List<Section>();
            IEnumerable<Section> sectionsIE = null;

            if (!String.IsNullOrEmpty(bookId))
            {
                int bId = Convert.ToInt32(bookId);
                IEnumerable<SectionsPostgreSql> dbSectionsResult = await db.Sections.Where(s => s.BookId == bId).ToListAsync();
                if (dbSectionsResult != null)
                {
                    foreach (SectionsPostgreSql s in dbSectionsResult)
                    {
                        sections = new Section
                        {
                            New = s.New,
                            ForFamily = s.ForFamily,
                            Technical = s.Technical,
                            Fiction = s.Fiction,
                            ForBusness = s.ForBusness,
                            BookId = Convert.ToString(s.BookId)
                        };
                        sectionList.Add(sections);
                        sectionsIE = sectionList;
                    }
                }
            }
            return sectionsIE;
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

        public async Task<IEnumerable<Section>> GetAllBookSections()
        {
            IEnumerable<SectionsPostgreSql> dbSectionsResult = await db.Sections.ToListAsync();
            Section sections = new Section();
            List<Section> sectionList = new List<Section>();
            IEnumerable<Section> sectionsIE = null;

            if (dbSectionsResult != null)
            {
                foreach (SectionsPostgreSql s in dbSectionsResult)
                {
                    sections = new Section
                    {
                        New = s.New,
                        ForFamily = s.ForFamily,
                        Technical = s.Technical,
                        Fiction = s.Fiction,
                        ForBusness = s.ForBusness,
                        BookId = Convert.ToString(s.BookId)
                    };
                    sectionList.Add(sections);
                    sectionsIE = sectionList;
                }
            }

            return sectionsIE;
        }

        public int GetBookIdByBNameAndAId(string bookName, int authorId)
        {
            int bookId = 0;
            var bookResult = db.Books.Where(p => p.AuthorId == authorId && p.Name == bookName);
            foreach (BookPostgreSql p in bookResult)
            {
                if (p.AuthorId == authorId && p.Name == bookName)
                {
                    bookId = p.Id;
                }
            }
            return bookId;
        }

        public async Task<int> CreateBook(Book book)
        {
            int DbResult = 0;
            int authorId = 0;
            if (dataReqiered.IsDataNoEmpty(book))
            {
                authorId = Convert.ToInt32(book.AuthorId);
                BookPostgreSql newBook = new BookPostgreSql
                {
                    Name = book.Name,
                    Year = book.Year,
                    Language = book.Language,
                    Binding = book.Binding,
                    Weight = book.Weight,
                    Pages = book.Pages,

                    Subscription = book.Subscription != null ? Convert.ToDecimal(book.Subscription) : 0,
                    Price = book.Price != null ? Convert.ToDecimal(book.Price) : 0,

                    Description = book.Description,
                    ImageBook = book.ImageBook,
                    AuthorId = authorId
                };
               
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
            return DbResult;
        }

        public async Task<int> AddBookSection(string bookName, string authorId, Section section)
        {
            int DbResult = 0;
            int aId = 0;
            int bookId= 0;
            if (!String.IsNullOrEmpty(bookName) && !String.IsNullOrEmpty(authorId))
            {
                aId = Convert.ToInt32(authorId);
                bookId = GetBookIdByBNameAndAId(bookName, aId);
                if(bookId > 0)
                {
                    SectionsPostgreSql addBookSection = new SectionsPostgreSql
                    {
                        New = section.New,
                        ForFamily = section.ForFamily,
                        Technical = section.Technical,
                        Fiction = section.Fiction,
                        ForBusness = section.ForBusness,
                        BookId = bookId
                    };

                    db.Sections.Add(addBookSection);

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

        public async Task<int> UpdateBook(string id, Book book)
        {
            int DbResult = 0;
            int oldDataBookId, newDataBookId = 0;

            if (!String.IsNullOrEmpty(id) && dataReqiered.IsDataNoEmpty(book))
            {
                oldDataBookId = Convert.ToInt32(id);
                newDataBookId = Convert.ToInt32(book.Id);
                BookPostgreSql updatingBook = null;
                updatingBook = await db.Books.FindAsync(oldDataBookId);

                if (oldDataBookId == newDataBookId)
                {
                    updatingBook.Year = book.Year;
                    updatingBook.Name = book.Name;
                    updatingBook.Language = book.Language;
                    updatingBook.Binding = book.Binding;
                    updatingBook.Pages = book.Pages;
                    updatingBook.Weight = book.Weight;

                    updatingBook.Subscription = book.Subscription != null ? Convert.ToDecimal(book.Subscription) : 0;
                    updatingBook.Price = book.Price != null ? Convert.ToDecimal(book.Price) : 0;

                    if(book.ImageBook != null)
                        updatingBook.ImageBook = book.ImageBook;

                    updatingBook.Description = book.Description;

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

        public async Task<int> UpdateBookSections(string bookId, Section section)
        {
            int DbResult = 0;
            SectionsPostgreSql updatingSection = null;
            if (!String.IsNullOrEmpty(bookId))
            {
                int upBookId = Convert.ToInt32(bookId);
                updatingSection = await db.Sections.FindAsync(upBookId);
                if(updatingSection.BookId == upBookId)
                {
                    updatingSection.New = section.New;
                    updatingSection.ForFamily = section.ForFamily;
                    updatingSection.Technical = section.Technical;
                    updatingSection.Fiction = section.Fiction;
                    updatingSection.ForBusness = section.ForBusness;

                    db.Entry(updatingSection).State = EntityState.Modified;

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
