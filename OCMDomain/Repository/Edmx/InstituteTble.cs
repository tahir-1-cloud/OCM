using System;
using System.Collections.Generic;

#nullable disable

namespace OCMDomain.Repository.Edmx
{
    public partial class InstituteTble
    {
        public int InstituteId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Phonesecond { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string RegistrationNo { get; set; }
        public string NoofBranches { get; set; }
        public string Startyear { get; set; }
    }
}
