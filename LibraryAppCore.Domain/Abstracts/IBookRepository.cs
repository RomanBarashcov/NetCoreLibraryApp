using LibraryAppCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAppCore.Domain.Abstracts
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetBookById(string bookId);
        Task<IEnumerable<Section>> GetSectionsByBookId(string bookId);
        Task<IEnumerable<Book>> GetAllBooks();
        Task<IEnumerable<Section>> GetAllBookSections();
        int GetBookIdByBNameAndAId(string bookName, int authorId);
        Task<int> CreateBook(Book book);
        Task<int> AddBookSection(string bookName, string authorId, Section section);
        Task<int> UpdateBook(string bookId, Book book);
        Task<int> UpdateBookSections(string bookId, Section section);
        Task<int> DeleteBook(string bookId);
        Task<IEnumerable<Book>> GetBookByAuthorId(string authorId);
    }
}
