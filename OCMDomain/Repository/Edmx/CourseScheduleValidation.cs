using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMDomain.Repository.Edmx
{
    public class CourseScheduleValidation
    {
        public int TimeTableId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string BreakTime { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public int? CourseId { get; set; }
    }
    [MetadataType(typeof(CourseScheduleValidation))]

    public partial class CourseScheduleTble
    {
        //[NotMapped]
        //public CourseTimeLineTble CourseTimeTble { get; set; }
        [NotMapped]
        public CourseTimeLineTble CourseTimeLine { get; set; }
        [NotMapped]
        public List<CourseTimeLineTble> listCourseTimeTble { get; set; }
    }
}
