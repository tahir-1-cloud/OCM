using System;
using System.Collections.Generic;

#nullable disable

namespace OCM.Repository.Edmx
{
    public partial class EmployTble
    {
        public int EmpId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string EmpCnic { get; set; }
        public string DrivingLicence { get; set; }
        public string Gender { get; set; }
        public string EmpNumber { get; set; }
        public string EmpAddress { get; set; }
        public string EmpEmail { get; set; }
        public int? EmpPhoneNum { get; set; }
        public string EmpMobileNum { get; set; }
        public DateTime? EmpDob { get; set; }
        public string EmpMaritalStatus { get; set; }
        public string EmpExperience { get; set; }
        public string EmpQualification { get; set; }
        public string EmpSpecializedDegree { get; set; }
        public DateTime? EmpDegreeYear { get; set; }
        public byte[] EmpDegree { get; set; }
        public string EmpDegrePath { get; set; }
        public byte[] EmpExperienceLetter { get; set; }
        public string EmpExperLetterPath { get; set; }
        public string EmpType { get; set; }
        public int? RefrenceId { get; set; }
        public string RefName { get; set; }
        public string RefMobileNum { get; set; }
        public string RefEmail { get; set; }
        public string RefAddress { get; set; }
        public bool? ApproveStatus { get; set; }
        public bool? PendingStatus { get; set; }
        public bool? RejectStatus { get; set; }
    }
}
