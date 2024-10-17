using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OCMDomain.Repository.Edmx;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using OCMDomain.Entities;

namespace OCM.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class StudentController : Controller
    {
        private readonly OCMContext _db;
        private readonly EmailService emailService;
        protected UserManager<Users> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IMapper mapper;
        public StudentController(OCMContext db, EmailService emailService, UserManager<Users> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _db = db;
            this.emailService = emailService;
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult PendingStudents()
        {
            var penstd = _db.CheckUserTbles.ToList().Where(x => x.PendingStatus == true).ToList();
            var std =new List<StudentRegistrationTble>();
            if (penstd.Count > 0)
            {
                foreach (var item in penstd)
                {
                    StudentRegistrationTble stdreg = new StudentRegistrationTble();
                    var res = _db.StudentRegistrationTbles.Find(item.StudentId);
                    if (res != null)
                    {
                        stdreg.Email = res.Email;
                        stdreg.FirstName = res.FirstName;
                        stdreg.LastName = res.LastName;
                        stdreg.Gender = res.Gender;
                        stdreg.StdCnic = res.StdCnic;
                        stdreg.Mobile = res.Mobile;
                        stdreg.StudentId = res.StudentId;
                        stdreg.Address = res.Address;
                        stdreg.Dob = res.Dob;
                        stdreg.PendingStatus = true;
                        stdreg.RejectStatus = false;
                        stdreg.ApproveStatus = false;

                        stdreg.CheckUserTble = new CheckUserTble();
                        stdreg.CheckUserTble.OnlineCourseId = item.OnlineCourseId;
                        stdreg.CheckUserTble.StudentId = item.StudentId;
                        stdreg.CheckUserTble.PendingStatus = item.PendingStatus;
                        stdreg.CheckUserTble.StdChlFormPath = item.StdChlFormPath;
                        stdreg.CheckUserTble.ApproveStatus = item.ApproveStatus;
                        stdreg.CheckUserTble.RejectStatus = item.RejectStatus;
                        stdreg.CheckUserTble.ChallanIsPaid = item.ChallanIsPaid;

                        stdreg.OnlineCourse = new OnlineCourseTble();
                        stdreg.OnlineCourse.OnlineCourseId = item.OnlineCourseId;

                        var listcourse = _db.OnlineCourseTbles.Where(x=>x.OnlineCourseId ==item.OnlineCourseId).ToList();
                        ViewBag.model = listcourse;

                        std.Add(stdreg);
                    }
                }
            }
            return View(std);
        }

        public IActionResult EnrolledStudent(string Status)
        {
            if (Status == null)
            {
                var Enroll = _db.CheckUserTbles.Where(x => x.ApproveStatus == true).ToList();
                List<StudentRegistrationTble> list = new List<StudentRegistrationTble>();
                foreach (var item in Enroll)
                {
                    var std = _db.StudentRegistrationTbles.ToList().Where(x => x.StudentId == item.StudentId).FirstOrDefault();
                    if (std != null)
                    {
                        std.CheckUserTble = new CheckUserTble();
                        std.CheckUserTble = item;
                        list.Add(std);
                    }
                }
                return View(list);

            }
            return View();

        }

        // Delete Student

        [HttpGet]
        public  IActionResult DeleteStudent(int id , int Cid)
        {
            try
            {
                var DeletedStd = _db.CheckUserTbles.ToList().Where(x => x.StudentId == id && x.OnlineCourseId == Cid).FirstOrDefault();
                if (DeletedStd == null)
                {
                    TempData["response"] = "User Delete Failed !";
                    return RedirectToAction("EnrolledStudent");
                }
                else
                {
                    var result = _db.CheckUserTbles.Remove(DeletedStd);
                   
                    _db.SaveChangesAsync();
                    TempData["response"] = "Student Deleted Successfully !";
                    return RedirectToAction("EnrolledStudent");
                }
            }
            catch (Exception ex)
            {
                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("EnrolledStudent");
            }
        }

        // Reject Student
        public async Task<IActionResult> RejectedStudent(int id,int courseid)
        {
            try
            {
                var RejectedStd =_db.CheckUserTbles.ToList().Where(x=>x.StudentId==id && x.OnlineCourseId==courseid).FirstOrDefault();
                RejectedStd.RejectStatus = true;
                RejectedStd.ApproveStatus = false;
                RejectedStd.PendingStatus = false;
                _db.Entry(RejectedStd).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                 _db.SaveChanges();

                var rejstd = _db.StudentRegistrationTbles.ToList().Where(x => x.StudentId == id).FirstOrDefault();
                rejstd.PendingStatus = false;
                rejstd.RejectStatus = true;
                rejstd.ApproveStatus = false;
                _db.Entry(rejstd).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _db.SaveChanges();
                if (id != 0)
                {
                    var result = await _db.StudentRegistrationTbles.FindAsync(id);
                    bool email = emailService.SendRejectMailToStudent(result.Email, "", "", "");
                    if (email == false)
                    {
                        TempData["response"] = "Student Profile Rejected Successfully, Email Not Send By ISP";
                    }
                    else
                    {
                        TempData["response"] = "Student Profile Rejected Successfully, Email send successfully to user.";
                    }
                }
                TempData["response"] = "Rejected Successfully";
                return RedirectToAction("RejectedStudentList", "Student");
            }
            catch (Exception ex)
            {
                TempData["response"] = "Something wrong " + ex.Message;
                return RedirectToAction("RejectedStudent");
            }
        }

        public IActionResult RejectedStudentList(string status)
        {
            if (status == null)
            {
                var Enroll = _db.CheckUserTbles.Where(x => x.RejectStatus == true).ToList();
                List<StudentRegistrationTble> list = new List<StudentRegistrationTble>();
                foreach (var item in Enroll)
                {
                    var std = _db.StudentRegistrationTbles.ToList().Where(x => x.StudentId == item.StudentId).FirstOrDefault();
                    if (std != null)
                    {
                        std.CheckUserTble = new CheckUserTble();
                        std.CheckUserTble = item;
                        list.Add(std);
                    }
                }
                return View(list);

            }
            return View();
        }

        //public IActionResult BlockedStudents()
        //{
        //    var Block = _db.StudentRegistrationTbles.ToList();
        //    return View(Block);
        //}

        //Student Approve Action
        public async Task<IActionResult> ApproveStudent(int id,int courseid)
        {
            try
            {
                var Approvestd = await _db.StudentRegistrationTbles.FindAsync(id);
                var isexist = _db.CheckUserTbles.ToList().Exists(x => x.OnlineCourseId == courseid && x.StudentId == id);
                if (isexist) 
                {
                    var data = _db.CheckUserTbles.ToList().Where(x => x.OnlineCourseId == courseid && x.StudentId == id).FirstOrDefault();
                    data.ApproveStatus = true;
                    data.PendingStatus = false;
                    data.RejectStatus = false;
                    _db.Entry(data).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _db.SaveChanges();

                    var isverified = _db.StudentRegistrationTbles.ToList().Where(x => x.StudentId == id).FirstOrDefault();
                    isverified.PendingStatus = false;
                    isverified.ApproveStatus = true;
                    isverified.RejectStatus = false;
                    _db.Entry(data).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _db.SaveChanges();
                }
                Users obj = new Users();
                obj.Email = Approvestd.Email;
                obj.FirstName = Approvestd.FirstName;
                obj.LastName = Approvestd.LastName;
                obj.Cnic = Approvestd.StdCnic;
                obj.IsVerified = IsVerified.Approved;
                obj.PhoneNumber = Approvestd.Mobile;
                obj.TwoFactorEnabled = false;
                obj.LockoutEnabled = false;
                obj.EmailConfirmed = true;
                obj.City = "";
                obj.Address = Approvestd.Address;
                obj.AccessFailedCount = 0;
                obj.RoleName = "Student";
                var Id = AddAsync(obj, courseid, id);
                if (Id.ToString() != "0")
                {
                    bool email = emailService.SendApproveMailToStudent(Approvestd.Email, "", "", "http://localhost:25149/public/Security/Login");
                    if (email == false)
                    {
                        TempData["response"] = "Unhandelled Operation.Email Not Send";
                    }
                    else
                    {
                        TempData["response"] = "Student Profile Accepted Successfully, Email send successfully to user.";
                    }

                    TempData["response"] = "Student Approved Successfully";
                    return RedirectToAction("EnrolledStudent", "Student");
                }
                return RedirectToAction("EnrolledStudent", "Student");
            }
            catch (Exception ex)
            {
                TempData["response"] = "Something wrong " + ex.Message;
                return RedirectToAction("ApproveStudent");
            }
        }

        //[HttpPost]
        //public IActionResult BlockStudent(int id, string status)
        //{
        //    try
        //    {
        //        var Getstd = _db.StudentRegistrationTbles.Find(id);
        //        if (Getstd != null)
        //        {
        //            if (status == "true")
        //            {
        //                Getstd.ApproveStatus = false;
        //            }
        //            else
        //            {
        //                Getstd.ApproveStatus = true;
        //            }
        //            _db.Entry(Getstd).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        //            _db.SaveChanges();
        //            TempData["response"] = "Student Access Status Changed.";
        //            return RedirectToAction("BlockedStudents", "Student");
        //        }
        //        else
        //        {
        //            TempData["response"] = "Student Id Is Invalid.";
        //        }
        //        return View();
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["response"] = "Something wrong " + ex.Message;
        //        return View();
        //    }
        //}

        public async Task<string> AddAsync(Users model,int? courseid,int? studentid)
        {
            try
            {
                model.UserName = model.Email;
                model.Password = model.Email;
                model.Status = true;
                if (!string.IsNullOrEmpty(model.Password))
                {
                    model.PasswordHash = userManager.PasswordHasher.HashPassword(model, model.Password);
                    await userManager.CreateAsync(model, model.Password);
                }
                else
                {
                    await userManager.CreateAsync(model);

                }
                await _db.SaveChangesAsync();
                bool adminRoleExists = await roleManager.RoleExistsAsync(model.RoleName);
                if (!adminRoleExists)
                {
                    IdentityRole identityRole = new IdentityRole
                    {
                        Name = model.RoleName
                    };
                    await roleManager.CreateAsync(identityRole);
                    await _db.SaveChangesAsync();

                }
                await userManager.AddToRoleAsync(model, model.RoleName);
                await _db.SaveChangesAsync();



                var list = _db.DetailAccounts.ToList().Where(x => x.Type == "Student");
                DetailAccount items = null;
                var newAcode = "";
                if (list != null && list.Count() > 0)
                {
                    items = list.LastOrDefault();
                    if (items != null)
                    {
                        if (items.AccountCode != null)
                        {
                            var SSsplit = items.AccountCode.Split('-');
                            int code = Convert.ToInt32(SSsplit[2]) + 1;
                            newAcode = "01-001-000" + Convert.ToString(code);
                        }
                        else
                        {
                            newAcode = "01-001-0001";
                        }
                    }
                    else
                    {
                        newAcode = "01-001-0001";
                    }
                }
                else
                {
                    newAcode = "01-001-0001";
                }
                DetailAccount detailAcc = new DetailAccount();
                detailAcc.Type = "Student";
                detailAcc.Name = model.FirstName + model.LastName;
                detailAcc.UserId = studentid;
                detailAcc.AccountCode = newAcode;
                var DACCount = _db.DetailAccounts.Add(detailAcc);
                await _db.SaveChangesAsync();

         
                var currentCourseFee = _db.FeeTbles.ToList().Where(x => x.CourseId == courseid).FirstOrDefault();
                if(currentCourseFee != null)
                {
                    Receivable objrec = new Receivable();
                    objrec.Amount = currentCourseFee.FeeAmount;
                    objrec.DetailAccountId = DACCount.Entity.DetailAccountId;
                    _db.Receivables.Add(objrec);
                    await _db.SaveChangesAsync();
                }


                var GetAccount = _db.DetailAccounts.ToList().Where(x => x.UserId == studentid).FirstOrDefault();
                if (GetAccount != null)
                {
                    var NewCourseFee = _db.FeeTbles.ToList().Where(x => x.CourseId == courseid).FirstOrDefault();
                    var Recv = _db.Receivables.ToList().Where(x => x.DetailAccountId == GetAccount.DetailAccountId).FirstOrDefault();
                    if (Recv != null)
                    {
                        double total = Convert.ToDouble(Recv.Amount);
                        total += Convert.ToDouble(NewCourseFee.FeeAmount);

                        Recv.Amount = total.ToString();
                        _db.Entry(Recv).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        _db.SaveChanges();
                    }
                }
                return model.Id;
            }
            catch (Exception ex)
            {
                TempData["response"] = "Something wrong " + ex.Message;
                return "0";
            }
        }

        public IActionResult MyCourses()
        {
            var username = HttpContext.User.Identity.Name;
            var stdid = _db.StudentRegistrationTbles.Where(x => x.Email == username).ToList().FirstOrDefault().StudentId;
            var chklist = _db.CheckUserTbles.Where(c => c.StudentId == stdid).ToList();
            var courses = new List<OnlineCourseTble>();
            var onlinecourse = _db.OnlineCourseTbles.ToList();
            var courseQuota = _db.CourseQuotaTbles.ToList();
            var teacher = _db.AssignTeacherTbles.ToList();
            foreach (var item in chklist)
            {
                foreach (var onlincou in onlinecourse)
                {
                    if (item.OnlineCourseId == onlincou.OnlineCourseId)
                    {
                        OnlineCourseTble onlineCourseTble = new OnlineCourseTble();
                        onlineCourseTble = _db.OnlineCourseTbles.Where(x => x.OnlineCourseId == item.OnlineCourseId).FirstOrDefault();
                        if (onlineCourseTble != null)
                        {
                            foreach (var qouta in courseQuota)
                            {
                                if (onlineCourseTble.OnlineCourseId == qouta.CourseId)
                                {
                                    CourseQuotaTble quotaTble = new CourseQuotaTble();
                                    quotaTble = _db.CourseQuotaTbles.Where(x => x.CourseId == onlineCourseTble.OnlineCourseId).FirstOrDefault();
                                    if (quotaTble != null)
                                    {
                                        onlineCourseTble.CourseQuota = new CourseQuotaTble();
                                        onlineCourseTble.CourseQuota = quotaTble;
                                    }
                                }
                            }
                            foreach (var teach in teacher)
                            {
                                if (onlineCourseTble.OnlineCourseId == teach.CourseId)
                                {
                                    AssignTeacherTble assignTeacher = new AssignTeacherTble();
                                    assignTeacher = _db.AssignTeacherTbles.Where(x => x.CourseId == onlineCourseTble.OnlineCourseId).FirstOrDefault();
                                    if (assignTeacher != null)
                                    {
                                        onlineCourseTble.AssignTeacher = new AssignTeacherTble();
                                        onlineCourseTble.AssignTeacher = assignTeacher;
                                    }
                                }
                            }
                            courses.Add(onlineCourseTble);
                        }
                    }
                }
            }
            return View(courses);
        }


        [HttpGet]
        public async Task<IActionResult> UploadedLectureMaterial(int Id)
        {
            var checkr = _db.InstallmentPlanTbles.Where(x => x.FeeId == x.FeeId).ToList();
            
            foreach(var str in checkr)
            {

                //DateTime start = str.InstallmentDate.Value;
                //DateTime end = str.NextInstallmentDate.Value.Date;
                //var instalend = end.ToShortDateString();
               var start= str.InstallmentDate;
               var end = str.NextInstallmentDate;
                DateTime? current = DateTime.Now;
                if (start <= current && current <= end )
                {
                    if (str.Ispaid == true)
                    {

                        ModelClass model = new ModelClass();
                        model.OCT = await _db.OnlineCourseTbles.FindAsync(Id);
                        model.ListOCT = _db.OnlineCourseTbles.ToList();
                        //model.ListcourseMaterials =  _db.CourseMaterialTbles.Where(x => x.OnlineCourseId == Id).ToList();
                        var Lectures = _db.CourseMaterialTbles.ToList().Where(x => x.OnlineCourseId == Id && x.FileApprove == true).ToList();
                        model.ListcourseMaterials = new List<CourseMaterialTble>();
                        foreach (var item in Lectures)
                        {
                            model.ListcourseMaterials.Add(item);
                        }
                        var courseQuota = _db.CourseQuotaTbles.ToList();
                        for (int i = 0; i < model.ListOCT.Count(); i++)
                        {
                            foreach (var item in courseQuota.ToList().Where(x => x.CourseId == model.ListOCT[i].OnlineCourseId).ToList())
                            {
                                model.ListOCT[i].CourseQuota = item;
                            }
                        }
                        return View(model);


                    }
                    else
                    {
                        var std = _db.StudentRegistrationTbles.Where(x => x.StudentId == x.StudentId).FirstOrDefault();
                        return RedirectToAction("UploadChallan", "Security", new { Area = "Public", email = std.Email, cid = Id });
                      //  break;
                    }

                }
                else if (str.Ispaid==false)
                {
                    var std = _db.StudentRegistrationTbles.Where(x => x.StudentId == x.StudentId).FirstOrDefault();
                    return RedirectToAction("UploadChallan", "Security", new { Area = "Public", email = std.Email, cid = Id });
                    //break;
                }

            }
            
            return View();
           
         

        }

        public IActionResult CheckStudentCourseOutline(int id)
        {

            var getcot = _db.CourseOutlineTbles.Where(x => x.OnlineCourseId == id).FirstOrDefault();
            if (getcot != null)
            {
                if (getcot.CourseTitle != null)
                {
                    return View(getcot);
                }

            }
            return View();
        }









        //public async Task<IActionResult> CourseMaterial(int id)
        //{
        //    ModelClass model = new ModelClass();
        //    List<CheckUserTble> list = new List<CheckUserTble>();
        //    var stdlist = _db.StudentRegistrationTbles.ToList();
        //    var chkusertable = _db.CheckUserTbles.Where(x => x.OnlineCourseId == id).ToList();
        //    List<StudentRegistrationTble> stdreg = new List<StudentRegistrationTble>();
        //    foreach (var item in chkusertable)
        //    {
        //        var studentid = item.StudentId;
        //        var studentobj = stdlist.Where(x => x.StudentId == studentid).FirstOrDefault();

        //        if (studentobj != null)
        //        {
        //            stdreg.Add(studentobj);
        //        }


        //    }
        //    model.ListSrt = stdreg;
        //    model.OCT = await _db.OnlineCourseTbles.FindAsync(id);
        //    model.ListOCT = _db.OnlineCourseTbles.ToList();

        //    //model.Cut = await _context.CheckUserTbles.FindAsync(id);
        //    //model.listCut = await _context.CheckUserTbles.ToListAsync();


        //    var courseQuota = _db.CourseQuotaTbles.ToList();
        //    //var result = _context.CheckUserTbles.Where(x => x.OnlineCourseId == id).ToListAsync();
        //    for (int i = 0; i < model.ListOCT.Count(); i++)
        //    {
        //        foreach (var item in courseQuota.ToList().Where(x => x.CourseId == model.ListOCT[i].OnlineCourseId).ToList())
        //        {
        //            model.ListOCT[i].CourseQuota = item;
        //        }

        //    }
        //    return View(model);
        //}


        //public async Task<IActionResult> MyCourseDescription(int id)
        //{
        //    ModelClass model = new ModelClass();
        //    model.OCT = await _context.OnlineCourseTbles.FindAsync(id);
        //    model.ListOCT = await _context.OnlineCourseTbles.ToListAsync();
        //    var courseQuota = _context.CourseQuotaTbles.ToList();
        //    for (int i = 0; i < model.ListOCT.Count(); i++)
        //    {
        //        foreach (var item in courseQuota.ToList().Where(x => x.CourseId == model.ListOCT[i].OnlineCourseId).ToList())
        //        {
        //            model.ListOCT[i].CourseQuota = item;
        //        }
        //    }
        //    return View(model);
        //}
    }
}
