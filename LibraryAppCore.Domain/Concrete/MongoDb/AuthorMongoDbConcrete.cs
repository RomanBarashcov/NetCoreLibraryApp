using LibraryAppCore.Domain.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;
using LibraryAppCore.Domain.Entities;
using System.Threading.Tasks;
using LibraryAppCore.Domain.Entities.MondoDb;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using LibraryAppCore.Domain.Pagination;

namespace LibraryAppCore.Domain.Concrete.MongoDb
{
    public class AuthorMongoDbConcrete : IAuthorRepository
    {
        private PagedResults<Author> result = null;
        private IConvertDataHelper<AuthorMongoDb, Author> mongoDbDataConvert;
        private IDataRequired<Author> dataReqiered;
        private LibraryMongoDbContext db;
        private IPagination<AuthorMongoDb> pagination;

        public AuthorMongoDbConcrete(LibraryMongoDbContext context, IConvertDataHelper<AuthorMongoDb, Author> mDbDataConvert, IDataRequired<Author> dReqiered, IPagination<AuthorMongoDb> paging)
        {
            this.db = context;
            this.mongoDbDataConvert = mDbDataConvert;
            this.dataReqiered = dReqiered;
            this.pagination = paging;
        }

        public async Task<PagedResults<Author>> GetAllAuthors(int page, int pageSize, string orderBy, bool ascending)
        {
            orderBy = orderBy == "Id" ? "_id" : orderBy;

            var builder = Builders<AuthorMongoDb>.Filter;
            var filters = new List<FilterDefinition<AuthorMongoDb>>();

            IEnumerable<AuthorMongoDb> CollectionResult = db.Authors.Find(builder.Empty).ToEnumerable();
            IQueryable<AuthorMongoDb> authorsQueryResult = CollectionResult.AsQueryable();

            PagedResults<AuthorMongoDb> authorPagedResult = await pagination.CreatePagedResultsAsync(authorsQueryResult, page, pageSize, orderBy, ascending);

            if (authorPagedResult != null)
            {
                mongoDbDataConvert.InitData(authorPagedResult);
                result = mongoDbDataConvert.GetFormatedPagedResults();
            }

            return result;
        }

        public async Task<IEnumerable<Author>> GetAllAuthors()
        {
            var builder = Builders<AuthorMongoDb>.Filter;
            var filters = new List<FilterDefinition<AuthorMongoDb>>();

            List<AuthorMongoDb> CollectionResult = await db.Authors.Find(builder.Empty).ToListAsync();
            IEnumerable<Author> authorEnumResult = null;

            if (CollectionResult != null)
            {
                mongoDbDataConvert.InitData(CollectionResult);
                authorEnumResult = mongoDbDataConvert.GetFormatedEnumResult();
            }

            return authorEnumResult;
        }


        public async Task<string> GetAuthorIdByName(string firstName, string surName)
        {
            string authorId = "";

            if (!String.IsNullOrEmpty(firstName) && (!String.IsNullOrEmpty(surName)))
            {
                var filter = new BsonDocument("$and", new BsonArray
                {
                    new BsonDocument("Name", firstName),
                    new BsonDocument("Surname" , surName)
                });

                List<AuthorMongoDb> author = await db.Authors.Find(filter).ToListAsync();

                if (author != null)
                {
                    foreach (AuthorMongoDb a in author)
                    {
                        authorId = a.Id;
                        break;
                    }
                }
            }

            return authorId;
        }

        public async Task<Author> GetAuthorById(string authorId)
        {
            Author Author = new Author();

            if (!String.IsNullOrEmpty(authorId))
            {
                AuthorMongoDb authorDbResult = await db.Authors.Find(new BsonDocument("_id", new ObjectId(authorId))).FirstOrDefaultAsync();

                if(authorDbResult != null)
                {
                    Author.Id = authorDbResult.Id;
                    Author.Name = authorDbResult.Name;
                    Author.Surname = authorDbResult.Surname;
                }
            }

            return Author;
        }

        public async Task<int> CreateAuthor(Author author)
        {
            int DbResult = 0;

            if (dataReqiered.IsDataNoEmpty(author))
            {
                AuthorMongoDb newAuthor = new AuthorMongoDb { Id = author.Id, Name = author.Name, Surname = author.Surname };

                try
                {
                    await db.Authors.InsertOneAsync(newAuthor);
                    DbResult = 1;
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
                AuthorMongoDb oldAuthorData = await db.Authors.Find(new BsonDocument("_id", new ObjectId(authorId))).FirstOrDefaultAsync();

                if (oldAuthorData != null)
                {
                    AuthorMongoDb newAuthorData = new AuthorMongoDb { Id = author.Id, Name = author.Name, Surname = author.Surname };

                    try
                    {
                        await db.Authors.ReplaceOneAsync(new BsonDocument("_id", new ObjectId(authorId)), newAuthorData);
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

        public async Task<int> DeleteAuthor(string authorId)
        {
            int DbResult = 0;

            if (!String.IsNullOrEmpty(authorId))
            {
                List<AuthorMongoDb> CollectionResult = await db.Authors.Find(new BsonDocument("_id", new ObjectId(authorId))).ToListAsync();

                if (CollectionResult != null)
                {
                    await db.Authors.DeleteOneAsync(new BsonDocument("_id", new ObjectId(authorId)));
                    DbResult = 1;
                }
            }

            return DbResult;
        }

    }
}
