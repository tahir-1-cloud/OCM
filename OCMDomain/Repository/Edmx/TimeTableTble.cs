using System;
using System.Collections.Generic;

#nullable disable

namespace OCMDomain.Repository.Edmx
{
    public partial class TimeTableTble
    {
        public int ScheduleId { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string BreakTime { get; set; }
    }
}
