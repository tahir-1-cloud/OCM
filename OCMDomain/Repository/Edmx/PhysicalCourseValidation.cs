using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMDomain.Repository.Edmx
{
    public class PhysicalCourseValidation
    {
        [Required(ErrorMessage ="Course Name is requierd")]
        public string CourseName { get; set; }

        [Required(ErrorMessage = "Course code is requierd")]
        public string CourseCode { get; set; }
        public string CoursePhoto { get; set; }

        [Required(ErrorMessage = "Description is requierd")]
        public string Description { get; set; }

       
        public string BatchName { get; set; }
        public int? BatchId { get; set; }

        [Required(ErrorMessage = "TotalFee is requierd")]
        public double? TotalFee { get; set; }

        [Required(ErrorMessage = "FeePerCredit is requierd")]
        public string FeePerCredit { get; set; }
        public DateTime? DueDate { get; set; }
    }

    [MetadataType(typeof(PhysicalCourseValidation))]

    public partial class PhysicalCourseTble
    {
    }
}