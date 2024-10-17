using System;
using System.Collections.Generic;

#nullable disable

namespace OCM.Repository.Edmx
{
    public partial class InstitutionTble
    {
        public int InstituteId { get; set; }
        public string Name { get; set; }
        public string RegistrationNo { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public byte[] Logo { get; set; }
        public string LogoByPath { get; set; }
        public string StartedYear { get; set; }
        public string OwnerName { get; set; }
        public string NoofStudents { get; set; }
        public string NoofCourses { get; set; }
        public bool? Aolevel { get; set; }
        public bool? IsBoard { get; set; }
        public string Board { get; set; }
    }
}
