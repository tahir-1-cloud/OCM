using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMDomain.Repository.Edmx
{
   public  class RegistraionValidation
    {
        [Required(ErrorMessage ="First Name is Requierd")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is Requierd")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is Requierd")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mobile is Requierd")]
        public int? Mobile { get; set; }

        [Required(ErrorMessage = "Address is Requierd")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Cnic is Requierd")]
        public int? Cnic { get; set; }

        [Required(ErrorMessage = "Password is Requierd")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Confirm Password is Requierd")]
        [Compare("Password",ErrorMessage ="Password do not match")]
        public string ConfirmPassword { get; set; }
       

    }

    [MetadataType(typeof(RegistraionValidation))]
    public partial class RegisterNewUserTble
    { 

    }


    }
