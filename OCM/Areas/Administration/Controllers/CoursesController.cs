using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OCMDomain.Repository.Edmx;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Mvc.Rendering;
using Grpc.Core;
using System.Globalization;

namespace OCM.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class CoursesController : Controller
    {

        private readonly OCMContext _db;

        protected IHostingEnvironment _environment;


        public CoursesController(OCMContext db, IHostingEnvironment environment)
        {
            _db = db;
            _environment = environment;
        }

        public IActionResult GetEmploye()
        {
            var employe = _db.EmployTbles.ToList().Where(x => x.EmpType == "Trainer").ToList();

            return Json(employe);
        }

        public IActionResult AssignedTeacher()
        {
            return View();
        }
        [HttpPost]
        public string AssignedTeacher(EmployTble employ, int CourseId, int BatchId)
        {
            var hasExistId = new CourseQuotaTble();
            hasExistId = null;
            var courseQouta = _db.CourseQuotaTbles.ToList();
            var coreses = _db.OnlineCourseTbles.ToList();
            //var teacher = _db.AssignTeacherTbles.Where(x=>x.EmpId==employ.).ToList();
            var ABC = _db.AssignTeacherTbles.ToList().Where(x => x.EmpId == employ.EmpId && x.CourseId == CourseId).ToList();
            if (ModelState.IsValid)
            {
                if (ABC.Count != 0)
                {
                    foreach (var item in ABC)
                    {
                        hasExistId = courseQouta.ToList().Where(x => x.CourseId == item.CourseId && x.BatchId == BatchId).FirstOrDefault();
                    }
                }
                else
                {
                    foreach (var item in _db.AssignTeacherTbles.ToList())
                    {
                        hasExistId = courseQouta.ToList().Where(x => x.CourseId == item.CourseId && x.BatchId == BatchId).FirstOrDefault();
                    }
                }
                if (hasExistId != null)
                {
                    TempData["response"] = "Teacher has already assigned";
                    return "already assign";
                }
                AssignTeacherTble obj = new AssignTeacherTble();
                obj.EmpId = employ.EmpId;
                obj.TeacherName = employ.FirstName;
                if (CourseId > 0)
                {
                    obj.CourseId = CourseId;
                }
                obj.AssignBy = "Admin";
                obj.AssignDate = DateTime.Now;
                _db.AssignTeacherTbles.Add(obj);
                _db.SaveChanges();
                TempData["response"] = "Teacher Assigned Successfully";
                return "true";
            }
            return "false";
        }
        public async Task<IActionResult> CourseSchedule()
        {

            var courses = new List<OnlineCourseTble>();
            var onlinecourse = await _db.OnlineCourseTbles.ToListAsync();
            var courseQuota = _db.CourseQuotaTbles.ToList();
            var courseFee = _db.FeeTbles.ToList();
            for (int i = 0; i < onlinecourse.Count(); i++)
            {
                foreach (var item in courseQuota.ToList().Where(x => x.CourseId == onlinecourse[i].OnlineCourseId).ToList())
                {
                    onlinecourse[i].CourseQuota = item;
                }
                foreach (var item in courseFee.ToList().Where(x => x.CourseId == onlinecourse[i].OnlineCourseId).ToList())
                {
                    onlinecourse[i].CourseFee = item;
                }
            }
            return View(onlinecourse);
        }


        public async Task<IActionResult> AddPhysicalCourse()
        {

            try
            {
                var str = _db.BatchTbles.ToList();
                var Allbatches = new List<BatchTble>();
                foreach (var item in str)
                {
                    DateTime? start = item.StartDate;
                    var end = item.EndDate.Value;
                    DateTime? current = DateTime.Now;
                    if (start <= current && current <= end)
                    {
                        Allbatches.Add(item);
                    }
                }
                ViewBag.Batch = new SelectList(Allbatches.ToList(), "BatchId", "BtachName");
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Something wrong " + ex.Message;
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPhysicalCourse(OnlineCourseTble onlineCourse)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (onlineCourse.CoverPhoto != null)
                    {
                        var file = onlineCourse.CoverPhoto;
                        string filePathbar;
                        string uniqueFileName = null;

                        string path1 = @"wwwroot";
                        string fullPath;
                        fullPath = Path.GetFullPath(path1);
                        string uploadsFolder = fullPath + "/Courses/Physical";
                        uniqueFileName = Guid.NewGuid().ToString() + "" + ".PNG";
                        filePathbar = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var fileStream = new FileStream(filePathbar, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        onlineCourse.LogoByPath = "~/Courses/Physical/" + "" + uniqueFileName;
                    }
                    onlineCourse.CourseType = "Physical";
                    var method = _db.OnlineCourseTbles.Where(x => x.Name == onlineCourse.Name || x.Code == onlineCourse.Code).FirstOrDefault();
                    if (method != null)
                    {
                        TempData["response"] = "Course with same name and code already exists";
                        return RedirectToAction("AddPhysicalCourse");

                    }
                    else
                    {
                        var NewCourse = _db.OnlineCourseTbles.Add(onlineCourse);
                        await _db.SaveChangesAsync();
                        FeeTble fees = new FeeTble();
                        fees.CourseId = NewCourse.Entity.OnlineCourseId;
                        fees.CourseName = NewCourse.Entity.Name;
                        if (onlineCourse.CourseFee != null)
                        {
                            fees.DueDate = onlineCourse.CourseFee.DueDate;
                            fees.IsInstallementAllow = onlineCourse.CourseFee.IsInstallementAllow;
                            fees.PerCreditHour = onlineCourse.CourseFee.PerCreditHour;
                            fees.FeeAmount = onlineCourse.CourseFee.FeeAmount;
                            _db.FeeTbles.Add(fees);
                            await _db.SaveChangesAsync();

                        }
                        CourseQuotaTble Quota = new CourseQuotaTble();
                        if (onlineCourse.CourseQuota != null)
                        {
                            if (onlineCourse.CourseQuota.BatchId != null)
                            {
                                Quota.BatchId = onlineCourse.CourseQuota.BatchId;
                                var getBatch = _db.BatchTbles.Find(onlineCourse.CourseQuota.BatchId);
                                Quota.BatchName = getBatch.BtachName;
                            }
                            Quota.CourseId = NewCourse.Entity.OnlineCourseId;
                            Quota.CourseName = NewCourse.Entity.Name;
                            Quota.CreateDate = DateTime.Now;
                            Quota.NoofStudents = onlineCourse.CourseQuota.NoofStudents;
                            _db.CourseQuotaTbles.Add(Quota);
                            await _db.SaveChangesAsync();
                            TempData["response"] = "Physical course Added Successfully";
                            return RedirectToAction("CourseSchedule", "Courses");

                        }
                        else
                        {
                            TempData["response"] = "Validate Required " + ModelState.Values;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["response"] = "Something wrong " + ex.Message;
                return View();
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> DeletePCourse(int? Id)
        {
            if (Id > 0)
            {
                var ondata = await _db.OnlineCourseTbles.FindAsync(Id);
                if (ondata != null)
                {
                    var feeid = ondata.OnlineCourseId;
                    var feedata = _db.FeeTbles.Where(x => x.CourseId == feeid).FirstOrDefault();
                    if (feedata != null)
                    {
                        _db.FeeTbles.Remove(feedata);
                        await _db.SaveChangesAsync();
                    }
                    var coudata = _db.CourseQuotaTbles.Where(x => x.CourseId == feeid).FirstOrDefault();
                    if (coudata != null)
                    {
                        _db.CourseQuotaTbles.Remove(coudata);
                        await _db.SaveChangesAsync();
                    }
                    _db.OnlineCourseTbles.Remove(ondata);
                    _db.SaveChanges();
                }
                else
                {
                    TempData["response"] = "Failed!";
                    return RedirectToAction("CourseSchedule", "Courses");
                }
            }
            TempData["response"] = "Successfully Deleted !";
            return RedirectToAction("CourseSchedule", "Courses");
        }

        public async Task<IActionResult> AddOnlineCourse()
        {
            try
            {
                var str = _db.BatchTbles.ToList();
                var Allbatches = new List<BatchTble>();
                foreach (var item in str)
                {
                    DateTime? start = item.StartDate;
                    var end = item.EndDate.Value;
                    DateTime? current = DateTime.Now;
                    if (start <= current && current <= end)
                    {
                        Allbatches.Add(item);
                    }
                }
                ViewBag.Batch = new SelectList(Allbatches.ToList(), "BatchId", "BtachName");
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Something wrong " + ex.Message;
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOnlineCourse(OnlineCourseTble onlineCourse)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (onlineCourse.CoverPhoto1 != null)
                    {
                        var file = onlineCourse.CoverPhoto1;
                        string filePathbar;
                        string uniqueFileName = null;

                        string path1 = @"wwwroot";
                        string fullPath;
                        fullPath = Path.GetFullPath(path1);
                        string uploadsFolder = fullPath + "/Courses/Physical";
                        uniqueFileName = Guid.NewGuid().ToString() + "" + ".PNG";
                        filePathbar = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var fileStream = new FileStream(filePathbar, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        onlineCourse.LogoByPath = "~/Courses/Physical/" + "" + uniqueFileName;
                    }

                    onlineCourse.CourseType = "Online";
                    var method = _db.OnlineCourseTbles.Where(x => x.Name == onlineCourse.Name || x.Code == onlineCourse.Code).FirstOrDefault();
                    if (method != null)
                    {
                        TempData["response"] = "Course with same name and code already exists";
                        return RedirectToAction("AddOnlineCourse");

                    }
                    else
                    {
                        var NewCourse = _db.OnlineCourseTbles.Add(onlineCourse);
                        await _db.SaveChangesAsync();
                        FeeTble fees = new FeeTble();
                        fees.CourseId = NewCourse.Entity.OnlineCourseId;
                        fees.CourseName = NewCourse.Entity.Name;
                        if (onlineCourse.CourseFee != null)
                        {
                            fees.DueDate = onlineCourse.CourseFee.DueDate;
                            fees.IsInstallementAllow = onlineCourse.CourseFee.IsInstallementAllow;
                            fees.PerCreditHour = onlineCourse.CourseFee.PerCreditHour;
                            fees.FeeAmount = onlineCourse.CourseFee.FeeAmount;
                            _db.FeeTbles.Add(fees);
                            await _db.SaveChangesAsync();

                        }
                        CourseQuotaTble Quota = new CourseQuotaTble();
                        if (onlineCourse.CourseQuota != null)
                        {
                            if (onlineCourse.CourseQuota.BatchId != null)
                            {
                                Quota.BatchId = onlineCourse.CourseQuota.BatchId;
                                var getBatch = _db.BatchTbles.Find(onlineCourse.CourseQuota.BatchId);
                                Quota.BatchName = getBatch.BtachName;
                            }
                            Quota.CourseId = NewCourse.Entity.OnlineCourseId;
                            Quota.CourseName = NewCourse.Entity.Name;
                            Quota.CreateDate = DateTime.Now;
                            Quota.NoofStudents = onlineCourse.CourseQuota.NoofStudents;
                            _db.CourseQuotaTbles.Add(Quota);
                            await _db.SaveChangesAsync();
                            TempData["response"] = "Online course Added Successfully";
                            return RedirectToAction("CourseSchedule", "Courses");
                        }
                        else
                        {
                            TempData["response"] = "Validate Required " + ModelState.Values;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                TempData["response"] = "Something wrong " + ex.Message;
                return View();
            }
            return View();
        }

        [HttpGet]
        public IActionResult EditPhycourse(int id)
        {
            OnlineCourseTble obj = new OnlineCourseTble();
            var Allbatches = _db.BatchTbles.ToList();
            ViewBag.Batch = new SelectList(Allbatches.ToList(), "BatchId", "BtachName");
            var course = _db.OnlineCourseTbles.ToList().Where(c => c.OnlineCourseId == id).FirstOrDefault();
            if (course != null)
            {
                var cdata = _db.FeeTbles.ToList().Where(x => x.CourseId == course.OnlineCourseId).FirstOrDefault();
                var qdata = _db.CourseQuotaTbles.ToList().Where(x => x.CourseId == course.OnlineCourseId).FirstOrDefault();
                if (cdata != null)
                {
                    course.CourseFee = new FeeTble();
                    course.CourseFee.FeeAmount = cdata.FeeAmount;
                    course.CourseFee.PerCreditHour = cdata.PerCreditHour;
                    course.CourseFee.IsInstallementAllow = cdata.IsInstallementAllow;
                    course.CourseFee.CourseName = cdata.CourseName;
                    course.CourseFee.DueDate = cdata.DueDate;
                    //string hhh = cdata.DueDate.Value.ToShortDateString();           
                    //course.CourseFee.DueDate = DateTime.ParseExact(hhh, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    course.CourseQuota = new CourseQuotaTble();
                    course.CourseQuota.NoofStudents = qdata.NoofStudents;
                    course.CourseQuota.CreateDate = qdata.CreateDate;
                    course.CourseQuota.BatchId = qdata.BatchId;
                    course.CourseQuota.CourseName = qdata.CourseName;
                    course.CourseQuota.BatchName = qdata.BatchName;
                }
                return View(course);
            }
            return Content("Course Type Is not same");
        }
        [HttpPost]
        public async Task<IActionResult> EditPhycourse(int? id, OnlineCourseTble model, DateTime duedate )
        {
            if (id != null)
            {
                try
                {
                    var course = _db.OnlineCourseTbles.AsQueryable().ToList().Where(e => e.OnlineCourseId == id).FirstOrDefault();
                    var tt = course.CourseType;
                    if (model.Name != null)
                    {
                        course.Name = model.Name;
                    }
                    if (model.CourseType != null)
                    {
                        course.CourseType = model.CourseType;
                    }
                    if (model.CoverPhoto != null)
                    {
                        var file = model.CoverPhoto;
                        string filePathbar;
                        string uniqueFileName = null;
                        string path1 = @"wwwroot";
                        string fullPath;
                        fullPath = Path.GetFullPath(path1);
                        string uploadsFolder = fullPath + "/Courses/Physical";
                        uniqueFileName = Guid.NewGuid().ToString() + "" + ".PNG";
                        filePathbar = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var fileStream = new FileStream(filePathbar, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }
                        model.LogoByPath = "~/Courses/Physical/" + "" + uniqueFileName;
                    }
                    if (model.Code != null)
                    {
                        course.Code = model.Code;
                    }
                    if (model.CourseType != null)
                    {
                        course.CourseType = model.CourseType;
                    }
                    if (model.Description != null)
                    {
                        course.Description = model.Description;
                    }
                    if (model.CreditHours != null)
                    {
                        course.CreditHours = model.CreditHours;
                    }
                    _db.Entry(course).State = EntityState.Modified;
                    var data = await _db.SaveChangesAsync();
                    var cdata = _db.FeeTbles.Where(x => x.CourseId == course.OnlineCourseId).FirstOrDefault();
                    if (model.CourseFee != null)
                    {
                        if (model.CourseFee.FeeAmount != null)
                        {
                            cdata.FeeAmount = model.CourseFee.FeeAmount;
                        }
                        if (model.CourseFee.PerCreditHour != null)
                        {
                            cdata.PerCreditHour = model.CourseFee.PerCreditHour;
                        }
                        if (model.CourseFee.IsInstallementAllow != null)
                        {
                            cdata.IsInstallementAllow = model.CourseFee.IsInstallementAllow;
                        }
                        if (model.CourseFee.CourseName != null)
                        {
                            cdata.CourseName = cdata.CourseName;
                        }
                        if (model.CourseFee.DueDate != null)
                        {
                            cdata.DueDate = model.CourseFee.DueDate;
                        }
                        _db.Entry(cdata).State = EntityState.Modified;
                        var ccdata = await _db.SaveChangesAsync();
                    }
                    var qdata = _db.CourseQuotaTbles.Where(x => x.CourseId == course.OnlineCourseId).FirstOrDefault();
                    if (model.CourseQuota != null)
                    {
                        var getbatch = _db.BatchTbles.Find(model.CourseQuota.BatchId);
                        if (model.CourseQuota.BatchName != null)
                        {
                            qdata.BatchName = getbatch.BtachName;
                        }
                        if (model.CourseQuota.CourseName != null)
                        {
                            qdata.CourseName = qdata.CourseName;
                        }
                        if (model.CourseQuota.CreateDate != null)
                        {
                            qdata.CreateDate = DateTime.Now;
                        }
                        if (model.CourseQuota.NoofStudents != null)
                        {
                            qdata.NoofStudents = model.CourseQuota.NoofStudents;
                        }
                        _db.Entry(qdata).State = EntityState.Modified;
                        await _db.SaveChangesAsync();
                    }
                    TempData["response"] = "updated sucessfully";
                    return RedirectToAction("CourseSchedule", "Courses");
                }
                catch (Exception ex)
                {
                    TempData["response"] = "failed!" + ex.Message; ;
                    return RedirectToAction("CourseSchedule", "Courses");
                }
            }
            return NotFound();
        }

        public async Task<IActionResult> Detail(int? id)
        {
            try
            {
                var detail = await _db.OnlineCourseTbles.FindAsync(id);
                if (detail == null)
                {
                    return RedirectToAction("CourseSchedule", "Courses");
                }
                //var courses = new List<OnlineCourseTble>();
                //var onlinecourse = await _db.OnlineCourseTbles.ToListAsync();
                var courseQuota = _db.CourseQuotaTbles.ToList();
                var courseFee = _db.FeeTbles.ToList();

                var model = await _db.CourseQuotaTbles.Where(x => x.CourseId == detail.OnlineCourseId).ToListAsync();
                var countstudent = 0;
                foreach (var item in model)
                {
                    var ut = await _db.CheckUserTbles.Where(x => x.OnlineCourseId == id).ToListAsync();
                    foreach (var std in ut)
                    {
                        StudentRegistrationTble studentRegistrationTble = new StudentRegistrationTble();
                        studentRegistrationTble = await _db.StudentRegistrationTbles.FindAsync(std.StudentId);
                        if (studentRegistrationTble != null && studentRegistrationTble.ApproveStatus == true)
                        {
                            countstudent++;
                        }
                    }
                    var totalseats = Convert.ToInt32(item.NoofStudents);
                    if (countstudent < totalseats)
                    {
                        detail.CourseQuota = new CourseQuotaTble();
                        detail.CourseQuota.NoofStudents = Convert.ToString(totalseats);
                        detail.CourseQuota.CourseName = item.CourseName;
                        detail.CourseQuota.BatchId = item.BatchId;
                        detail.CourseQuota.BatchName = item.BatchName;
                        detail.CourseQuota.RemainingSeats = Convert.ToString(totalseats - countstudent);
                    }
                }
                var coursetimelinetable = _db.CourseTimeLineTbles.ToList();
                var courseschedule = _db.CourseScheduleTbles.ToList();
                //for (int i = 0; i < onlinecourse.Count(); i++)
                //{
                //}

                //foreach (var item in courseQuota.ToList().Where(x => x.CourseId == detail.OnlineCourseId).ToList())
                //{
                //    detail.CourseQuota = item;
                //}
                var geteacher = _db.AssignTeacherTbles.ToList().Where(x => x.CourseId == detail.OnlineCourseId).FirstOrDefault();
                if (geteacher != null)
                {
                    if (geteacher.TeacherName != null)
                    {
                        detail.AssignTeacher = geteacher;
                    }
                    
                }

                foreach (var item in courseFee.ToList().Where(x => x.CourseId == detail.OnlineCourseId).ToList())
                {
                    detail.CourseFee = item;
                }

                var item1 = courseschedule.Where(x => x.CourseId == detail.OnlineCourseId).ToList();
                foreach (var item in item1.ToList())
                {
                    item.CourseTimeLine = coursetimelinetable.Where(x => x.TimeTableId == item.TimeTableId).FirstOrDefault();
                    item.listCourseTimeTble = coursetimelinetable.Where(x => x.TimeTableId == item.TimeTableId).ToList();
                }
                foreach (var item in courseschedule.ToList().Where(x => x.CourseId == detail.OnlineCourseId).ToList())
                {
                    // detail.CourseSchedule=item;
                    var timetableid = item1.Where(x => x.TimeTableId == item.TimeTableId).FirstOrDefault();
                    item.listCourseTimeTble = coursetimelinetable.Where(x => x.TimeTableId == timetableid.TimeTableId).ToList();
                    detail.listCourseTimeLine = item.listCourseTimeTble;
                    detail.CourseTimeLine = item.CourseTimeLine;
                }
                var coursesech = courseschedule.ToList().Where(x => x.CourseId == detail.OnlineCourseId).ToList();
                OnlineCourseTble onlineCourseTble = new OnlineCourseTble();
                onlineCourseTble.listCourseSchedule = coursesech;
                detail.listCourseSchedule = onlineCourseTble.listCourseSchedule;
                if (detail.CourseSchedule != null)
                {
                    var timetable = _db.CourseTimeLineTbles.Where(x => x.TimeTableId == detail.CourseSchedule.TimeTableId).ToList();
                    onlineCourseTble.listCourseTimeLine = timetable;
                }
              
                return View(detail);
              
            }
            catch (Exception ex)
            {
                TempData["response"] = "Something wrong " + ex.Message;
                return View();
            }
        }

        public IActionResult TeacherList()
        {
            var list = _db.AssignTeacherTbles.ToList();
            return View(list);
        }
        public IActionResult AssignTeacher()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignTeacher(AssignTeacherTble assign)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AssignTeacherTble teacher = new AssignTeacherTble();

                    teacher.TeacherName = assign.TeacherName;
                    teacher.AssignDate = assign.AssignDate;
                    teacher.AssignBy = assign.AssignBy;
                    _db.AssignTeacherTbles.Add(teacher);
                    await _db.SaveChangesAsync();
                    TempData["response"] = "Batch Added Successfully !";
                    return RedirectToAction("TeacherList");
                }
            }
            catch (Exception ex)
            {
                TempData["response"] = "Something wrong " + ex.Message;

            }
            return View();

        }

        public async Task<IActionResult> DeleteTeacher(int Id)
        {
            if (Id > 0)
            {
                var ondata = await _db.AssignTeacherTbles.FindAsync(Id);
                if (ondata != null)
                {
                    var schid = ondata.AssignId;
                    var ttdata = _db.AssignTeacherTbles.Where(x => x.AssignId == schid).FirstOrDefault();
                    if (ttdata != null)
                    {
                        _db.AssignTeacherTbles.Remove(ttdata);
                        await _db.SaveChangesAsync();
                    }
                    //var coudata = _db.EmployTbles.Where(x => x.EmpId == schid).FirstOrDefault();
                    //if (coudata != null)
                    //{
                    //    _db.EmployTbles.Remove(coudata);
                    //    await _db.SaveChangesAsync();
                    //}
                }
                else
                {
                    TempData["response"] = "Failed!";
                    return RedirectToAction("CourseSchedule", "Courses");
                }
            }
            TempData["response"] = "Successfully Deleted !";
            return RedirectToAction("Detail");
        }

        [HttpGet]
        public IActionResult TimeTable()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TimeTable(OnlineCourseTble tt)
        {
            try
            {
                if (tt != null)
                {
                    CourseScheduleTble timeTable = new CourseScheduleTble();
                    timeTable.BreakTime = tt.CourseSchedule.BreakTime;
                    timeTable.CourseId = tt.OnlineCourseId;
                    timeTable.CreatedDate = DateTime.UtcNow.Date;
                    timeTable.CreatedBy = "Admin";
                    var Newadd = _db.CourseScheduleTbles.Add(timeTable);
                    await _db.SaveChangesAsync();

                    CourseTimeLineTble lit = new CourseTimeLineTble();
                    if (tt.CourseTimeLine.Day != null)
                    {
                        lit.StartingTime = tt.CourseTimeLine.StartingTime;
                        lit.CourseId = timeTable.CourseId;
                        lit.Day = tt.CourseTimeLine.Day;
                        lit.EndingTime = tt.CourseTimeLine.EndingTime;
                        lit.TimeTableId = timeTable.TimeTableId;
                        _db.CourseTimeLineTbles.Add(lit);
                        await _db.SaveChangesAsync();
                        TempData["response"] = "TimeTable Added Successfully !";
                        return RedirectToAction("Detail");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["response"] = "Something wrong " + ex.Message;
                return View();
            }
            return RedirectToAction("Detail");
        }


        [HttpGet]
        public IActionResult CourseTimeTable(int? id)
        {
            var gettimetable = _db.CourseScheduleTbles.Where(x => x.CourseId == id).ToList();
            //ViewBag.timetable = gettimetable;

            var getschedule = _db.CourseTimeLineTbles.Where(x => x.CourseId == id).ToList();
            var i = 0;
            foreach (var item in getschedule)
            {
                
                //var time = item.StartingTime.Value.ToShortTimeString();
                ////item.StartinghTime = time;

                //var time1 = item.EndingTime.Value.ToShortTimeString();
                ////item.EndinghTime = time1;
                item.BreakTime = gettimetable[i].BreakTime;
                i++;
            }

           

            return View(getschedule);
        }



        public async Task<IActionResult> DelTimetable(int? Id)
        {
            if (Id > 0)
            {
                var ondata = await _db.CourseTimeLineTbles.FindAsync(Id);
                if (ondata != null)
                {
                    var schid = ondata.TimeTableId;
                    var ttdata = _db.CourseTimeLineTbles.Where(x => x.TimeTableId == schid).FirstOrDefault();
                    if (ttdata != null)
                    {
                        _db.CourseTimeLineTbles.Remove(ttdata);
                       await _db.SaveChangesAsync();
                    }
                    var coudata = _db.CourseScheduleTbles.Where(x => x.TimeTableId == schid).FirstOrDefault();
                    if (coudata != null)
                    {
                        _db.CourseScheduleTbles.Remove(coudata);
                        await _db.SaveChangesAsync();
                    }
                }
                else
                {
                    TempData["response"] = "Failed!";
                    return RedirectToAction("CourseTimeTable");
                }
            }
            TempData["response"] = "Successfully Deleted !";
            return RedirectToAction("CourseTimeTable");
        }
    }
}

