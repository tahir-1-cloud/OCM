using System;
using System.Collections.Generic;

#nullable disable

namespace OCMDomain.Repository.Edmx
{
    public partial class DetailAccount
    {
        public DetailAccount()
        {
            Receivables = new HashSet<Receivable>();
            Vouchers = new HashSet<Voucher>();
        }

        public int DetailAccountId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string AccountCode { get; set; }
        public int? UserId { get; set; }

        public virtual ICollection<Receivable> Receivables { get; set; }
        public virtual ICollection<Voucher> Vouchers { get; set; }
    }
}
