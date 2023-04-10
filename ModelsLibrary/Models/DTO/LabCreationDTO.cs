using ModelsLibrary.Utilities;

namespace ModelsLibrary.Models.DTO
{
    public class LabCreationDTO
    {
        public int RoomNo { get; set; }
        public int BuildingNo { get; set; }
        public GridType GridType { get; set; }
        public LabStatus Status { get; set; }
    }
}
