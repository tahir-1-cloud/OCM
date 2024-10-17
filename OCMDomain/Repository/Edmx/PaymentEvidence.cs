using System;
using System.Collections.Generic;

#nullable disable

namespace OCMDomain.Repository.Edmx
{
    public partial class PaymentEvidence
    {
        public int PaymentevidenceId { get; set; }
        public byte[] InvoiceScreenShot { get; set; }
        public int? BookingId { get; set; }
        public bool? Status { get; set; }
        public DateTime? AppliedDate { get; set; }
        public DateTime? VerfiedDate { get; set; }
    }
}
