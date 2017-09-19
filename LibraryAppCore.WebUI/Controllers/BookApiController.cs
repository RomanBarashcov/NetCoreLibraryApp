using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net;
using LibraryAppCore.Domain.Entities;
using LibraryAppCore.Domain.Abstracts;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LibraryAppCore.WebUI.Controllers
{
    [Route("/BookApi")]
    public class BookApiController : Controller
    {
        private IBookRespository repository;
        private IDataRequired<Book> dataReqiered;
        public BookApiController(IBookRespository bookRepository, IDataRequired<Book> dRequired)
        {
            this.repository = bookRepository;
            this.dataReqiered = dRequired;
        }

        public async Task<IEnumerable<Book>> GetBooks()
        {
            IEnumerable<Book> Books = await repository.GetAllBooks();
            return Books;
        }

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

        [HttpGet("/BookApi/GetBookByAuthorId/{id}")]
        public async Task<IEnumerable<Book>> GetBookByAuthorId(string id)
        {
            IEnumerable<Book> Books = await repository.GetBookByAuthorId(id);
            return Books;
        }
    }
}
