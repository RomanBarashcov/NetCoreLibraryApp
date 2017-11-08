using LibraryAppCore.Domain.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;
using LibraryAppCore.Domain.Entities;
using System.Threading.Tasks;
using LibraryAppCore.Domain.Entities.MondoDb;
using MongoDB.Bson;
using MongoDB.Driver;
using LibraryAppCore.Domain.Pagination;
using System.Linq;
using LibraryAppCore.Domain.QueryResultObjects;
using LibraryAppCore.Domain.QueryResultObjects.MongoDb;

namespace LibraryAppCore.Domain.Concrete.MongoDb
{
    public class BookMongoDbConcrete : IBookRepository
    {
        private PagedResults<Book> result = null;
        private IConvertDataHelper<BookMongoDbQueryResult, Book> mongoDbDataConvert;
        private IDataRequired<Book> dataReqiered;
        private LibraryMongoDbContext db;
        private IPagination<BookMongoDbQueryResult> pagination;

        public BookMongoDbConcrete(LibraryMongoDbContext context, IConvertDataHelper<BookMongoDbQueryResult, Book> mDbDataConvert, IDataRequired<Book> dReqiered, IPagination<BookMongoDbQueryResult> paging)
        {
            this.db = context;
            this.mongoDbDataConvert = mDbDataConvert;
            this.dataReqiered = dReqiered;
            this.pagination = paging;
        }

        public async Task<PagedResults<Book>> GetAllBooks(int page, int pageSize, string orderBy, bool ascending)
        {
            var authorCollections = await db.Authors.Find(new BsonDocument()).ToListAsync();
            var bookCollections = await db.Books.Find(new BsonDocument()).ToListAsync();

            IQueryable<BookMongoDbQueryResult> BookQueryResult = await GetBookQueryResult(authorCollections, bookCollections);

            PagedResults<BookMongoDbQueryResult> bookPagedResult = await pagination.CreatePagedResultsAsync(BookQueryResult, page, pageSize, orderBy, ascending);

            if (bookPagedResult != null)
            {
                mongoDbDataConvert.InitData(bookPagedResult);
                result = mongoDbDataConvert.GetFormatedPagedResults();
            }

            return result;
        }

        public async Task<int> CreateBook(Book book)
        {
            int DbResult = 0;

            if (dataReqiered.IsDataNoEmpty(book))
            {
                bool isNewBook = await CheckingBookOnDuplicate(book);

                if (isNewBook)
                {
                    BookMongoDb newBook = new BookMongoDb { Id = book.Id, Year = book.Year, Name = book.Name, Description = book.Description, AuthorId = book.AuthorId };

                    try
                    {
                        await db.Books.InsertOneAsync(newBook);
                        DbResult = 1;
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
            bool isNewBook = false;

            if (dataReqiered.IsDataNoEmpty(book))
            {
                var filter = new BsonDocument("$and", new BsonArray
                {
                    new BsonDocument("Name" , book.Name),
                    new BsonDocument("Description", book.Description)
                });

                List<BookMongoDb> createdBook = await db.Books.Find(filter).ToListAsync();

                if(createdBook.Count > 0)
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


        public async Task<int> UpdateBook(string bookId, Book book)
        {
            int DbResult = 0;

            if (!String.IsNullOrEmpty(bookId) && dataReqiered.IsDataNoEmpty(book))
            {
                List<BookMongoDb> oldBookData = await db.Books.Find(new BsonDocument("_id", new ObjectId(bookId))).ToListAsync();

                if (oldBookData != null)
                {
                    BookMongoDb newBookData = new BookMongoDb { Id = book.Id, Year = book.Year, Name = book.Name, Description = book.Description, AuthorId = book.AuthorId };

                    try
                    {
                        await db.Books.ReplaceOneAsync(new BsonDocument("_id", new ObjectId(bookId)), newBookData);
                        DbResult = 1;
                    }
                    catch
                    {
                        return DbResult;
                    }
                }
            }

            return DbResult;
        }

        public async Task<int> DeleteBook(string bookId)
        {
            int DbResult = 0;

            if (!String.IsNullOrEmpty(bookId))
            {
                List<BookMongoDb> deletingBook = await db.Books.Find(new BsonDocument("_id", new ObjectId(bookId))).ToListAsync();

                if (deletingBook != null)
                {
                    await db.Books.DeleteOneAsync(new BsonDocument("_id", new ObjectId(bookId)));
                    DbResult = 1;
                }
            }

            return DbResult;
        }

        public async Task<PagedResults<Book>> GetBookByAuthorId(string authorId, int page, int pageSize, string orderBy, bool ascending)
        {
            PagedResults<BookMongoDbQueryResult> booksPagedResult = null;
            if (!String.IsNullOrEmpty(authorId))
            {
                var authorCollections = await db.Authors.Find(new BsonDocument()).ToListAsync();
                var bookCollections = await db.Books.Find(new BsonDocument("AuthorId", authorId)).ToListAsync();

                IQueryable<BookMongoDbQueryResult> BookQueryResult = await GetBookQueryResult(authorCollections, bookCollections);

                booksPagedResult = await pagination.CreatePagedResultsAsync(BookQueryResult, page, pageSize, orderBy, ascending);

                if (booksPagedResult != null)
                {
                    mongoDbDataConvert.InitData(booksPagedResult);
                    result = mongoDbDataConvert.GetFormatedPagedResults();
                }
            }

            return result;
        }

        private async Task<IQueryable<BookMongoDbQueryResult>> GetBookQueryResult(List<AuthorMongoDb> authorCollection, List<BookMongoDb> bookCollection)
        {
            List<BookMongoDbQueryResult> BookQueryResult = new List<BookMongoDbQueryResult>();

            foreach (var b in bookCollection)
            {
                BookMongoDbQueryResult books = new BookMongoDbQueryResult();

                books.Id = b.Id;
                books.Year = b.Year;
                books.Name = b.Name;
                books.Description = b.Description;
                books.AuthorId = b.AuthorId;

                foreach (var a in authorCollection)
                {
                    if (books.AuthorId == a.Id)
                    {
                        books.AuthorName = a.Name + " " + a.Surname;
                        break;
                    }
                }

                BookQueryResult.Add(books);
            }

            return BookQueryResult.AsQueryable();
        }
    }
}
