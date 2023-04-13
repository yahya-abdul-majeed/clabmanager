using Microsoft.AspNetCore.Mvc.Rendering;
using ModelsLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary.Models.ViewModels
{
    public class LabDetailVM
    {
        public Lab Lab { get; set; }    
        public List<SelectListItem> priorities { get; set; }
    }
}
