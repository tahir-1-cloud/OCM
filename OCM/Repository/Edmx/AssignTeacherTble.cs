using System;
using System.Collections.Generic;

#nullable disable

namespace OCM.Repository.Edmx
{
    public partial class AssignTeacherTble
    {
        public int AssignId { get; set; }
        public int? CourseId { get; set; }
        public int? EmpId { get; set; }
        public DateTime? AssignDate { get; set; }
        public string AssignBy { get; set; }
        public string TeacherName { get; set; }
    }
}
