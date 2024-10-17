using System;
using System.Collections.Generic;

#nullable disable

namespace OCMDomain.Repository.Edmx
{
    public partial class CourseMaterialTble
    {
        public int CourseId { get; set; }
        public int? OnlineCourseId { get; set; }
        public string Filename { get; set; }
        public string FileType { get; set; }
        public DateTime? UploadDate { get; set; }
        public string FilePath { get; set; }
        public bool? FileApprove { get; set; }
        public bool? FilePending { get; set; }
        public bool? FileReject { get; set; }
        public string VideoPath { get; set; }
        public string ImagePath { get; set; }
        public string VideoTitle { get; set; }
        public string VideoDescription { get; set; }
    }
}
