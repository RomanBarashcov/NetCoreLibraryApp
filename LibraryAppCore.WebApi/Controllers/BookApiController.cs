using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibraryAppCore.Domain.Entities;
using LibraryAppCore.Domain.Abstracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using LibraryAppCore.Domain.Pagination;

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
        public async Task<PagedResults<Book>> GetBooks(int page, int pageSize, string orderBy, bool ascending)
        {
            PagedResults<Book> Books = await repository.GetAllBooks(page, pageSize, orderBy, ascending);
            return Books;
        }

        [Authorize]
        [HttpGet("/BookApi/GetBookById/{id}")]
        public async Task<Book> GetBookById(string id)
        {
            Book Book = await repository.GetBookById(id);
            return Book;
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
        public async Task<PagedResults<Book>> GetBookByAuthorId(string id, int page, int pageSize, string orderBy, bool ascending)
        {
            PagedResults<Book> Books = null;

            if (!String.IsNullOrEmpty(id))
            {
                Books = await repository.GetBookByAuthorId(id, page, pageSize, orderBy, ascending);
            }

            return Books;
        }
    }
}
