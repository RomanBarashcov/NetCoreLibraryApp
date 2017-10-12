using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibraryAppCore.Domain.Entities;
using LibraryAppCore.Domain.Abstracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LibraryAppCore.WebApi.Controllers
{
    [Route("/BookApi")]
    public class BookApiController : Controller
    {
        private IBookRepository repository;
        private IDataRequired<Book> dataReqiered;
        public BookApiController(IBookRepository bookRepository, IDataRequired<Book> dRequired)
        {
            this.repository = bookRepository;
            this.dataReqiered = dRequired;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<Book>> GetBooks()
        {
            IEnumerable<Book> Books = await repository.GetAllBooks();
            return Books;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] Book book)
        {
            int DbResult = 0;
            IActionResult ActionRes = BadRequest();
            if (dataReqiered.IsDataNoEmpty(book))
            {
                DbResult = await repository.CreateBook(book);
                if (DbResult != 0)
                {
                    ActionRes = Ok();
                }
            }
            return ActionRes;
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(string id, [FromBody] Book book)
        {
            int DbResult = 0;
            IActionResult ActionRes = BadRequest();
            if (!String.IsNullOrEmpty(id) && dataReqiered.IsDataNoEmpty(book))
            {
                DbResult = await repository.UpdateBook(id, book);
                if (DbResult != 0)
                {
                    ActionRes = Ok();
                }
            }
            return ActionRes;
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(string id)
        {
            int DbResult = 0;
            IActionResult ActionRes = BadRequest();
            if (!String.IsNullOrEmpty(id))
            {
                DbResult = await repository.DeleteBook(id);
                if (DbResult != 0)
                {
                    ActionRes = Ok();
                }
            }
            return ActionRes;
        }

        [AllowAnonymous]
        [HttpGet("/BookApi/GetBookByAuthorId/{id}")]
        public async Task<IEnumerable<Book>> GetBookByAuthorId(string id)
        {
            IEnumerable<Book> Books = await repository.GetBookByAuthorId(id);
            return Books;
        }
    }
}
