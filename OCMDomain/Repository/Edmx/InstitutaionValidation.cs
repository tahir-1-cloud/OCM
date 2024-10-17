using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMDomain.Repository.Edmx
{
   public class InstitutaionValidation
    {
        public int InstituteId { get; set; }


        [Required(ErrorMessage = "Name is requierd")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Registration Number is requierd ")]
        public string RegistrationNo { get; set; }

        [Required(ErrorMessage = "Phone is requierd ")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email Number is requierd ")]

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Address is requierd ")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Logo is requierd ")]
        public byte[] Logo { get; set; }
        public string LogoByPath { get; set; }

        [Required(ErrorMessage = "Date is requierd ")]
        public string StartedYear { get; set; }

        [Required(ErrorMessage = "Owner Name is requierd ")]
        public string OwnerName { get; set; }

        [Required(ErrorMessage = "Number of Students is requierd ")]
        public string NoofStudents { get; set; }

        [Required(ErrorMessage = "Number of Courses is requierd ")]
        public string NoofCourses { get; set; }
        public bool? Aolevel { get; set; }
        public bool? IsBoard { get; set; }
        public string Board { get; set; }

        //[Required(ErrorMessage ="Please Select Logo")]
       // public IFormFile CoverPhoto { get; set; }
    }

    [MetadataType(typeof(InstitutaionValidation))]

    public partial class InstitutionTble
    {

    }
}
