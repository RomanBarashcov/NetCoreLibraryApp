using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibraryAppCore.Domain.Abstracts;
using LibraryAppCore.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using LibraryAppCore.Domain.Pagination;

namespace LibraryAppCore.WebApi.Controllers
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

        [AllowAnonymous]
        [HttpGet]
        public async Task<PagedResults<Author>> GetAuthors(int page, string orderBy, bool ascending)
        {
            PagedResults<Author> Authors = await repository.GetAllAuthors(page, orderBy, ascending);
            return Authors;
        }

        [AllowAnonymous]
        [HttpGet("/AuthorApi/GetAuthors")]
        public async Task<IEnumerable<Author>> GetAuthors()
        {
            IEnumerable<Author> Authors = await repository.GetAllAuthors();
            return Authors;
        }

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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
