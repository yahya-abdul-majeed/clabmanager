using ModelsLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary.Models.DTO
{
    public class IssueUpdateDTO
    {
        public int IssueId { get; set; }
        public IssuePriority Priority { get; set; }
        public IssueState State { get; set; }
    }
}
