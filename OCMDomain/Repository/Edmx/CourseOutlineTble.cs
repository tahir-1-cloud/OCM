using System;
using System.Collections.Generic;

#nullable disable

namespace OCMDomain.Repository.Edmx
{
    public partial class CourseOutlineTble
    {
        public int Id { get; set; }
        public string CourseTitle { get; set; }
        public string CourseDescription { get; set; }
        public int? OnlineCourseId { get; set; }
        public int? EmpId { get; set; }
        public int? BatchId { get; set; }
    }
}
