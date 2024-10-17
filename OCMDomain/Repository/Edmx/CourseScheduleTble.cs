using System;
using System.Collections.Generic;

#nullable disable

namespace OCMDomain.Repository.Edmx
{
    public partial class CourseScheduleTble
    {
        public int TimeTableId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string BreakTime { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public int? CourseId { get; set; }
    }
}
