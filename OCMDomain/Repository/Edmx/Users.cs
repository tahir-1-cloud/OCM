using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMDomain.Repository.Edmx
{
    public enum IsVerified
    {
        Pending = 1,
        Approved = 2,
        Rejected = 3,
    }
    public class Users : IdentityUser
    {
        [Required(ErrorMessage = "First Name Is Required")]
        [RegularExpression(@"^[^\s]+(\s+[^\s]+)*$", ErrorMessage = "Please avoid white spaces.")]
        public string FirstName { get; set; }
        [RegularExpression(@"^[^\s]+(\s+[^\s]+)*$", ErrorMessage = "Please avoid white spaces.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Last Name Is Required")]
        [RegularExpression(@"^[^\s]+(\s+[^\s]+)*$", ErrorMessage = "Please avoid white spaces.")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Email Is Required")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Please enter a valid email address.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Password Is Required")]
        [Compare("Password", ErrorMessage = "Password & Comfirm Password Should Be Same")]
        [RegularExpression(@"^[^\s]+(\s+[^\s]+)*$", ErrorMessage = "Please avoid white spaces.")]
        [NotMapped]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Address Is Required")]
        [RegularExpression(@"^[^\s]+(\s+[^\s]+)*$", ErrorMessage = "Please avoid white spaces.")]
        public string Address { get; set; }
        public string RoleName { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        [Required(ErrorMessage = "Cnic  is requiered")]
        [RegularExpression("^[0-9]{5}[0-9]{7}[0-9]$", ErrorMessage = "CNIC No Must Have 13 Digits(Wthout Dashes)")]
        public string Cnic { get; set; }
        public string ImagePath { get; set; }
        public byte[] profile_img { get; set; }
        public bool Status { get; set; }
        public IsVerified IsVerified { get; set; }
        public string ParentId { get; set; }
        public string CaseRate { get; set; }

        public int? UserTitleId { get; set; }
        [NotMapped]
        public string newPassword { get; set; }
    }
}
