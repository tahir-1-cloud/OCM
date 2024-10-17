using System;
using System.Collections.Generic;

#nullable disable

namespace OCM.Repository.Edmx
{
    public partial class ContactUsTble
    {
        public int ContacUsId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
