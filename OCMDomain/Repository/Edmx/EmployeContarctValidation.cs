using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMDomain.Repository.Edmx
{
    public class EmployeContarctValidation
    {
        public List<EmployContractTble> EmpContractList { get; set; }


        [Required(ErrorMessage = "Email is rquierd")]
        [EmailAddress(ErrorMessage = "invalid email address")]
        [RegularExpression("^[a-za-z0-9_\\.-]+@([a-za-z0-9-]+\\.)+[a-za-z]{2,6}$", ErrorMessage = "e-mail is not valid")]
        public string Email { get; set; }

 
        [Required(ErrorMessage = "Cnic  is requiered")]
        [RegularExpression("^[0-9]{5}[0-9]{7}[0-9]$", ErrorMessage = "CNIC No must follow the XXXXXXXXXXXXX format!")]
        public int? Cnic { get; set; }

        [Required(ErrorMessage = "FullName is Requierd")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Enum is Requierd")]
        public int? Enum { get; set; }
        public string ContractNum { get; set; }
        public string ContractType { get; set; }
        public DateTime? ProbationStartDate { get; set; }
        public DateTime? ProbationEndDate { get; set; }
        public string ProbationPeriod { get; set; }
        public DateTime? JoiningDate { get; set; }
        public DateTime? ContractExpireDate { get; set; }
        public bool IsProbation { get; set; }

        [Required(ErrorMessage = "SalaryAmount is Requierd")]
        public string SalaryAmount { get; set; }
        public string SalaryType { get; set; }

        [Required(ErrorMessage = "Address is Requierd")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Mobile is Requierd")]
        public int? Mobile { get; set; }

        [NotMapped]
        public virtual EmployTble EmployTble { get; set; }
    }

    [MetadataType(typeof(EmployeValidation))]

    public partial class EmployContractTble
    {

        [NotMapped]
        public virtual EmployTble EmployTble { get; set; }
    }
}
