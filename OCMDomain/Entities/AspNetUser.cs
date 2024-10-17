using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMDomain.Entities
{
   public class AspNetUser
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress,ErrorMessage ="Enter Valid Email")]
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password, ErrorMessage = "Password is not valid")]
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }

        [Display(Name = "Mobile Number:")]
        [Required(ErrorMessage = "Mobile Number is required.")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }

      
      
        public string UserName { get; set; }
        public int RoleId { get; set; }
        public string UserPic { get; set; }
        public string Mobile { get; set; }
        public string City { get; set; }
        public string TaxId { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? LastChangePwdDate { get; set; }
        public bool? IsActive { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Phone { get; set; }
        public bool? InsuranceAgent { get; set; }
        public string Hearfrom { get; set; }
        public byte[] ProfileImg { get; set; }
        public string Imageupload { get; set; }
        public string State { get; set; }
        public string NationalId { get; set; }
        public bool? AdminApproval { get; set; }
        public bool? AdminRejected { get; set; }

    }
}
