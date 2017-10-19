using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibraryAppCore.Domain.Entities;
using LibraryAppCore.Domain.Abstracts;
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

        [AllowAnonymous]
        [HttpGet("/BookApi/GetBookSections")]
        public async Task<IEnumerable<Section>> GetBookSections()
        {
            IEnumerable<Section> Sections = await repository.GetAllBookSections();
            return Sections;
        }

        [Authorize]
        [HttpGet("/BookApi/GetBookById/{bookId}")]
        public async Task<IEnumerable<Book>> GetBookById(string bookId)
        {
            IEnumerable<Book> Book = null;
            if (!String.IsNullOrEmpty(bookId))
            {
                Book = await repository.GetBookById(bookId);
            }
            return Book;
        }

        [Authorize]
        [HttpGet("/BookApi/GetBookSectionsById/{bookId}")]
        public async Task<IEnumerable<Section>> GetBookSectionsById(string bookId)
        {
            IEnumerable<Section> Sections = null;
            if (!String.IsNullOrEmpty(bookId))
            {
                Sections = await repository.GetSectionsByBookId(bookId);
            }
            return Sections;
        }

        [Authorize]
        [HttpPost("/BookApi/CreateBook")]
        public async Task<IActionResult> CreateBook([FromBody] Book book)
        {
            int DbResult = 0;
            IActionResult ActionRes = BadRequest();
            if (dataReqiered.IsDataNoEmpty(book))
            {
                DbResult = await repository.CreateBook(book);
                if (DbResult != 0)
                {
                    ActionRes = Ok("Book created!");
                }
            }
            return ActionRes;
        }

        [Authorize]
        [HttpPost("/BookApi/AddSectionsBook/{authorId}/{bookName}")]
        public async Task<IActionResult> AddSectionsBook(string authorId, string bookName, [FromBody]Section bookSections)
        {
            int DbResult = 0;
            IActionResult ActionRes = BadRequest();
            if (!String.IsNullOrEmpty(authorId) && !String.IsNullOrEmpty(bookName))
            {
                DbResult = await repository.AddBookSection(bookName, authorId, bookSections);
                if(DbResult != 0)
                {
                    ActionRes = Ok("Book sections updated!");
                }
            }
            return ActionRes;

        }

        [Authorize]
        [HttpPut("/BookApi/UpdateBook/{id}")]
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
        [HttpPut("/BookApi/UpdateBookSections/{bookId}")]
        public async Task<IActionResult> UpdateBookSections(string bookId, [FromBody]Section bookSections)
        {
            int DbResult = 0;
            IActionResult ActionRes = BadRequest();
            if (!String.IsNullOrEmpty(bookId))
            {
                DbResult = await repository.UpdateBookSections(bookId, bookSections);
                if(DbResult != 0)
                {
                    ActionRes = Ok("book section updated!");
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
            IEnumerable<Book> Books = null;
            if (!String.IsNullOrEmpty(id))
            {
                Books = await repository.GetBookByAuthorId(id);
            }
            return Books;
        }
    }
}
