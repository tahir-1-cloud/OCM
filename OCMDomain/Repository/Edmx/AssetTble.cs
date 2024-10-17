using System;
using System.Collections.Generic;

#nullable disable

namespace OCMDomain.Repository.Edmx
{
    public partial class AssetTble
    {
        public int AssetId { get; set; }
        public string AssetName { get; set; }
        public int? AssetQuantity { get; set; }
        public int? DamagedAsset { get; set; }
        public string SelectLab { get; set; }
    }
}
