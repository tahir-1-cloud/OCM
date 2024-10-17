using System;
using System.Collections.Generic;

#nullable disable

namespace OCMDomain.Repository.Edmx
{
    public partial class Voucher
    {
        public int VoucherId { get; set; }
        public string VoucherNumber { get; set; }
        public string AccountName { get; set; }
        public string AccountCode { get; set; }
        public string Debit { get; set; }
        public string Credit { get; set; }
        public string Amount { get; set; }
        public DateTime? Date { get; set; }
        public int? DetailAccountId { get; set; }
        public string ClosingBalance { get; set; }

        public virtual DetailAccount DetailAccount { get; set; }
    }
}
