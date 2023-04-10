using ModelsLibrary.Models.DTO;
using ModelsLibrary.Utilities;

namespace ModelsLibrary.Models
{
    public class Lab
    {
        public int LabId { get;set; }
        public int RoomNo { get; set; } 
        public int BuildingNo { get;set; }
        public LabStatus Status { get; set; }
        public GridType GridType { get; set; }
        public ICollection<ComputerDTO> ComputerList { get; set; }
    }
}
