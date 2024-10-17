using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMDomain.Repository.Edmx
{
    public partial class Roles 
    {
        [Required(ErrorMessage = "Please select Role")]
        public string Name { get; set; }
        public string Id { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string NormalizedName { get; set; }
    }
}
