using System;
using System.Collections.Generic;

#nullable disable

namespace OCMDomain.Repository.Edmx
{
    public partial class FeeTble
    {
        public int FeeId { get; set; }
        public string FeeAmount { get; set; }
        public string PerCreditHour { get; set; }
        public bool? IsInstallementAllow { get; set; }
        public int? CourseId { get; set; }
        public string CourseName { get; set; }
        public DateTime? DueDate { get; set; }
        public string AddedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? MonthlyInstallment { get; set; }
        public int? OnlineCourseId { get; set; }
    }
}
