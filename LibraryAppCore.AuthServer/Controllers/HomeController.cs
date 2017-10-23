using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using LibraryAppCore.AuthServer.Quickstart.UI;
using IdentityServer4.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LibraryAppCore.AuthServer.Controllers
{

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        //public async Task<IActionResult> Error(string errorId)
        //{
        //    var vm = new ErrorViewModel();

        //    // retrieve error details from identityserver
        //    var message = await _interaction.GetErrorContextAsync(errorId);
        //    if (message != null)
        //    {
        //        vm.Error = message;
        //    }

        //    return View("Error", vm);
        //}
    }
}
