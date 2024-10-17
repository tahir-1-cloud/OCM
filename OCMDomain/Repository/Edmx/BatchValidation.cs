using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMDomain.Repository.Edmx
{
   public class BatchValidation
    {
        public int BatchId { get; set; }

        [Required(ErrorMessage ="Batch Name Is Required.")]
        public string BtachName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<BatchTble> BatchNameList { get; set; }

    }
    [MetadataType(typeof(BatchValidation))]
    public partial class BatchTble
    {

    }
   }
