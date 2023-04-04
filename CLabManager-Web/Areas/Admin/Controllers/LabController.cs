using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace CLabManager_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LabController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult CreateComputer(IFormCollection values)
        {
            var obj = new
            {
                computerName = values["computerName"],
                description = values["computerDesc"],
                isPositioned = false,
                labId = (int?)null
            };
            var url = "http://localhost:5028/api/labs";
           
            using (var httpClient = new HttpClient())
            {
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
                using(var response = httpClient.PostAsync(url, stringContent).GetAwaiter().GetResult())
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {

                    }
                }
            }
            return View("Create");
        }
    }
}
