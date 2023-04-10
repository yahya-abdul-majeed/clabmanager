using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ModelsLibrary.Models.ViewModels
{
    public class CreateLabVM
    {
        public Lab? Lab { get; set; } = null;
        public List<Computer> UnassignedComputers { get;set; }
        public List<SelectListItem> items { get; set; }
    }
}
