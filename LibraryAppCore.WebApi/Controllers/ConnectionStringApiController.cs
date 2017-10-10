using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using LibraryAppCore.WebApi.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LibraryAppCore.WebApi.Controllers
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
        public IActionResult SetDbConnection([FromBody]LibraryAppCore.WebApi.Models.DbConnection dbConnection)
        {
            ConnectionDB.ConnectionString = dbConnection.ConnectionString;
            return Ok();
        }

       
    }
}
