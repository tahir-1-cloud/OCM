using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace OCMDomain.Repository.Edmx
{
    public partial class CourseTimeLineTble
    {
        public int Id { get; set; }
        public string Day { get; set; }
        public DateTime? StartingTime { get; set; }
        public DateTime? EndingTime { get; set; }
        public int? TimeTableId { get; set; }
        public int? CourseId { get; set; }


    }
}
