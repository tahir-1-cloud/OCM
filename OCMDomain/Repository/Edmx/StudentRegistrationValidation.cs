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
    public class StudentRegistrationValidation
    {
        public int StudentId { get; set; }
        [Required(ErrorMessage = "First Name is Required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is Required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = " Email is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Address  is Required")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Mobile  is Required")]
        public string Mobile { get; set; }
        [Required(ErrorMessage = "Gender  is Required")]

        public string Gender { get; set; }
        [Required(ErrorMessage = "Date of Birth  is Required")]
        public DateTime? Dob { get; set; }
        [Required(ErrorMessage = "Cnic  is requiered")]
        [RegularExpression("^[0-9]{5}[0-9]{7}[0-9]$", ErrorMessage = "CNIC/Bay-Form No Must Have 13 Digits(Wthout Dashes)")]
        public string StdCnic { get; set; }
        public IFormFile CoverPhoto { get; set; }
        public byte[] StdChallanForm { get; set; }
        [Required(ErrorMessage = "Challan Form Path is requiered")]
        public string StdChlFormPath { get; set; }
        [NotMapped]
        public virtual OnlineCourseTble OnlineCourse { get; set; }
        [NotMapped]
        public virtual CheckUserTble CheckUserTble { get; set; }
    }
    [MetadataType(typeof(StudentRegistrationValidation))]

    public partial class StudentRegistrationTble
    {
        [NotMapped]
        public IFormFile CoverPhoto { get; set; }

        [NotMapped]
        public virtual OnlineCourseTble OnlineCourse { get; set; }

        [NotMapped]
        public virtual CourseQuotaTble CourseQuota { get; set; }
        [NotMapped]
        public virtual CheckUserTble CheckUserTble { get; set; }
    }
}
