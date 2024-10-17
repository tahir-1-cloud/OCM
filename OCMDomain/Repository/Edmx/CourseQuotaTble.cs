using System;
using System.Collections.Generic;

#nullable disable

namespace OCMDomain.Repository.Edmx
{
    public partial class CourseQuotaTble
    {
        public int CourseQuotaId { get; set; }
        public string NoofStudents { get; set; }
        public DateTime? CreateDate { get; set; }
        public string BatchName { get; set; }
        public int? BatchId { get; set; }
        public int? CourseId { get; set; }
        public string CourseName { get; set; }
        public string RemainingSeats { get; set; }
    }
}
