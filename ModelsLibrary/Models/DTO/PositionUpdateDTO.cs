using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary.Models.DTO
{
    public class PositionUpdateDTO
    {
        public int? PositionOnGrid { get; set; } = null;
        public bool IsPositioned { get; set; }
        public int? LabId { get; set; }

    }
}
