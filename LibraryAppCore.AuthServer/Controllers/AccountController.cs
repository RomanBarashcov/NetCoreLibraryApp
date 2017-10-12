using System;
using System.Threading.Tasks;
using LibraryAppCore.Domain.Entities;
using LibraryAppCore.AuthServer.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using IdentityServer4.Services;
using IdentityServer4.Events;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using LibraryAppCore.AuthServer;
using Microsoft.IdentityModel.Tokens;
using IdentityModel.Client;

namespace LibraryAppCore.WebApi.Controllers
{

    [Route("/Account")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("/Account/Login")]
        public async Task<IActionResult> Login([FromBody]LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    if (Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return await GetUserAccessToken(model.Email, model.Password);
                    }

                    return BadRequest("Retunr Ulr is incorrect");
                }
                else
                {
                    return BadRequest("Password or Email is incorrect");
                }
            }

            return BadRequest("Check all your data, and try again");
        }

        [AllowAnonymous]
        [HttpPost("/Account/Register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Email };

                var result = await _userManager.FindByEmailAsync(model.Email);
                if (result != null)
                {
                    var userCreatedResult = await _userManager.CreateAsync(user, model.Password);
                    if (userCreatedResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, false);
                        return await GetUserAccessToken(model.Email, model.Password);
                    }
                    else
                    {
                        return BadRequest(userCreatedResult.Errors.ToString());
                    }
                }
                else
                {
                    return BadRequest("This email address is registered. Please input other email address");
                }
            }

            return BadRequest("Check all your data, and try again");
        }


        public async Task<IActionResult> GetUserAccessToken(string email, string password)
        {
            if (!String.IsNullOrEmpty(email) && !String.IsNullOrEmpty(password))
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user != null)
                {
                    var tokenClient = new TokenClient("http://localhost:52658/connect/token", "library_app_core_client_side", "secret");
                    var tokenResponse = await tokenClient.RequestClientCredentialsAsync("library_app_core_wep_api");
                    var accessToken = tokenResponse.AccessToken;
                    return Ok(new { token = accessToken });
                }
            }

            return BadRequest("Could not create token");
        }

        //[HttpPost("/Account/Logout")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Logout()
        //{
        //    var user = HttpContext.User;

        //    if (user?.Identity.IsAuthenticated == true)
        //    {
        //        await HttpContext.SignOutAsync();
        //        User userByEmail = await _userManager.FindByEmailAsync(user.Identity.Name);

        //        if (userByEmail != null)
        //        {
        //            await _events.RaiseAsync(new UserLogoutSuccessEvent(userByEmail.Id, user.Identity.Name));
        //        }
        //        else {
        //            return BadRequest();
        //        }

        //        return Ok();
        //    }

        //    return BadRequest();
        //}

    }
}