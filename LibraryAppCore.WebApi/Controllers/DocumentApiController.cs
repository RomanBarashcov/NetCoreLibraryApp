using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LibraryAppCore.Domain.Abstracts;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using LibraryAppCore.Domain.Entities.MsSql;
using System.Threading.Tasks;

namespace LibraryAppCore.WebApi.Controllers
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

        [AllowAnonymous]
        [HttpPost("/DocumentApi/Upload/")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file)
        {
            if (file == null) return BadRequest("File is null");
            if (file.Length == 0) return BadRequest("File is empty");

            List<BookPostgreSql> books = repository.ReadDocument(file);

            if(books != null)
            {
                bool result = await repository.SaveData(books);
                if (result)
                    return Ok("Data added successfully");
            }

            return BadRequest("Error");
        }


    }
}

