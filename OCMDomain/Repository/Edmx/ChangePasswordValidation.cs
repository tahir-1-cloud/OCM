using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMDomain.Repository.Edmx
{
    public class ChangePasswordValidation
    {
        public int ChangePasswordId { get; set; }


        [Required(ErrorMessage = "Current Password is requiered")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Password Is Required")]
        //[RegularExpression(@"^[^\s]+(\s+[^\s]+)*$", ErrorMessage = "Please avoid white spaces.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = " Confirm Password Is Required")]
        [Compare("NewPassword", ErrorMessage = "Password & Comfirm Password Should Be Same")]
        public string ConfirmNewPassword { get; set; }
    }
    [MetadataType(typeof(ChangePasswordValidation))]

    public partial class ChangePasswordTble
    {

    }
}
