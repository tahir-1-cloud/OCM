using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMDomain.Repository.Edmx
{
   public class FaqValidation
    {
        public int FaqId { get; set; }

        [Required(ErrorMessage ="Title is Required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is Required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Date is Required")]
        public DateTime? Date { get; set; }
        public string CreatedBy { get; set; }

      
    }
}
