using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMDomain.Entities
{
    public class ForgotPaswordTble
    {
        public int ForgotId { get; set; }
        [Required (ErrorMessage ="Email is required Field")]
        [DataType(DataType.EmailAddress,ErrorMessage ="Email is not Valid")]
        public string Email { get; set; }
    }
}
