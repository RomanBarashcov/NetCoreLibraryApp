using LibraryAppCore.Domain.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;
using LibraryAppCore.Domain.Entities;
using System.Threading.Tasks;

namespace LibraryAppCore.Domain.Concrete.MongoDb
{
    public class BookMongoDbConcrete : IBookRespository
    {
        public Task<int> CreateBook(Book book)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteBook(string bookId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Book>> GetAllBooks()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Book>> GetBookByAuthorId(string authorId)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateBook(string bookId, Book book)
        {
            throw new NotImplementedException();
        }
    }
}
