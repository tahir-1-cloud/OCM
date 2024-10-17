using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace OCMDomain.Repository.Edmx
{
    public class EmployeValidation
    {
        public List<EmployTble> EmployList { get; set; }

        [Required(ErrorMessage = "First Name is requiered")]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [Required(ErrorMessage = "LastName  is requiered")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Cnic  is requiered")]
        [RegularExpression("^[0-9]{5}[0-9]{7}[0-9]$", ErrorMessage = "CNIC No must follow the XXXXXXXXXXXXX format!")]
        public string EmpCnic { get; set; }

        [Required(ErrorMessage = "Driving Licence  is requiered")]

        public string DrivingLicence { get; set; }

        public string Gender { get; set; }

        //[Required(ErrorMessage = "Number  is requiered")]

        public int? EmpNumber { get; set; }

        [Required(ErrorMessage = "Address is requiered")]
        public string EmpAddress { get; set; }

        [Required(ErrorMessage = "email is rquierd")]
        [EmailAddress(ErrorMessage = "invalid email address")]
        [RegularExpression("^[a-za-z0-9_\\.-]+@([a-za-z0-9-]+\\.)+[a-za-z]{2,6}$", ErrorMessage = "e-mail is not valid")]

        public string EmpEmail { get; set; }

        //[Required(ErrorMessage = " Phone is requiered")]
        public int? EmpPhoneNum { get; set; }

        [Required(ErrorMessage = " Mobile is requiered")]
        public string EmpMobileNum { get; set; }

        public DateTime? EmpDob { get; set; }
        public string EmpMaritalStatus { get; set; }


        public string EmpExperience { get; set; }
        public string EmpQualification { get; set; }
        public string EmpSpecializedDegree { get; set; }
        public DateTime? EmpDegreeYear { get; set; }
        public byte[] EmpDegree { get; set; }
        public string EmpDegrePath { get; set; }
        public byte[] EmpExperienceLetter { get; set; }

        [Required(ErrorMessage = "Exper Letter Path is requiered")]
        public string EmpExperLetterPath { get; set; }
        public string EmpType { get; set; }
        public int? RefrenceId { get; set; }

        [Required(ErrorMessage = "Ref Name  is requiered")]
        public string RefName { get; set; }

        [Required(ErrorMessage = "Mobile  is requiered")]
        public string RefMobileNum { get; set; }

        [Required(ErrorMessage = "Email is rquierd")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string RefEmail { get; set; }

        [Required(ErrorMessage = "Address  is requiered")]
        public string RefAddress { get; set; }
       
        public IFormFile CoverPhoto { get; set; }
        public IFormFile CoverPhoto1 { get; set; }



    }

    [MetadataType(typeof(EmployeValidation))]

    public partial class EmployTble
    {
        [NotMapped]       
        public IFormFile CoverPhoto { get; set; }

        [NotMapped]
        public IFormFile CoverPhoto1 { get; set; }
        [NotMapped]
        public string FullName { get; set; }

        public void CopyTo(FileStream fileStream)
        {
            throw new NotImplementedException();
        }
    }
}