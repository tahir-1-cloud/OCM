using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMDomain.Repository.Edmx
{
   public class LabTableValidation
    {
        public int LabId { get; set; }
        [Required(ErrorMessage = "Department Is Required")]
        public string Department { get; set; }
        [Required(ErrorMessage = "LabName Is Required")]
        public string LabName { get; set; }
        [Required(ErrorMessage = "AssistantName Is Required")]
        public string AssistantName { get; set; }
        [Required(ErrorMessage = "RoomNo Is Required")]

        public int? RoomNo { get; set; }
        [Required(ErrorMessage = "SittingCapacity Is Required")]
        public int? SittingCapacity { get; set; }
    }
}
