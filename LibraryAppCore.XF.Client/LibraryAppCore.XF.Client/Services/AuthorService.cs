using LibraryAppCore.XF.Client.Models;
using LibraryAppCore.XF.Client.Pagination;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAppCore.XF.Client.Services
{
    public class AuthorService
    {
        private string authorApiUrl = "http://localhost:50185/AuthorApi";

        public AuthorService() {}

        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }

        public async Task<PagedResults<Author>> GetAuthors()
        {
            PagedResults<Author> authorReuslt = null;
            HttpClient client = GetClient();
            client.BaseAddress = new Uri(authorApiUrl + "?page=" + 1 + "&pageSize=" + 5 + "&orderBy=" + "Id" + "&ascending=" + true);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            var response = await client.GetStringAsync(client.BaseAddress);
            authorReuslt = JsonConvert.DeserializeObject<PagedResults<Author>>(response);

            return authorReuslt;
        }

        public async Task<bool> CreateAuthor(Author author)
        {
            HttpClient client = GetClient();
            var response = await client.PostAsync(authorApiUrl, new StringContent(
                    JsonConvert.SerializeObject(author),
                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
                return false;

            return true;
        }

        public async Task<bool> UpdateAuthor(Author author)
        {
            HttpClient client = GetClient();
            var response = await client.PutAsync(authorApiUrl + "/" + author.Id, new StringContent(
                    JsonConvert.SerializeObject(author),
                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
                return false;

            return true;
        }

        public async Task<bool> DeleteAuthor(string id)
        {
            HttpClient client = GetClient();
            var response = await client.DeleteAsync(authorApiUrl + "/" + id);
            if (response.StatusCode != HttpStatusCode.OK)
                return false;

            return true;
        }
    }
}
