using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMDomain.Entities
{
   public class RegisterNewUserTble
    {
        public int RegisterNewUserId { get; set; }
        [Required (ErrorMessage ="FirstName is requierd Field")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "LastName is requierd Field")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is requierd Field")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mobile is requierd Field")]
        public int? Mobile { get; set; }

        [Required(ErrorMessage = "Address is requierd Field")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Cnic is requierd Field")]
        public int? Cnic { get; set; }
        [Required(ErrorMessage = "Password is requierd Field")]
        [DataType(DataType.Password, ErrorMessage = "Password is not valid")]
        public string Password { get; set; }
        [Required(ErrorMessage = "ConfirmPassword is requierd Field")]
        [DataType(DataType.Password,ErrorMessage ="Password is not valid")]
        [Compare("Password",ErrorMessage ="Password do not match")]
        public string ConfirmPassword { get; set; }
     
        [Required(ErrorMessage = "SelectRole is requierd Field")]
        public string SelectRole { get; set; }
    }
}
