using ModelsLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CLabManager_Web.Areas.User.Controllers
{
    [Area("User")]
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            List<Lab> labs = new List<Lab>();
            using (var httpClient = new HttpClient())
            {
                using (var response = httpClient.GetAsync("http://localhost:5028/api/labs").GetAwaiter().GetResult())
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    labs = JsonConvert.DeserializeObject<List<Lab>>(apiResponse);
                }
            }
            return View(labs);
        }
    }
}
