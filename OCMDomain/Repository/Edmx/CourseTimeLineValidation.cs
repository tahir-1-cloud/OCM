using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMDomain.Repository.Edmx
{
  public  class CourseTimeLineValidation
    {
        public int Id { get; set; }
        public string Day { get; set; }
        public DateTime? StartingTime { get; set; }
        public DateTime? EndingTime { get; set; }
        public int? TimeTableId { get; set; }
        public int? CourseId { get; set; }
        [NotMapped]
        public string StartinghTime { get; set; }
        [NotMapped]
        public string EndinghTime { get; set; }
        [NotMapped]
        public string BreakTime { get; set; }

        //public string StartinghTime { get; set; }
        //public string EndinghTime { get; set; }

    }
    [MetadataType(typeof(CourseTimeLineValidation))]
    public partial class CourseTimeLineTble
    {
        //[NotMapped]
        //public string StartinghTime { get; set; }
        //[NotMapped]
        //public string EndinghTime { get; set; }
        [NotMapped]
        public string BreakTime { get; set; }
    }
}
