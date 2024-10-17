using System;
using System.Collections.Generic;

#nullable disable

namespace OCMDomain.Repository.Edmx
{
    public partial class StudentRegistrationTble
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string Gender { get; set; }
        public DateTime? Dob { get; set; }
        public bool? PendingStatus { get; set; }
        public bool? ApproveStatus { get; set; }
        public bool? RejectStatus { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }
        public string PasswordHash { get; set; }
        public byte[] StdChallanForm { get; set; }
        public string StdChlFormPath { get; set; }
        public string StdCnic { get; set; }
    }
}
