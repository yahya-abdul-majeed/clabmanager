using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsLibrary.Models.DTO
{
    public class ComputerDTO
    {
        [Key]
        public int ComputerId { get; set; }
        public string ComputerName { get; set; }
        public string Description { get; set; }
        public bool IsPositioned { get; set; }
        public int? LabId { get; set; }
    }
}
