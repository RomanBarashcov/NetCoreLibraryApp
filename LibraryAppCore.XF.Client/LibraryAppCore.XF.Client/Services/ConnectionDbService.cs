using LibraryAppCore.XF.Client.Models;
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
    public class ConnectionDbService
    {
        private string ConnectionStringApiUrl = "http://localhost:50185/ConnectionStringApi";
        private DbConnection dbCon { get; set; }
        
        public ConnectionDbService()
        {
            dbCon = new DbConnection();
        }

        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }

        public async Task<bool> SetConnectionString(string dbConnection)
        {
            HttpClient client = GetClient();
            dbCon.ConnectionString = dbConnection;

            var response = await client.PostAsync(ConnectionStringApiUrl, new StringContent(
                    JsonConvert.SerializeObject(dbCon),
                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
                return false;

            return true;
        }
    }
}
