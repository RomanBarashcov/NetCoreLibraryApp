using LibraryAppCore.XF.Client.Models;
using LibraryAppCore.XF.Client.Pagination;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAppCore.XF.Client.Services
{
    public class BookService
    {
        private string bookApiUrl = "http://localhost:50185/BookApi";

        public BookService() { }

        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }

        public async Task<PagedResults<Book>> GetBooks(int page, string orderBy, bool ascending)
        {
            PagedResults<Book> bookReuslt = null;
            HttpClient client = GetClient();
            client.BaseAddress = new Uri(bookApiUrl + "?page=" + page + "&pageSize=" + 5 + "&orderBy=" + orderBy + "&ascending=" + ascending);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            var response = await client.GetStringAsync(client.BaseAddress);
            bookReuslt = JsonConvert.DeserializeObject<PagedResults<Book>>(response);

            return bookReuslt;
        }

        public async Task<bool> CreateBook(Book book)
        {
            HttpClient client = GetClient();
            var response = await client.PostAsync(bookApiUrl, new StringContent(
                    JsonConvert.SerializeObject(book),
                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
                return false;

            return true;
        }

        public async Task<bool> UpdateBook(Book book)
        {
            HttpClient client = GetClient();
            var response = await client.PutAsync(bookApiUrl + "/" + book.Id, new StringContent(
                    JsonConvert.SerializeObject(book),
                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
                return false;

            return true;
        }

        public async Task<bool> DeleteBook(string id)
        {
            HttpClient client = GetClient();
            var response = await client.DeleteAsync(bookApiUrl + "/" + id);
            if (response.StatusCode != HttpStatusCode.OK)
                return false;

            return true;
        }
    }
}
