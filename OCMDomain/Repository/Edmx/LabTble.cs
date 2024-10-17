using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace OCMDomain.Repository.Edmx
{
    public partial class LabTble
    {
        public int LabId { get; set; }
        public string Department { get; set; }
        public string LabName { get; set; }
        public string AssistantName { get; set; }
        public int? RoomNo { get; set; }
        public int? SittingCapacity { get; set; }
    }
    public partial class AssetTble
    {
        [NotMapped]
        public virtual LabTble labtab { get; set; }
    }
}
