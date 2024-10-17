using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMDomain.Repository.Edmx
{
    public class OnlineCourseValidation
    {
        public int OnlineCourseId { get; set; }

        [Required(ErrorMessage = "Course Name is requierd")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Course code is requierd")]
        public int? Code { get; set; }

        [Required(ErrorMessage = "Credit Hours is requierd")]
        public string CreditHours { get; set; }


        [Required(ErrorMessage = "Description is requierd")]
        [Range(2 , 5)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Type is requierd")]
        public string CourseType { get; set; }
        [NotMapped]
        public virtual CourseQuotaTble CourseQuota { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "Course Fee is requierd")]
        public virtual FeeTble CourseFee { get; set; }

        
        public IFormFile CoverPhoto { get; set; }
        public IFormFile CoverPhoto1 { get; set; }

        [NotMapped]
        public virtual EmployTble EmployTble { get; set; }
        [NotMapped]
        public virtual AssignTeacherTble AssignTeacher { get; set; }

    }

    [MetadataType(typeof(OnlineCourseValidation))]

    public partial class OnlineCourseTble
    {
        [NotMapped]
        public virtual CourseQuotaTble CourseQuota { get; set; }
        [NotMapped]

        public virtual FeeTble CourseFee { get; set; }

        [NotMapped]
        public IFormFile CoverPhoto { get; set; }

        [NotMapped]
        public IFormFile CoverPhoto1 { get; set; }

        [NotMapped]     
        public virtual EmployTble EmployTble { get; set; }
        [NotMapped]
        public virtual TimeTableTble TimeTble { get; set; }
        [NotMapped]
        public virtual AssignTeacherTble AssignTeacher { get; set; }
        [NotMapped]
        public virtual CourseScheduleTble CourseSchedule { get; set; }
        [NotMapped]
        public virtual List<CourseScheduleTble> listCourseSchedule { get; set; }
        [NotMapped]
        public virtual CourseTimeLineTble CourseTimeLine { get; set; }
        [NotMapped]
        public virtual List<CourseTimeLineTble> listCourseTimeLine { get; set; }
        [NotMapped]
        public virtual CourseOutlineTble CourseOutlineTble { get; set; }
        [NotMapped]
        public virtual List<CourseOutlineTble> ListCourseOutlineTble { get; set; }

        //[NotMapped]
        //public virtual List<StudentRegistrationTble> StudentRegistrationTble { get; set; }
        [NotMapped]
        public virtual StudentRegistrationTble StudentRegistrationTble { get; set; }
        
    }
}