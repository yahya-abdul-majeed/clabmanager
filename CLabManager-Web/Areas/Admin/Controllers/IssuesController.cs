using Microsoft.AspNetCore.Authentication;
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
        public async Task<IActionResult> Index(int? roomNo =0, int? buildingNo =0, string? priority = null, string? state = null)
        {
            if(SD.getPrincipal().Identity == null)
                return RedirectToAction("AccessDenied", "Authentication", new { Area = "User" });
            if (SD.getPrincipal().IsInRole("User"))
                return RedirectToAction("AccessDenied", "Authentication",new {Area = "User"});
            IssueIndexVM vm = new IssueIndexVM();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Request.Cookies[SD.XAccessToken]);
                using (var response = await httpClient.GetAsync("https://localhost:7138/api/Issues"))
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    vm.Issues = JsonConvert.DeserializeObject<List<Issue>>(apiResponse);   
                }
                //checking for building and room no's
                if (buildingNo != 0 && roomNo != 0)
                {
                    vm.Issues = vm.Issues.Where(l => l.Lab.BuildingNo == buildingNo).ToList();
                    vm.Issues = vm.Issues.Where(l => l.Lab.RoomNo == roomNo).ToList();
                }
                else if (roomNo == 0 && buildingNo != 0)
                {
                    vm.Issues = vm.Issues.Where(l => l.Lab.BuildingNo == buildingNo).ToList();
                }
                else if (buildingNo == 0 && roomNo != 0)
                {
                    vm.Issues = vm.Issues.Where(l => l.Lab.RoomNo == roomNo).ToList();
                }
                //checking for state and priority
                if(priority != "All" && priority != null)
                {
                    vm.Issues = vm.Issues.Where(l=>l.Priority.ToString() ==priority).ToList();  
                }
                if(state != "All" && state != null)
                {
                    vm.Issues = vm.Issues.Where(l=>l.State.ToString() == state).ToList();
                }
            }
            Array values = Enum.GetValues(typeof(IssueState));
            Array values2 = Enum.GetValues(typeof(IssuePriority));
            vm.Items = new List<SelectListItem>();
            vm.PItems = new List<SelectListItem>();
            foreach (var i in values)
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

        public async Task<IActionResult> IssueDetail(int id)    
        {
            if (SD.getPrincipal().IsInRole("User"))
                return RedirectToAction("AccessDenied", "Authentication", new { Area = "User" });
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Request.Cookies[SD.XAccessToken]);
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
            //var test = User.Identity.Name;
            //var duck = typeof(HttpContext).GetInterfaces();
            //var st =  base.Accepted();
            StringContent content = new StringContent(JsonConvert.SerializeObject(dto));
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Request.Cookies[SD.XAccessToken]);
            using (var response = await httpClient.PutAsync($"https://localhost:7138/api/issues/{dto.IssueId}", content))
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
            }

            return RedirectToAction("IssueDetail", new { id = dto.IssueId});
        }
    }
}
