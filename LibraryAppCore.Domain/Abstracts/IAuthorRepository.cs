using LibraryAppCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAppCore.Domain.Abstracts
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<Author>> GetAllAuthors();
        Task<int> GetAuthorIdByFullName(string firstName, string lastName);
        Task<Author> GetAuthorById(string authorId);
        Task<int> CreateAuthor(Author author);
        Task<int> UpdateAuthor(string authorId, Author author);
        Task<int> DeleteAuthor(string authorId);
    }
}
