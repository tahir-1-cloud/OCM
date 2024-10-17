using System;
using System.Collections.Generic;

#nullable disable

namespace OCMDomain.Repository.Edmx
{
    public partial class FaqsTble
    {
        public int FaqId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
        public string CreatedBy { get; set; }
    }
}
