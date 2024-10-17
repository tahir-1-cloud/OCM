using System;
using System.Collections.Generic;

#nullable disable

namespace OCM.Repository.Edmx
{
    public partial class CourseTimeLineTble
    {
        public int Id { get; set; }
        public string Day { get; set; }
        public DateTime? StartingTime { get; set; }
        public DateTime? EndingTime { get; set; }
        public int? TimeTableId { get; set; }
    }
}
