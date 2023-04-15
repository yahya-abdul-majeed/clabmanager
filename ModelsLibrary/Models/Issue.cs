using ModelsLibrary.Utilities;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsLibrary.Models
{
    public class Issue
    {
        public int IssueId { get; set; }
        public IssuePriority Priority { get; set; }
        public string Title { get; set; }   
        public string Description { get; set; } 
        public IssueState State { get; set; }
        [ForeignKey("Computer")]
        public int ComputerId { get; set; } 
        public Computer Computer { get; set; }
        [ForeignKey("Lab")]
        public int LabId { get; set; }
        public Lab Lab { get;set; }

        //submitted by: user
    }
}
