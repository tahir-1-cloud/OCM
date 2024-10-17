using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OCMDomain.Entities;
using OCMDomain.Repository.Edmx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OCM.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class TeachersController : Controller
    {
        private readonly OCMContext _context;

        protected UserManager<Users> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private IWebHostEnvironment env;
        private readonly IMapper mapper;
        public TeachersController(OCMContext context, UserManager<Users> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, IWebHostEnvironment env)
        {
            this.roleManager = roleManager;
            _context = context;
            this.userManager = userManager;
            this.mapper = mapper;
            this.env = env;

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddNewTeacher()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewTeacher(Users model)
        {
            try
            {
                Users data = mapper.Map<Users>(model);
                model.RoleName = "Teacher";
                var UserExist = _context.Users.ToList().Where(x => x.Email == data.Email).ToList().FirstOrDefault();
                if (UserExist == null)
                {
                    if (model.profile_img != null && model.profile_img.Length > 0)
                    {
                        var imgPath = SaveImage(model.profile_img, Guid.NewGuid().ToString());
                        model.profile_img = null;
                        model.ImagePath = imgPath;
                    }
                    model.Id = await AddAsync(model);
                    TempData["response"] = "Teacher Registered Successully.";
                    return RedirectToAction("ExistingUser", "UsersManagement", new { area = "Administration" });

                }
                else
                {
                    TempData["response"] = "User with same email already exists.";
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Something wrong " + ex.Message;
                return View();
            }
        }

        public async Task<string> AddAsync(Users model)
        {
            try
            {
                model.UserName = model.Email;
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
                await _context.SaveChangesAsync();
                bool adminRoleExists = await roleManager.RoleExistsAsync(model.RoleName);
                if (!adminRoleExists)
                {
                    IdentityRole identityRole = new IdentityRole
                    {
                        Name = model.RoleName
                    };
                    await roleManager.CreateAsync(identityRole);
                    await _context.SaveChangesAsync();

                }
                await userManager.AddToRoleAsync(model, model.RoleName);
                await _context.SaveChangesAsync();
                return model.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected string SaveImage(byte[] str, string ImgName)
        {
            string hostRootPath = env.WebRootPath;
            string webRootPath = env.ContentRootPath;
            string imgPath = string.Empty;

            if (!string.IsNullOrEmpty(webRootPath))
            {
                string path = webRootPath + "\\images\\";
                string imageName = ImgName + ".jpg";
                imgPath = Path.Combine(path, imageName);
                byte[] bytes = str;
                System.IO.File.WriteAllBytes(imgPath, bytes);
                imgPath = $"Images/{imageName}";
            }
            else if (!string.IsNullOrEmpty(hostRootPath))
            {
                string path = hostRootPath + "\\images\\";
                string imageName = ImgName + ".jpg";
                imgPath = Path.Combine(path, imageName);
                byte[] bytes = str;
                System.IO.File.WriteAllBytes(imgPath, bytes);
                imgPath = $"Images/{imageName}"; ;
            }
            return imgPath;
        }


        public async Task<IActionResult> MyCourses()
        {
            var courses = new List<OnlineCourseTble>();
            var empid = Int32.Parse(HttpContext.Session.GetString("EmployeeId"));
            var onlinecourselist = new List<OnlineCourseTble>();
            var assignteachertable = _context.AssignTeacherTbles.Where(x => x.EmpId == empid).ToList();
            if (assignteachertable != null)
            {
                for (int j = 0; j < assignteachertable.Count; j++)
                {
                    var onlinecourse = await _context.OnlineCourseTbles.Where(x => x.OnlineCourseId == assignteachertable[j].CourseId).FirstOrDefaultAsync();
                    if (onlinecourse != null)
                    {
                        onlinecourselist.Add(onlinecourse);
                    }
                }
            }
            var courseQuota = _context.CourseQuotaTbles.ToList();
            for (int i = 0; i < onlinecourselist.Count(); i++)
            {
                foreach (var item in courseQuota.ToList().Where(x => x.CourseId == onlinecourselist[i].OnlineCourseId).ToList())
                {
                    onlinecourselist[i].CourseQuota = item;
                }
            }
            return View(onlinecourselist);
        }


        public async Task<IActionResult> MyCourseDescription(int id)
        {
            ModelClass model = new ModelClass();
            List<CheckUserTble> list = new List<CheckUserTble>();
            var stdlist = _context.StudentRegistrationTbles.ToList();
            var chkusertable = _context.CheckUserTbles.Where(x => x.OnlineCourseId == id).Where(y => y.ApproveStatus == true).ToList();

            var batch = _context.CourseQuotaTbles.Where(x => x.CourseId == id).FirstOrDefault();

            List<StudentRegistrationTble> stdreg = new List<StudentRegistrationTble>();
            foreach (var item in chkusertable)
            {
                var studentid = item.StudentId;
                var studentobj = stdlist.Where(x => x.StudentId == studentid).FirstOrDefault();

                if (studentobj != null)
                {
                    stdreg.Add(studentobj);
                }


            }
            model.ListSrt = stdreg;
            model.OCT = await _context.OnlineCourseTbles.FindAsync(id);
            model.ListOCT = await _context.OnlineCourseTbles.ToListAsync();
            model.CourseQt = batch;
            //model.Cut = await _context.CheckUserTbles.FindAsync(id);
            //model.listCut = await _context.CheckUserTbles.ToListAsync();

       
            

            var courseQuota = _context.CourseQuotaTbles.ToList();
            //var result = _context.CheckUserTbles.Where(x => x.OnlineCourseId == id).ToListAsync();
            for (int i = 0; i < model.ListOCT.Count(); i++)
            {
                foreach (var item in courseQuota.ToList().Where(x => x.CourseId == model.ListOCT[i].OnlineCourseId).ToList())
                {
                    model.ListOCT[i].CourseQuota = item;
                }

            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> LectureMaterial(int id, CourseMaterialTble course)
        {
            ModelClass model = new ModelClass();
            model.OCT = await _context.OnlineCourseTbles.FindAsync(id);
            model.ListOCT = await _context.OnlineCourseTbles.ToListAsync();
            model.ListcourseMaterials = await _context.CourseMaterialTbles.Where(x => x.OnlineCourseId == id).ToListAsync();
            //model.courseMaterials = _context.CourseMaterialTbles.FirstOrDefault();

            var Allbatches = await _context.BatchTbles.ToListAsync();
            ViewBag.Batch = new SelectList(Allbatches.ToList(), "BatchId", "BtachName");

            var courseQuota = _context.CourseQuotaTbles.ToList();
            for (int i = 0; i < model.ListOCT.Count(); i++)
            {
                foreach (var item in courseQuota.ToList().Where(x => x.CourseId == model.ListOCT[i].OnlineCourseId).ToList())
                {
                    model.ListOCT[i].CourseQuota = item;
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> LectureMaterial(CourseMaterialTble Coursedata)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Coursedata.Video != null)
                    {
                        var file = Coursedata.Video;
                        string filePathbar;
                        string uniqueFileName = null;
                        string path1 = @"wwwroot";
                        string fullPath;
                        fullPath = Path.GetFullPath(path1);
                        string uploadsFolder = fullPath + "/Material/Video";
                        uniqueFileName = Guid.NewGuid().ToString() + "" + ".mp4";
                        filePathbar = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var fileStream = new FileStream(filePathbar, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }
                        Coursedata.VideoPath = "/Material/Video/" + "" + uniqueFileName;
                        //Coursedata.FileType = "MP4";
                        Coursedata.UploadDate = DateTime.Now;
                        Coursedata.FileApprove = false;
                        string name = Path.GetFileName(Coursedata.Video.FileName).Trim();
                        Coursedata.Filename = name;
                        string ext = Path.GetExtension(Coursedata.Video.FileName).Trim('.').ToUpper();
                        Coursedata.FileType = ext;
                        Coursedata.VideoTitle = Coursedata.VideoTitle;
                        Coursedata.VideoDescription = Coursedata.VideoDescription;
                    }
                    if (Coursedata.CoverPhoto != null)
                    {
                        var file = Coursedata.CoverPhoto;
                        string filePathbar;
                        string uniqueFileName = null;
                        string path1 = @"wwwroot";
                        string fullPath;
                        fullPath = Path.GetFullPath(path1);
                        string uploadsFolder = fullPath + "/Material/data";
                        uniqueFileName = Guid.NewGuid().ToString() + "" + ".JPG";
                        filePathbar = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var fileStream = new FileStream(filePathbar, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }
                        //Coursedata.FileType = "JPEG/PNG";
                        Coursedata.ImagePath = "/Material/data/" + "" + uniqueFileName;
                        Coursedata.UploadDate = DateTime.Now;
                        Coursedata.FileApprove = false;
                        string name = Path.GetFileName(Coursedata.CoverPhoto.FileName).Trim();
                        Coursedata.Filename = name;
                        string ext = Path.GetExtension(Coursedata.CoverPhoto.FileName).Trim('.').ToUpper();
                        Coursedata.FileType = ext;
                    }
                    if (Coursedata.CoverPhoto1 != null)
                    {
                        var file = Coursedata.CoverPhoto1;
                        string filePathbar;
                        string uniqueFileName = null;
                        string path1 = @"wwwroot";
                        string fullPath;
                        fullPath = Path.GetFullPath(path1);
                        string uploadsFolder = fullPath + "/Material/File";
                        uniqueFileName = Guid.NewGuid().ToString() + "" + ".PDF";
                        filePathbar = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var fileStream = new FileStream(filePathbar, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }
                        Coursedata.FilePath = "/Material/File/" + "" + uniqueFileName;
                        //Coursedata.FileType = "PDF";
                        Coursedata.UploadDate = DateTime.Now;
                        Coursedata.FileApprove = false;
                        string name = Path.GetFileName(Coursedata.CoverPhoto1.FileName).Trim();
                        Coursedata.Filename = name;
                        var ext = Path.GetExtension(Coursedata.CoverPhoto1.FileName).TrimStart('.').ToUpper();
                        Coursedata.FileType = ext.ToString();
                    }
                    //Coursedata.Filename = Coursedata?.Video?.FileName + "  " + Coursedata?.CoverPhoto?.FileName + "  " + Coursedata?.CoverPhoto1?.FileName;
                    //Coursedata.FileType = Coursedata?.Video?.ContentType + "  " + Coursedata?.CoverPhoto?.ContentType + "  " + Coursedata?.CoverPhoto1?.ContentType;
                    Coursedata.OnlineCourseId = Coursedata.OnlineCourseId;
                    _context.CourseMaterialTbles.Add(Coursedata);
                    await _context.SaveChangesAsync();
                    TempData["response"] = "Course Material Uploaded Successfully";
                    return RedirectToAction("LectureMaterial");
                }
            }
            catch (Exception ex)
            {
                TempData["response"] = "Something wrong " + ex.Message;
            }
            return View();
        }


        [HttpPost]
        public IActionResult TransferMaterial(int batchid, int Courseid)
        {
            if (Courseid == 0)
            {
                return null;
            }
            var lecmaterial = _context.CourseMaterialTbles.Where(x => x.OnlineCourseId == Courseid).ToList();
            var course_quota = _context.CourseQuotaTbles.Where(x => x.BatchId == batchid).FirstOrDefault();
            
            if (lecmaterial != null)
            {
                for (int i = 0; i < lecmaterial.Count(); i++)
                {
                    var data = _context.OnlineCourseTbles.ToList().Where(x => x.OnlineCourseId == lecmaterial[i].OnlineCourseId).FirstOrDefault();
                    var coursecode = data.Code;
                    var dataa = _context.OnlineCourseTbles.ToList().Where(x => x.Code == coursecode).FirstOrDefault();
                    CourseMaterialTble obj = new CourseMaterialTble();

                   
                    obj.OnlineCourseId = course_quota.CourseId;
                    obj.Filename = lecmaterial[i].Filename;
                    obj.FileType = lecmaterial[i].FileType;
                    obj.UploadDate = lecmaterial[i].UploadDate;
                    obj.FilePath = lecmaterial[i].FilePath;
                    obj.VideoPath = lecmaterial[i].VideoPath;
                    obj.ImagePath = lecmaterial[i].ImagePath;
                    obj.VideoTitle = lecmaterial[i].VideoTitle;
                    obj.VideoDescription = lecmaterial[i].VideoDescription;
                    obj.FileApprove = false;

                   

                    _context.CourseMaterialTbles.Add(obj);
                   
                }
                _context.SaveChanges();
            }
            return View();
        }






        [HttpGet]
        public IActionResult CourseOutline(int id)
        {
            ModelClass model = new ModelClass();
            model.CourseQt = _context.CourseQuotaTbles.Where(x => x.CourseId == id).FirstOrDefault();
            model.AssignTT = _context.AssignTeacherTbles.Where(x => x.CourseId == id).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CourseOutline(int id, ModelClass model)
        {
            if (model != null)
            {
                CourseOutlineTble COT = new CourseOutlineTble();
                COT.OnlineCourseId = model.CourseQt.CourseId;
                COT.BatchId = model.CourseQt.BatchId;
                COT.EmpId = model.AssignTT.EmpId;

                COT.CourseTitle = model.CourseOT.CourseTitle;
                COT.CourseDescription = model.CourseOT.CourseDescription;
                _context.CourseOutlineTbles.Add(COT);
                await _context.SaveChangesAsync();
                TempData["response"] = "Course Outline Added Successfully !";
                return RedirectToAction("CourseOutline");
            }
            else
            {
                return View();
            }
        }

        public IActionResult CheckCourseOutline(int id)
        {

            var getcot = _context.CourseOutlineTbles.Where(x => x.OnlineCourseId == id).FirstOrDefault();
            if (getcot != null)
            {
                if (getcot.CourseTitle != null)
                {
                    return View(getcot);
                }

            }
            return View();
        }


        [HttpPost]
        public IActionResult PublishMaterial(int id, string status)
        {
            try
            {
                var Getlecid = _context.CourseMaterialTbles.Find(id);
                if (Getlecid != null)
                {
                    if (status == "false")
                    {
                        Getlecid.FileApprove = true;
                    }
                    else
                    {
                        Getlecid.FileApprove = false;
                    }
                    _context.Entry(Getlecid).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();
                    TempData["response"] = "Course Published Successfully.";
                    return View();
                }
                else
                {
                    TempData["response"] = "Course Not Found.";
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
        public async Task<IActionResult> DeleteConfirmed(int? id, int? onlinecourseid)
        {
            try
            {
                var delecourese = await _context.CourseMaterialTbles.FindAsync(id);
                if (delecourese == null)
                {
                    TempData["response"] = "Invalid Course Id";
                    return RedirectToAction("LectureMaterial");
                }
                else
                {
                    var delresult = _context.CourseMaterialTbles.Remove(delecourese);
                    await _context.SaveChangesAsync();
                    TempData["response"] = "Lecture Deleted Successfully";
                    return RedirectToAction("LectureMaterial",new { id=onlinecourseid});
                    //return RedirectToAction("/LectureMaterial/" + id);
                }
            }
            catch (Exception ex)
            {
                TempData["response"] = "Something wrong " + ex.Message;
                return RedirectToAction("LectureMaterial", new { id = onlinecourseid });
            }
        }
    }
}
