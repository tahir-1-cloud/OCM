using System;
using System.Collections.Generic;

#nullable disable

namespace OCM.Repository.Edmx
{
    public partial class ChangePasswordTble
    {
        public int ChangePasswordId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
