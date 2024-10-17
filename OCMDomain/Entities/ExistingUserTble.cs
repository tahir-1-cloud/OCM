using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMDomain.Entities
{
    public  class ExistingUserTble
    {
        public int UserId { get; set; }
        public int SerNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Type { get; set; }
        public int Mobile { get; set; }
        public int Cnic { get; set; }
    }
}
