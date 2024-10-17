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
    public class UserValidate
    {
        [Required(ErrorMessage ="First Name Is Required")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email Is Required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password Is Required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Password Is Required")]
        [Compare("Password", ErrorMessage ="Password & Comfirm Password Should Be Same")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Mobile Number Is Required")]
        public string PhoneNumber { get; set; }
        public string RoleName { get; set; }
        public string Rate { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public int UserTitleId { get; set; }
        public string ParentId { get; set; }
        public byte[] profile_img { get; set; }

    }
}
