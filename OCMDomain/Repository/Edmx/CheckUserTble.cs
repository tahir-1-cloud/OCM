using System;
using System.Collections.Generic;

#nullable disable

namespace OCMDomain.Repository.Edmx
{
    public partial class CheckUserTble
    {
        public int Id { get; set; }
        public int? CourseQuotaId { get; set; }
        public int StudentId { get; set; }
        public int OnlineCourseId { get; set; }
        public bool? PendingStatus { get; set; }
        public bool? ApproveStatus { get; set; }
        public bool? RejectStatus { get; set; }
        public bool? ChallanIsPaid { get; set; }
        public string StdChlFormPath { get; set; }
    }
}
