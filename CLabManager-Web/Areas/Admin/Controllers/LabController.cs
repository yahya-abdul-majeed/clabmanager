using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ModelsLibrary.Models;
using ModelsLibrary.Models.DTO;
using ModelsLibrary.Models.ViewModels;
using ModelsLibrary.Utilities;
using Newtonsoft.Json;
using NToastNotify;
using System.Net;
using System.Reflection.Emit;
using System.Text;

namespace CLabManager_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LabController : Controller
    {
        private readonly IToastNotification _toastNotification;
        public LabController(IToastNotification toastNotification)
        {
            _toastNotification = toastNotification;
        }
        public void Redirector(int? LabId)
        {
            Response.Redirect($"https://localhost:7183/Admin/Lab/Create?LabId={LabId}");
        }
        public async Task<IActionResult> Create(int? LabId)
        {
            if (SD.getPrincipal().Identity == null)
                return RedirectToAction("AccessDenied", "Authentication", new { Area = "User" });
            if (SD.getPrincipal().IsInRole("User"))
                return RedirectToAction("AccessDenied", "Authentication", new { Area = "User" });
            CreateLabVM vm = new CreateLabVM();
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Request.Cookies[SD.XAccessToken]);
            string url = $"https://localhost:7138/api/Labs/{LabId}";
            if (LabId != null)
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

            url = "https://localhost:7138/api/Computers/unassigned";
            using (var response = await httpClient.GetAsync(url))
            {
                var apiResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var computers = JsonConvert.DeserializeObject<List<Computer>>(apiResponse);
                vm.UnassignedComputers = computers;
            }
            //lab select
            url = "https://localhost:7138/api/Labs";
            using (var response = await httpClient.GetAsync(url))
            {
                var apiResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var labs = JsonConvert.DeserializeObject<List<Lab>>(apiResponse);
                labs = labs.OrderBy(p => p.BuildingNo).ThenBy(p => p.RoomNo).ToList();
                vm.Labs = new List<SelectListItem>();
                foreach (var i in labs)
                {
                    vm.Labs.Add(new SelectListItem
                    {
                        Text = "Building " + i.BuildingNo.ToString() + " Room " + i.RoomNo.ToString(),
                        Value = i.LabId.ToString()
                    });
                }
            }

            //gridType select
            Array values = Enum.GetValues(typeof(GridType));
            vm.items = new List<SelectListItem>();
            foreach (var i in values)
            {
                vm.items.Add(new SelectListItem
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
            if (SD.getPrincipal().IsInRole("User"))
                return RedirectToAction("AccessDenied", "Authentication", new { Area = "User" });
            var url = "https://localhost:7138/api/Labs";
            Lab lab = new Lab();
            var obj = new
            {
                roomNo = dto.RoomNo,
                buildingNo = dto.BuildingNo,
                gridType = dto.GridType,
                status = dto.Status
            };
            StringContent content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Request.Cookies[SD.XAccessToken]);
                using (var response = await httpClient.PostAsync(url, content))
                {
                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        var apiResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        lab = JsonConvert.DeserializeObject<Lab>(apiResponse);
                        _toastNotification.AddSuccessToastMessage("Lab Created");
                    }
                    else
                    {
                        _toastNotification.AddErrorToastMessage("Lab Creation failed");
                        return RedirectToAction("Create");
                    }
                }
            }
            return RedirectToAction("Create", new { LabId = lab.LabId });
        }

        public async Task<IActionResult> DeleteLab(int? LabId)
        {
            if (LabId == 0 || LabId ==null)
            {
                return RedirectToAction(nameof(Create), new { LabId = (int?)null });
            }
            using(var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Request.Cookies[SD.XAccessToken]);

                using (var response = await httpClient.DeleteAsync($"https://localhost:7138/api/Labs/{LabId}"))
                {
                    if(response.IsSuccessStatusCode)
                    {
                        _toastNotification.AddSuccessToastMessage("Lab deleted");
                        return RedirectToAction(nameof(Create), new {LabId = (int?)null});
                    }
                    else
                    {
                        _toastNotification.AddErrorToastMessage("Lab delete failed");
                        
                        return Redirect(Request.Headers["Referer"].ToString());
                    }
                }
            }
        }

        public async Task<IActionResult> DeleteComputer(int compId)
        {
            using(var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Request.Cookies[SD.XAccessToken]);
                using(var response = await httpClient.DeleteAsync($"https://localhost:7138/api/computers/{compId}"))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        _toastNotification.AddErrorToastMessage("Delete failed");
                    }
                    return Redirect(Request.Headers["Referer"].ToString());
                }
            }
        }
    }
}
