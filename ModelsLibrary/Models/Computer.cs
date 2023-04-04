using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsLibrary.Models
{
    public class Computer
    {
        public int ComputerId { get;set; }
        public string ComputerName { get;set;}
        public string Description { get;set; }
        public bool IsPositioned { get;set; }
        [ForeignKey("Lab")]
        public int? LabId { get;set; }
        public Lab? Lab { get; set; } //navigation property

        //public ValueTuple<int, int> PositionOnGrid { get;set; }
    }
}
