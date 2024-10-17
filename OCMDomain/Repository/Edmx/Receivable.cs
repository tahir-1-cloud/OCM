using System;
using System.Collections.Generic;

#nullable disable

namespace OCMDomain.Repository.Edmx
{
    public partial class Receivable
    {
        public int RecvId { get; set; }
        public string Amount { get; set; }
        public int? DetailAccountId { get; set; }

        public virtual DetailAccount DetailAccount { get; set; }
    }
}
