using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ModelsLibrary.Models;
using ModelsLibrary.Models.ViewModels;
using ModelsLibrary.Utilities;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace CLabManager_Web.Areas.User.Controllers
{
    [Area("User")]
    public class LabsController : Controller
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public LabsController(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public async Task<IActionResult> Index(int buildingNo, int roomNo)
        {
            List<Lab> labs = new List<Lab>();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Request.Cookies[SD.XAccessToken]);
                //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
                //httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _contextAccessor.HttpContext.Request.Cookies[SD.XAccessToken]);
                //Request.Headers.Authorization = "Bearer " + HttpContext.Request.Cookies[SD.XAccessToken];
                // GetAwaiter() is blocking, use await instead
                using (var response =await httpClient.GetAsync("https://localhost:7138/api/labs")) // for jwt, url should be https
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    labs = JsonConvert.DeserializeObject<List<Lab>>(apiResponse);
                }
            }
            if (buildingNo != 0 && roomNo != 0)
            {
                labs = labs.Where(l => l.BuildingNo == buildingNo).ToList();
                labs = labs.Where(l => l.RoomNo == roomNo).ToList();
            }
            else if (roomNo == 0 && buildingNo != 0)
            {
                labs = labs.Where(l => l.BuildingNo == buildingNo).ToList();
            }
            else if (buildingNo == 0 && roomNo != 0)
            {
                labs = labs.Where(l => l.RoomNo == roomNo).ToList();
            }
            return View(labs);
        }

        public async Task<IActionResult> LabDetail(int id)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Request.Cookies[SD.XAccessToken]);
            LabDetailVM vm = new LabDetailVM();
            using(var response = await httpClient.GetAsync($"https://localhost:7138/api/Labs/{id}"))
            {
                var apiResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                vm.Lab = JsonConvert.DeserializeObject<Lab>(apiResponse);
            }
            Array values = Enum.GetValues(typeof(IssuePriority));
            vm.priorities = new List<SelectListItem>();
            foreach(var i in values)
            {
                vm.priorities.Add(new SelectListItem
                {
                    Text = Enum.GetName(typeof(IssuePriority), i),
                    Value = i.ToString()
                });
            }
            return View(vm);
        }
    }

    
}
