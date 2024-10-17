using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace OCM.Repository.Edmx
{
    public partial class CheckUserTble
    {
        public int StudentId { get; set; }
        public int? CourseQuotaId { get; set; }
        public int OnlineCourseId { get; set; }
        public int Id { get; set; }
        public bool? PendingStatus { get; set; }
        public bool? ApproveStatus { get; set; }
        public bool? RejectStatus { get; set; }
        public bool? ChallanIsPaid { get; set; }
        public string StdChlFormPath { get; set; }
    }
    

}
