using LibraryAppCore.Domain.Entities;
using LibraryAppCore.Domain.Pagination;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAppCore.Domain.Abstracts
{
    public interface IBookRepository
    {
        Task<PagedResults<Book>> GetAllBooks(int page, string orderBy, bool ascending);
        Task<int> CreateBook(Book book);
        Task<int> UpdateBook(string bookId, Book book);
        Task<int> DeleteBook(string bookId);
        Task<PagedResults<Book>> GetBookByAuthorId(string authorId, int page, string orderBy, bool ascending);
    }
}
