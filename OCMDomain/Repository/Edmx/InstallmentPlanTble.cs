using System;
using System.Collections.Generic;

#nullable disable

namespace OCMDomain.Repository.Edmx
{
    public partial class InstallmentPlanTble
    {
        public int Id { get; set; }
        public int? TotalDuration { get; set; }
        public DateTime? InstallmentDate { get; set; }
        public bool? Ispaid { get; set; }
        public string ChallanFormPath { get; set; }
        public DateTime? NextInstallmentDate { get; set; }
        public int? StudentId { get; set; }
        public int? OnlineCourseId { get; set; }
        public int? FeeId { get; set; }
    }
}
