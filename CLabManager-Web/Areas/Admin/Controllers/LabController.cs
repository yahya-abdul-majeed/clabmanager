using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using ModelsLibrary.Models;
using ModelsLibrary.Models.ViewModels;
using Newtonsoft.Json;
using System.Text;

namespace CLabManager_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LabController : Controller
    {
        public async Task<IActionResult> Create()
        {
            CreateLabVM vm = new CreateLabVM();
            var url = "http://localhost:5028/api/Computers/unassigned";
            using (var httpClient = new HttpClient())
            {
                using(var response = await httpClient.GetAsync(url))
                {
                    var apiResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    var computers = JsonConvert.DeserializeObject<List<Computer>>(apiResponse);
                    vm.UnassignedComputers = computers;
                }
            }
            return View(vm);
        }

        [HttpPost]
        public ActionResult CreateComputer(string computerName, string computerDesc)
        {
            var obj = new
            {
                computerName = computerName,
                description = computerDesc
            };
            var url = "https://localhost:7138/api/Computers";
           
            using (var httpClient = new HttpClient())
            {
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
                using(var response = httpClient.PostAsync(url, stringContent).GetAwaiter().GetResult())
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Created)
                    { 
                        
                    }
                }
            }
            return RedirectToAction("Create");
        }
    }
}
