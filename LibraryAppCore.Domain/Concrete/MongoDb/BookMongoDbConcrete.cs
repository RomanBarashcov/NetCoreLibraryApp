using LibraryAppCore.Domain.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;
using LibraryAppCore.Domain.Entities;
using System.Threading.Tasks;
using LibraryAppCore.Domain.Entities.MondoDb;
using MongoDB.Bson;
using MongoDB.Driver;

namespace LibraryAppCore.Domain.Concrete.MongoDb
{
    public class BookMongoDbConcrete : IBookRepository
    {
        private IEnumerable<Book> result = null;
        private IConvertDataHelper<BookMongoDb, Book> mongoDbDataConvert;
        private IDataRequired<Book> dataReqiered;
        LibraryMongoDbContext db;

        public BookMongoDbConcrete(LibraryMongoDbContext context, IConvertDataHelper<BookMongoDb, Book> mDbDataConvert, IDataRequired<Book> dReqiered)
        {
            this.db = context;
            this.mongoDbDataConvert = mDbDataConvert;
            this.dataReqiered = dReqiered;
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            var builder = Builders<BookMongoDb>.Filter;
            var filters = new List<FilterDefinition<BookMongoDb>>();
            List<BookMongoDb> CollectionResult = await db.Books.Find(builder.Empty).ToListAsync();

            if (CollectionResult != null)
            {
                mongoDbDataConvert.InitData(CollectionResult);
                result = mongoDbDataConvert.GetIEnumerubleDbResult();
            }
            return result;
        }

        public async Task<int> CreateBook(Book book)
        {
            int DbResult = 0;
            if (dataReqiered.IsDataNoEmpty(book))
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
            return DbResult;
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

        public async Task<IEnumerable<Book>> GetBookByAuthorId(string authorId)
        {
            if (!String.IsNullOrEmpty(authorId))
            {
                List<BookMongoDb> BooksByAuthor = await db.Books.Find(new BsonDocument("AuthorId", authorId)).ToListAsync();
                if (BooksByAuthor != null)
                {
                    mongoDbDataConvert.InitData(BooksByAuthor);
                    result = mongoDbDataConvert.GetIEnumerubleDbResult();
                }
            }
            return result;
        }

        public Task<IEnumerable<Book>> GetBookById(string bookId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Section>> GetSectionsByBookId(string bookId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Section>> GetAllBookSections()
        {
            throw new NotImplementedException();
        }

        public int GetBookIdByBNameAndAId(string bookName, int authorId)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddBookSection(string bookName, string authorId, Section section)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateBookSections(string bookId, Section section)
        {
            throw new NotImplementedException();
        }
    }
}
