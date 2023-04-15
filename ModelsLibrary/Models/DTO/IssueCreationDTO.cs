using System.ComponentModel.DataAnnotations.Schema;
using ModelsLibrary.Utilities;

namespace ModelsLibrary.Models.DTO
{
    public class IssueCreationDTO
    {
        public IssuePriority Priority { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IssueState State { get; set; }
        [ForeignKey("Computer")]
        public int ComputerId { get; set; }
        public int LabId { get; set; }
    }
}
