using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMDomain.Repository.Edmx
{
    public class AssetTableValidation
    { 
    
        public int AssetId { get; set; }
        [Required(ErrorMessage = "AssetName Is Required")]
        public string AssetName { get; set; }
        [Required(ErrorMessage = "AssetQuantity Is Required")]
        public int? AssetQuantity { get; set; }
        [Required(ErrorMessage = "DamagedAsset Is Required")]
        public int? DamagedAsset { get; set; }
        public string SelectLab { get; set; }
    }
}
