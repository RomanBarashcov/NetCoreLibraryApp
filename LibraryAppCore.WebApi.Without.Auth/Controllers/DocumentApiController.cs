using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LibraryAppCore.Domain.Entities;
using LibraryAppCore.Domain.Abstracts;

namespace LibraryAppCore.WebApi.Without.Auth.Controllers
{
    [Route("/DocumentApi")]
    public class DocumentApiController : Controller
    {
        private IDocumentRepository repository;

        public DocumentApiController(IDocumentRepository _repository)
        {
            repository = _repository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Connection success!");
        }

        [HttpPost("/DocumentApi/Upload/")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file)
        {
            if (file == null) return BadRequest("File is null");
            if (file.Length == 0) return BadRequest("File is empty");

            List<Book> books = await repository.ReadDocumentAsync(file);

            if (books != null)
            {
                bool result = await repository.SaveData(books);
                if (result)
                    return Ok("Data added successfully");
                else
                    return Ok("All data is dublicating!");
            }

            return BadRequest("Error");
        }
    }
}