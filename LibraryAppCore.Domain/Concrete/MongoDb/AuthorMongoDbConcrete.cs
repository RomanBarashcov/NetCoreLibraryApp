using LibraryAppCore.Domain.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;
using LibraryAppCore.Domain.Entities;
using System.Threading.Tasks;

namespace LibraryAppCore.Domain.Concrete.MongoDb
{
    public class AuthorMongoDbConcrete : IAuthorRepository
    {
        public Task<int> CreateAuthor(Author author)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAuthor(string authorId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Author>> GetAllAuthors()
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAuthor(string authorId, Author author)
        {
            throw new NotImplementedException();
        }
    }
}
