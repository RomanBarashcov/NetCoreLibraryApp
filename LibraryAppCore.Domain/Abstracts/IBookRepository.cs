using LibraryAppCore.Domain.Entities;
using LibraryAppCore.Domain.Pagination;
using LibraryAppCore.Domain.QueryResultObjects;
using System.Threading.Tasks;

namespace LibraryAppCore.Domain.Abstracts
{
    public interface IBookRepository
    {
        Task<PagedResults<Book>> GetAllBooks(int page, int pageSize, string orderBy, bool ascending);
        Task<Book> GetBookById(string bookId);
        Task<int> CreateBook(Book book);
        Task<int> UpdateBook(string bookId, Book book);
        Task<int> DeleteBook(string bookId);
        Task<PagedResults<Book>> GetBookByAuthorId(string authorId, int page, int pageSize, string orderBy, bool ascending);
    }
}
