using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMDomain.Repository.Edmx
{
   public class ContactUsValidation
    {
        [Required(ErrorMessage = "First Name is rquierd")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is rquierd")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is rquierd")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Subject is rquierd")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Message is rquierd")]
        public string Message { get; set; }

    }
    [MetadataType(typeof(ContactUsValidation))]

    public partial class ContactUsTble
    { }
}
