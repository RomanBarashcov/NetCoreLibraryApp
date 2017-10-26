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
        List<BookPostgreSql> ReadDocument(IFormFile file);
        Task<bool> SaveData(List<BookPostgreSql> books);
    }
}
