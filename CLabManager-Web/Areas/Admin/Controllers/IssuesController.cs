using Microsoft.AspNetCore.Mvc;
using ModelsLibrary.Models;
using Newtonsoft.Json;

namespace CLabManager_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class IssuesController : Controller
    {
        public async Task<IActionResult> Index()
        {
            List<Issue> issues = new List<Issue>();
            using(var httpClient = new HttpClient())
            {
                using(var response = await httpClient.GetAsync("https://localhost:7138/api/Issues"))
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    issues = JsonConvert.DeserializeObject<List<Issue>>(apiResponse);   
                }
            }
            return View(issues);
        }

        public async Task<IActionResult> IssueDetail(int id)
        {
            var httpClient = new HttpClient();
            Issue issue;
            using(var response = await httpClient.GetAsync($"https://localhost:7138/api/Issues/{id}"))
            {
                var apiResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult(); 
                issue = JsonConvert.DeserializeObject<Issue>(apiResponse);
            }
            return View(issue);
        }
    }
}
