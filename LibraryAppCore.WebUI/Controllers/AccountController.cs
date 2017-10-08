using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LibraryAppCore.Domain.Entities;
using LibraryAppCore.WebUI.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LibraryAppCore.WebUI.Controllers
{
    
    [Route("/Account")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _config;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
        
        [HttpPost("/Account/Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            IActionResult ActionRes = BadRequest();
            if (ModelState.IsValid)
            {
                User user = new User {Email = model.Email, UserName = model.Email};

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    ActionRes = await GenerateToken(model.Email, model.Password);
                }
                else
                {
                    ActionRes = BadRequest(result.Errors.ToString());
                }
            }
            else
            {
                ActionRes = BadRequest("Password or Email is Incorrect");
            }
            return ActionRes;
        }

        [HttpPost("/Account/Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            IActionResult ActionRes = BadRequest();
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        ActionRes = await GenerateToken(model.Email, model.Password);
                    }
                }
                else
                {
                    ActionRes = BadRequest("Password or Email is Incorrect");
                }
            }
            return ActionRes;
        }


        public async Task<IActionResult> GenerateToken(string email, string password)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(email);
 
                if (user != null)
                {
                    var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
                    if (result.Succeeded)
                    {
 
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        };
 
                        var key = AuthOptions.GetSymmetricSecurityKey();
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
 
                        var token = new JwtSecurityToken(
                                AuthOptions.ISSUER,
                                AuthOptions.AUDIENCE,
                                claims,
                                expires: DateTime.Now.AddMinutes(30),
                                signingCredentials: creds
                            );
 
                        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
                    }
                }
            }
 
            return BadRequest("Could not create token");
        }
        
        [HttpPost("/Account/LogOff")]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [HttpGet]
        public IActionResult GetLogin()
        {
            return Ok($"Ваш логин: {User.Identity.Name}");
        }
    }
}