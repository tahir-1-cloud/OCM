using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OCMDomain.Repository.Edmx;

namespace OCMDomain.Entities
{
    public class ModelClass
    {
        public OnlineCourseTble OCT { get; set; }
        public List<OnlineCourseTble> ListOCT { get; set; }
        public List<CourseMaterialTble> ListcourseMaterials { get; set; }
        public StudentRegistrationTble Srt  { get; set; }
        public List<StudentRegistrationTble> ListSrt { get; set; }
        public CheckUserTble Cut { get; set; }
        public List<CheckUserTble> listCut { get; set; }
        public CourseMaterialTble CourseMT { get; set; }
        public List<CourseMaterialTble> Cmt { get; set; }
        public CourseOutlineTble CourseOT { get; set; }
        public List<CourseOutlineTble> Cot { get; set; }
        public CourseQuotaTble CourseQt { get; set; }
        public List<CourseQuotaTble> ListCqt { get; set; }
        public AssignTeacherTble AssignTT { get; set; }
        public List<AssignTeacherTble> Att { get; set; }
    }
}
