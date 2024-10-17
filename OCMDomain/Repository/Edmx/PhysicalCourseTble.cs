using System;
using System.Collections.Generic;

#nullable disable

namespace OCMDomain.Repository.Edmx
{
    public partial class PhysicalCourseTble
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public string Description { get; set; }
        public string BatchName { get; set; }
        public int? BatchId { get; set; }
        public double? TotalFee { get; set; }
        public string FeePerCredit { get; set; }
        public DateTime? DueDate { get; set; }
        public string CoursePhoto { get; set; }
    }
}
