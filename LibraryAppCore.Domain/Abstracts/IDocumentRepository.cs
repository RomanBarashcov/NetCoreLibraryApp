using LibraryAppCore.Domain.Entities;
using LibraryAppCore.Domain.Entities.MsSql;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAppCore.Domain.Abstracts
{
    public interface IDocumentRepository
    {
        Task <List<Book>> ReadDocumentAsync(IFormFile file);
        Task<bool> SaveData(List<Book> books);
    }
}
