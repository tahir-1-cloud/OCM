using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OCMDomain.Repository.Edmx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCM.Areas.Public.Controllers
   
{
    [Area("Public")]
    public class PublicController : Controller
    {
        private readonly OCMContext _context;
        public PublicController(OCMContext context)
        {
            _context = context;
        }
        public async Task<IActionResult>  Index()
        {
            var allcoures = await _context.OnlineCourseTbles.ToListAsync();
            return View(allcoures);

        }

        public async Task<IActionResult> ContactUs()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]

        public async Task<IActionResult> ContactUs(ContactUsTble NewUser)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.ContactUsTbles.Add(NewUser);
                    await _context.SaveChangesAsync();
                    TempData["Msg"] = "Thanks for Contact Us";
                    return RedirectToAction("ContactUs");
                }
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }
      
         public async Task<IActionResult> Subscription()
        {
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]       
        public async Task<IActionResult> Subscription(SubscriptionTble NewSubscribe)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.SubscriptionTbles.Add(NewSubscribe);
                    await _context.SaveChangesAsync();
                    TempData["Msg"] = "Thanks for Subscribe Us";
                    return RedirectToAction("Subscription");
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Something wrong " + ex.Message;
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> CourseDetails(int? id)
        {
            try
            {
                var detail = await _context.OnlineCourseTbles.FindAsync(id);
                if (detail == null)
                {
                    return RedirectToAction("Index", "Public");
                }
                else
                {
                    var courseFee = _context.FeeTbles.ToList();
                    var model = await _context.CourseQuotaTbles.Where(x => x.CourseId == detail.OnlineCourseId).ToListAsync();
                    var countstudent = 0;
                    foreach (var item in model)
                    {
                        var ut = await _context.CheckUserTbles.Where(x => x.OnlineCourseId == id).ToListAsync();
                        foreach (var std in ut)
                        {
                            StudentRegistrationTble studentRegistrationTble = new StudentRegistrationTble();
                            studentRegistrationTble = await _context.StudentRegistrationTbles.FindAsync(std.StudentId);
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
                    foreach (var item in courseFee.ToList().Where(x => x.CourseId == detail.OnlineCourseId).ToList())
                    {
                        detail.CourseFee = item;
                    }
                    var coursetimelinetable = _context.CourseTimeLineTbles.ToList();
                    var courseschedule = _context.CourseScheduleTbles.ToList();
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
                        var timetable = _context.CourseTimeLineTbles.Where(x => x.TimeTableId == detail.CourseSchedule.TimeTableId).ToList();
                        onlineCourseTble.listCourseTimeLine = timetable;
                    }
                    return View(detail);
                }
            }
            catch (Exception ex)
            {
                TempData["response"] = "Something wrong " + ex.Message;
                return View();
            }
        }











        public async Task<IActionResult> FAQs()
        {
            var AllFaqs = await _context.FaqsTbles.ToListAsync();
            return View(AllFaqs);
            
        }

        //public async Task<IActionResult> TimeTable()
        //{
        //    var Schedle = await _context.CourseScheduleTbles.ToListAsync();
        //    var Schedle1 = await _context.CourseScheduleTbles.ToListAsync();
        //    return View(fill);

        //}
    }
}
