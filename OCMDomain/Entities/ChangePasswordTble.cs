using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMDomain.Entities
{
    public class ChangePasswordTble
    {
        public int ChangePasswordId { get; set; }

        [Required (ErrorMessage ="Currenet Password is Requierd")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "New Password is Requierd")]
        public string NewPassword { get; set; }
      

        [Required(ErrorMessage = " Confirm New Password is Requierd")]
        [Compare("NewPassword", ErrorMessage = "Password do not match")]
         public string ConfirmNewPassword { get; set; }
    }
}
