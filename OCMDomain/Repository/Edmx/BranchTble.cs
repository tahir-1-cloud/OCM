using System;
using System.Collections.Generic;

#nullable disable

namespace OCMDomain.Repository.Edmx
{
    public partial class BranchTble
    {
        public int BranchId { get; set; }
        public int? InstituteId { get; set; }
        public string InstituteName { get; set; }
        public string BranchName { get; set; }
        public string BranchPhone { get; set; }
        public string BranchEmail { get; set; }
        public string BranchAddress { get; set; }
        public string Owner { get; set; }
        public string OwnerMobile { get; set; }
        public string OwnerEmail { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string AddedBy { get; set; }
        public string NoofStudents { get; set; }
        public string NoofCourses { get; set; }
    }
}
