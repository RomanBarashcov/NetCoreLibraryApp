using LibraryAppCore.Domain.Abstracts;
using System;
using System.Collections.Generic;
using LibraryAppCore.Domain.Entities;
using System.Threading.Tasks;
using LibraryAppCore.Domain.Entities.MondoDb;
using MongoDB.Bson;
using MongoDB.Driver;
using LibraryAppCore.Domain.Pagination;
using System.Linq;
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
            var BookQueryResult = from b in db.Books.AsQueryable()
                                  join a in db.Authors.AsQueryable() on b.AuthorId equals a.Id into joinedResult
                                  from r in joinedResult.DefaultIfEmpty()
                                  select new BookMongoDbQueryResult
                                  {
                                      Id = b.Id,
                                      Year = b.Year,
                                      Name = b.Name,
                                      Description = b.Description,
                                      AuthorId = b.AuthorId,
                                      AuthorName = r.Name + " " + r.Surname
                                  };

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
            if (!String.IsNullOrEmpty(authorId))
            {
                var BookQueryResult = from b in db.Books.AsQueryable().Where(b => b.AuthorId == authorId)
                                      join a in db.Authors.AsQueryable() on b.AuthorId equals a.Id into joinedResult
                                      from r in joinedResult.DefaultIfEmpty()
                                      select new BookMongoDbQueryResult
                                      {
                                          Id = b.Id,
                                          Year = b.Year,
                                          Name = b.Name,
                                          Description = b.Description,
                                          AuthorId = b.AuthorId,
                                          AuthorName = r.Name + " " + r.Surname
                                      };

                PagedResults<BookMongoDbQueryResult> booksPagedResult = await pagination.CreatePagedResultsAsync(BookQueryResult, page, pageSize, orderBy, ascending);

                if (booksPagedResult != null)
                {
                    mongoDbDataConvert.InitData(booksPagedResult);
                    result = mongoDbDataConvert.GetFormatedPagedResults();
                }
            }

            return result;
        }
    }
}
