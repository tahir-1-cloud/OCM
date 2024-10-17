using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMDomain.Entities
{
     
    public class ContactUsTble
    {
        public int ContacUsId { get; set; }
        [Required(ErrorMessage = "Frist Name is Required!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is Required!")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email  is Required!")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Subject is Required Field!")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "  Message is Required Field!")]
        public string Message { get; set; }
    }
}
