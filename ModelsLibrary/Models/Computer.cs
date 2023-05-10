using ModelsLibrary.Utilities;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsLibrary.Models
{
    public class Computer
    {
        public int ComputerId { get;set; }
        public string ComputerName { get;set;}
        public string Description { get;set; }
        public bool IsPositioned { get;set; }
        public GridType GridType { get; set; }
        public int? PositionOnGrid { get; set; } = null;
        [ForeignKey("Lab")]
        public int? LabId { get;set; }
        public Lab? Lab { get; set; } //navigation property

        //public ValueTuple<int, int> PositionOnGrid { get;set; }
    }
}
