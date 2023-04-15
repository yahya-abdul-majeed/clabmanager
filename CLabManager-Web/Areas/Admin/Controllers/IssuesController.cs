using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ModelsLibrary.Models;
using ModelsLibrary.Models.DTO;
using ModelsLibrary.Models.ViewModels;
using ModelsLibrary.Utilities;
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
            IssueDetailVM vm = new IssueDetailVM();
            using(var response = await httpClient.GetAsync($"https://localhost:7138/api/Issues/{id}"))
            {
                var apiResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult(); 
                vm.Issue = JsonConvert.DeserializeObject<Issue>(apiResponse);
            }
            Array values = Enum.GetValues(typeof(IssueState));
            Array values2 = Enum.GetValues(typeof(IssuePriority));
            vm.Items = new List<SelectListItem>();
            vm.PItems = new List<SelectListItem>();
            foreach(var i in values)
            {
                vm.Items.Add(new SelectListItem
                {
                    Text = Enum.GetName(typeof(IssueState), i),
                    Value = i.ToString()
                }); 
            }
            foreach(var i in values2)
            {
                vm.PItems.Add(new SelectListItem
                {
                    Text = Enum.GetName(typeof(IssuePriority), i),
                    Value = i.ToString()
                });
            }
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> IssueUpdate(IssueUpdateDTO dto)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(dto));
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var httpClient = new HttpClient();
            using (var response = await httpClient.PutAsync($"https://localhost:7138/api/issues/{dto.IssueId}", content))
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
            }

            return RedirectToAction("IssueDetail", new { id = dto.IssueId});
        }
    }
}
