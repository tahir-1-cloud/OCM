using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMDomain.Repository.Edmx
{
    public class ResetPasswordValidation
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Email is Requierd")]
        public string Email { get; set; }
        public bool? EmailSent { get; set; }

        [Required(ErrorMessage = "Password Is Required")]
        [RegularExpression(@"^[^\s]+(\s+[^\s]+)*$", ErrorMessage = "Please avoid white spaces.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password Is Required")]
        [Compare("Password", ErrorMessage = "Password & Comfirm Password Should Be Same")]
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
    }
    [MetadataType(typeof(ResetPasswordValidation))]

    public partial class ResetPassword
    {

       
    }
}
