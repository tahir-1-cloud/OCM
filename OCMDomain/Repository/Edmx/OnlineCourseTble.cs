using System;
using System.Collections.Generic;

#nullable disable

namespace OCMDomain.Repository.Edmx
{
    public partial class OnlineCourseTble
    {
        public int OnlineCourseId { get; set; }
        public string Name { get; set; }
        public int? Code { get; set; }
        public byte[] Logo { get; set; }
        public string Description { get; set; }
        public string LogoByPath { get; set; }
        public string CourseType { get; set; }
        public string CreditHours { get; set; }
    }
}
