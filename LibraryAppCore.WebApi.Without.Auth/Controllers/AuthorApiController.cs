using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LibraryAppCore.Domain.Entities;
using LibraryAppCore.Domain.Pagination;
using LibraryAppCore.Domain.Abstracts;

namespace LibraryAppCore.WebApi.Without.Auth.Controllers
{
    [Route("/AuthorApi")]
    public class AuthorApiController : Controller
    {
        private IAuthorRepository repository;
        private IDataRequired<Author> dataReqiered;

        public AuthorApiController(IAuthorRepository authorRepository, IDataRequired<Author> dReqiered)
        {
            this.repository = authorRepository;
            this.dataReqiered = dReqiered;
        }

        [HttpGet]
        public async Task<PagedResults<Author>> GetAuthors(int page, int pageSize, string orderBy, bool ascending)
        {
            PagedResults<Author> Authors = await repository.GetAllAuthors(page, pageSize, orderBy, ascending);
            return Authors;
        }

        [HttpGet("/AuthorApi/GetAuthors")]
        public async Task<IEnumerable<Author>> GetAuthors()
        {
            IEnumerable<Author> Authors = await repository.GetAllAuthors();
            return Authors;
        }

        [HttpGet("/AuthorApi/GetAuthorById/{id}")]
        public async Task<Author> GetAuthorById(string id)
        {
            Author Author = await repository.GetAuthorById(id);
            return Author;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuthor([FromBody]Author author)
        {
            int DbResult = 0;
            IActionResult ActionRes = BadRequest();

            if (dataReqiered.IsDataNoEmpty(author))
            {
                DbResult = await repository.CreateAuthor(author);

                if (DbResult != 0)
                {
                    ActionRes = Ok(author);
                }
            }

            return ActionRes;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(string id, [FromBody]Author author)
        {
            int DbResult = 0;
            IActionResult ActionRes = BadRequest();

            if (!String.IsNullOrEmpty(id) && dataReqiered.IsDataNoEmpty(author))
            {
                DbResult = await repository.UpdateAuthor(id, author);

                if (DbResult != 0)
                {
                    ActionRes = Ok(author);
                }
            }

            return ActionRes;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(string id)
        {
            int DbResult = 0;
            IActionResult ActionRes = BadRequest();

            if (!String.IsNullOrEmpty(id))
            {
                DbResult = await repository.DeleteAuthor(id);

                if (DbResult != 0)
                {
                    ActionRes = Ok();
                }
            }

            return ActionRes;
        }
    }
}