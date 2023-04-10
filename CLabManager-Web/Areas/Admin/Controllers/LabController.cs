using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.SignalR.Protocol;
using ModelsLibrary.Models;
using ModelsLibrary.Models.DTO;
using ModelsLibrary.Models.ViewModels;
using ModelsLibrary.Utilities;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace CLabManager_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LabController : Controller
    {
        public async Task<IActionResult> Create(int? LabId)
        {
            CreateLabVM vm = new CreateLabVM();
            string url = $"https://localhost:7138/api/Labs/{LabId}";
            if (LabId != null)
            {
                using(var httpClient = new HttpClient())
                {
                    
                    using (var response = await httpClient.GetAsync(url))
                    {
                        if (response.StatusCode.Equals(HttpStatusCode.OK))
                        {
                            var res = await response.Content.ReadAsStringAsync();
                            vm.Lab = JsonConvert.DeserializeObject<Lab>(res);
                        }
                        else
                        {
                            vm.Lab = null;
                        }
                    }
                }
            }
            
            url = "https://localhost:7138/api/Computers/unassigned";
            using (var httpClient = new HttpClient())
            {
                using(var response = await httpClient.GetAsync(url))
                {
                    var apiResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    var computers = JsonConvert.DeserializeObject<List<Computer>>(apiResponse);
                    vm.UnassignedComputers = computers;
                }
            }
            Array values = Enum.GetValues(typeof(GridType));
            vm.items = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            foreach (var i in values)
            {
                vm.items.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Text = Enum.GetName(typeof(GridType), i),
                    Value = i.ToString()
                });
            }
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLab(LabCreationDTO dto)
        {
            var url = "https://localhost:7138/api/Labs";
            Lab lab;
            var obj = new
            {
                roomNo= dto.RoomNo,
                buildingNo= dto.BuildingNo,
                gridType= dto.GridType,
                status= dto.Status
            };
            StringContent content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync(url, content))
                {
                    var apiResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    lab = JsonConvert.DeserializeObject<Lab>(apiResponse);
                }
            }
            return RedirectToAction("Create", new {LabId = lab.LabId});
        }

        //public ActionResult CreateComputer(string computerName, string computerDesc)
        //{
        //    var obj = new
        //    {
        //        computerName = computerName,
        //        description = computerDesc
        //    };
        //    var url = "https://localhost:7138/api/Computers";

        //    using (var httpClient = new HttpClient())
        //    {
        //        StringContent stringContent = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
        //        using(var response = httpClient.PostAsync(url, stringContent).GetAwaiter().GetResult())
        //        {
        //            if (response.StatusCode == System.Net.HttpStatusCode.Created)
        //            { 

        //            }
        //        }
        //    }
        //    return RedirectToAction("Create");
        //}
    }
}
