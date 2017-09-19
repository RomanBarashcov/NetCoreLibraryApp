using LibraryAppCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAppCore.Domain.Abstracts
{
    public interface IBookRespository
    {
        Task<IEnumerable<Book>> GetAllBooks();
        Task<int> CreateBook(Book book);
        Task<int> UpdateBook(string bookId, Book book);
        Task<int> DeleteBook(string bookId);
        Task<IEnumerable<Book>> GetBookByAuthorId(string authorId);
    }
}
