using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LibraryAppCore.WebApi.Controllers
{

    [Route("/HomeApi")]
    public class HomeApiController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public string Get()
        {
            return "WebApiProject Run";
        }

       
    }
}
