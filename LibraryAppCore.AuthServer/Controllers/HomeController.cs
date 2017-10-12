using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LibraryAppCore.AuthServer.Controllers
{
    [Route("/Home")]
    public class HomeController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public string Get()
        {
            return "Auth with Identity Server Run";
        }


    }
}
