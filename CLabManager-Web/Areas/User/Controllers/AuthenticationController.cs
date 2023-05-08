using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ModelsLibrary.Models;
using ModelsLibrary.Utilities;
using Newtonsoft.Json;
using NuGet.Common;
using System.ComponentModel.Design.Serialization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CLabManager_Web.Areas.User.Controllers
{
    [Area("User")]
    public class AuthenticationController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(SignInUser userData)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(userData));
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var url = "https://localhost:7138/api/Auth/signin";
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync(url, content))
                {
                    
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var apiResponse = await response.Content.ReadAsStringAsync();
                        var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(apiResponse);
                        Response.Cookies.Append(SD.XAccessToken, loginResponse.authResponse.Token );
                        return RedirectToAction("Index", "Labs", new { Area = "User" });
                    }
                }
            }
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(SignUpUser userData)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(userData));
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var url = "https://localhost:7138/api/Auth/signup";
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync(url, content))
                {
                  
                    if(response.StatusCode.Equals(System.Net.HttpStatusCode.OK))
                    {
                        var apiResponse = await response.Content.ReadAsStringAsync();
                        var authResponse = JsonConvert.DeserializeObject<AuthenticationResponse>(apiResponse);
                        Response.Cookies.Append(SD.XAccessToken, authResponse.Token);
                        return RedirectToAction("Index","Labs");
                    }
                }
            }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete(SD.XAccessToken);
            return RedirectToAction("Index", "Labs");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
