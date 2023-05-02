using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ModelsLibrary.Models;
using ModelsLibrary.Utilities;
using Newtonsoft.Json;
using System.ComponentModel.Design.Serialization;

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
                    }
                }
            }
            return View();
        }
    }
}
