using ModelsLibrary.Utilities;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsLibrary.Models.DTO
{
    public class IssueDTO
    {
        public int IssueId { get; set; }
        public IssuePriority Priority { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IssueState State { get; set; }
        [ForeignKey("Computer")]
        public int ComputerId { get; set; }
    }
}
