using System;
using System.Collections.Generic;

#nullable disable

namespace OCM.Repository.Edmx
{
    public partial class UpdateUserTble
    {
        public int UpdateUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int? Mobile { get; set; }
        public string Address { get; set; }
        public int? Cnic { get; set; }
        public string Type { get; set; }
    }
}
