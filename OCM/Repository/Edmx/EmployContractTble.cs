using System;
using System.Collections.Generic;

#nullable disable

namespace OCM.Repository.Edmx
{
    public partial class EmployContractTble
    {
        public int ConId { get; set; }
        public int? EmpId { get; set; }
        public string Email { get; set; }
        public string Cnic { get; set; }
        public string FullName { get; set; }
        public int? Enum { get; set; }
        public string ContractNum { get; set; }
        public string ContractType { get; set; }
        public DateTime? ProbationStartDate { get; set; }
        public DateTime? ProbationEndDate { get; set; }
        public string ProbationPeriod { get; set; }
        public DateTime? JoiningDate { get; set; }
        public DateTime? ContractExpireDate { get; set; }
        public bool? IsProbation { get; set; }
        public string SalaryAmount { get; set; }
        public string SalaryType { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
    }
}
