using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary.Models.ViewModels
{
    public class IssueIndexVM
    {
        public List<Issue> Issues { get; set; }
        public List<SelectListItem> Items { get; set; }
        public List<SelectListItem> PItems { get; set; }
    }
}
