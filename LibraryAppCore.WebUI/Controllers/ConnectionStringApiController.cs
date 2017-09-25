using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibraryAppCore_WebUI;
using Microsoft.Extensions.DependencyInjection;
using LibraryAppCore.WebUI.Models;
using LibraryAppCore.WebUI.Services;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Hosting;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LibraryAppCore.WebUI.Controllers
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
        public IActionResult SetDbConnection([FromBody]LibraryAppCore.WebUI.Models.DbConnection dbConnection)
        {
            IServiceCollectionExtension.cString = dbConnection.ConnectionString;
            return Ok();
        }

       
    }
}
