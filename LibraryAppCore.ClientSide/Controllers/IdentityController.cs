using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http;
using IdentityModel.Client;
using LibraryAppCore.AuthServer;

namespace LibraryAppCore.ClientSide.Controllers
{
    public class IdentityController : Controller
    {
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var apiCallUsingUserAccessToken = await ApiCallUsingUserAccessToken();
            ViewData["apiCallUsingUserAccessToken"] = apiCallUsingUserAccessToken.IsSuccessStatusCode ? await apiCallUsingUserAccessToken.Content.ReadAsStringAsync() : apiCallUsingUserAccessToken.StatusCode.ToString();

            var clientCredentialsResponse = await ApiCallUsingClientCredentials();
            ViewData["clientCredentialsResponse"] = clientCredentialsResponse.IsSuccessStatusCode ? await clientCredentialsResponse.Content.ReadAsStringAsync() : clientCredentialsResponse.StatusCode.ToString();

            return View();
        }

        private async Task<HttpResponseMessage> ApiCallUsingUserAccessToken()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var client = new HttpClient();
            client.SetBearerToken(accessToken);

            return await client.GetAsync( Config.AuthServerUrl + "/api/identity");
        }

        private async Task<HttpResponseMessage> ApiCallUsingClientCredentials()
        {
            var tokenClient = new TokenClient(Config.AngularClientUrl + "/connect/token", "library_app_core_client_side", "secret");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("library_app_core_wep_api");

            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            return await client.GetAsync(Config.AuthServerUrl + "/api/identity");
        }

        public async Task Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");
        }
    }
}
