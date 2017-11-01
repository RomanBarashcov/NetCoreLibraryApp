using LibraryAppCore.Domain.Entities;
using LibraryAppCore.Domain.Pagination;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAppCore.Domain.Abstracts
{
    public interface IAuthorRepository
    {
        Task<PagedResults<Author>> GetAllAuthors(int page, int pageSize, string orderBy, bool ascending);
        Task<IEnumerable<Author>> GetAllAuthors();
        Task<string> GetAuthorIdByName(string firstName, string surName);
        Task<Author> GetAuthorById(string authorId);
        Task<int> CreateAuthor(Author author);
        Task<int> UpdateAuthor(string authorId, Author author);
        Task<int> DeleteAuthor(string authorId);
    }
}
