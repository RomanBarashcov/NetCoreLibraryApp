using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LibraryAppCore.WebApi.Without.Auth.Services;
using LibraryAppCore.WebApi.Without.Auth.Models;

namespace LibraryAppCore.WebApi.Without.Auth.Controllers
{
    [Route("/ConnectionStringApi")]
    public class ConnectionStringApiController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        public IActionResult SetDbConnection([FromBody]DbConnection dbConnection)
        {
            ConnectionDB.ConnectionString = dbConnection.ConnectionString;
            return Ok();
        }
    }
}