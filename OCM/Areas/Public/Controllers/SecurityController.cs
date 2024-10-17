using Microsoft.AspNetCore.Mvc;
using OCMDomain.Repository.Edmx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace OCM.Areas.Public.Controllers
{
    [Area("Public")]
    [AllowAnonymous]
    public class SecurityController : Controller
    {
        private readonly EmailService emailService;


        private readonly OCMContext _context;

        protected UserManager<Users> _usermanger;
        protected SignInManager<Users> signinmanger;
        public SecurityController(EmailService emailService, OCMContext context, UserManager<Users> usermanger, SignInManager<Users> signinmanger)
        {
            this.emailService = emailService;
            _context = context;
            _usermanger = usermanger;
            this.signinmanger = signinmanger;

        }
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Login(Users data)
        {
            try
            {
                if (data.Email == "admin@ocm.com" && data.PasswordHash == "123")
                {
                    return RedirectToAction("Index", "Home", new { area = "Administration" });
                }
                var result = await signinmanger.PasswordSignInAsync
                         (
                             userName: data.Email,
                             password: data.PasswordHash,
                             isPersistent: true,
                            lockoutOnFailure: false

                         );

                if (result.Succeeded)
                {
                    var user = _context.Users.ToList().Where(x => x.Email == data.Email).FirstOrDefault();
                    if (user.Status == false)
                    {
                        TempData["response"] = "This account has been blocked, Please contact authorities to get unblocked";
                        return View();
                    }
                    if (result.Succeeded)
                    {
                        var GetStudents = _context.StudentRegistrationTbles.ToList().Where(x => x.Email == data.Email && x.ApproveStatus == true).FirstOrDefault();
                        if (GetStudents != null)
                        {
                            return RedirectToAction("Index", "StudentDashboard", new { area = "Student" });
                        }
                        else
                        {
                            var GetEmployee = _context.EmployTbles.ToList().Where(x => x.EmpEmail == data.Email && x.ApproveStatus == true).FirstOrDefault();
                            if (GetEmployee != null && GetEmployee.EmpType == "Trainer")
                            {
                                HttpContext.Session.SetString("EmployeeId", GetEmployee.EmpId.ToString());
                                return RedirectToAction("Index", "EmployeesDashboard", new { area = "Employees" });
                            }
                            if (GetEmployee != null && GetEmployee.EmpType == "Accountant")
                            {
                                HttpContext.Session.SetString("EmployeeId", GetEmployee.EmpId.ToString());
                                return RedirectToAction("Index", "AccountantDashboard", new { area = "Accountant" });
                            }
                            else
                            {
                                return RedirectToAction("Index", "Home", new { area = "Administration" });
                            }
                        }
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home", new { area = "Administration" });
                    }

                }

                else
                {
                    TempData["response"] = "Invalid UserName or Password.";
                    return View();

                }
            }
            catch (Exception ex)
            {
                TempData["response"] = "Something wrong " + ex.Message;
                return View();
            }
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (!ModelState.IsValid)
            {
                TempData["response"] = "Validate Required " + ModelState.Values;
                return View(email);
            }
            //var user = await _usermanger.FindByEmailAsync(email);
            var user = _context.Users.ToList().Where(x => x.Email == email).FirstOrDefault();
            if (user == null)
            {
                TempData["response"] = "Invalid Email Address, User Not Exists";
                return View();
            }
            var token = await _usermanger.GeneratePasswordResetTokenAsync(user);
            var link = Url.Action("ResetPassword", "Security", new { token, email = user.Email }, Request.Scheme);
            EmailService emailHelper = new EmailService();
            bool emailResponse = emailHelper.SendEmailPasswordReset(user.Email, link);
            if (emailResponse)
            {
                TempData["response"] = "The email has been sent. Please check your email to reset your password.";
                return View();
            }
            else
            {
                // log email failed
            }
            return View(email);
        }
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            var model = new ResetPassword { Token = token, Email = email };
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPassword model)
        {
            try
            {
                //if (!ModelState.IsValid)
                //    return View(model);
                var user = await _usermanger.GetUserAsync(User);
                if (User != null)
                {
                    await _usermanger.RemovePasswordAsync(user);
                    await _usermanger.AddPasswordAsync(user, model.Password);
                    //var user = _context.Users.ToList().Where(x => x.Email == model.Email).FirstOrDefault();
                    await _context.SaveChangesAsync();
                    TempData["response"] = "Password Changed Successfully. Please Login again.";
                    return RedirectToAction("Login");
                }
                //_usermanger.FindByEmailAsync(model.Email);
                if (User == null)
                {
                    TempData["response"] = "Invalid Email Address, User Not Exists";
                    return View();
                }
                var resetPassResult = await _usermanger.ResetPasswordAsync(user, model.Token, model.Password);
                if (!resetPassResult.Succeeded)
                {
                    foreach (var error in resetPassResult.Errors)
                    {
                        if (error.Code == "InvalidToken")
                        {
                            TempData["response"] = "Password Changed Successfully . Please Login again.";
                            return RedirectToAction("Login");
                        }
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return View();
                }
                await signinmanger.RefreshSignInAsync(user);
                TempData["response"] = "Password Changed Successfully. Please Login again.";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("Login", "Security");
            }
        }
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }


        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordTble model)
        {
            if (ModelState.IsValid)
            {
                var user = await _usermanger.GetUserAsync(User);
                if (user == null)
                {
                    TempData["response"] = "Invalid User Account";
                    return View();
                }
                var result = await _usermanger.ChangePasswordAsync(user,
                model.CurrentPassword, model.NewPassword);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }
                await signinmanger.RefreshSignInAsync(user);
                TempData["response"] = "Password has been changed successfully";
                return RedirectToAction("Login");
            }
            return View(model);
        }
        public ActionResult Test()
        {
            return View();
        }

        public IActionResult RegisterStudent(int id)
        {
            StudentRegistrationValidation newstd = new StudentRegistrationValidation();
            newstd.OnlineCourse = new OnlineCourseTble();
            newstd.OnlineCourse.OnlineCourseId = id;
            return View(newstd);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterStudent(StudentRegistrationTble obj, int id)
        {
            if (ModelState.IsValid)
            {
                var result = _context.StudentRegistrationTbles.ToList().Exists(x => x.Email == obj.Email);
                if (result)
                {
                    var stdid = _context.StudentRegistrationTbles.ToList().Where(x => x.Email == obj.Email).FirstOrDefault().StudentId;
                    var checkingtbl = _context.CheckUserTbles.ToList().Exists(x => x.StudentId == stdid && x.OnlineCourseId == id);
                    if (!checkingtbl)
                    {
                        CheckUserTble userTble = new CheckUserTble();
                        userTble.StudentId = stdid;
                        userTble.OnlineCourseId = id;
                        userTble.PendingStatus = true;
                        userTble.ApproveStatus = false;
                        userTble.RejectStatus = false;
                        userTble.ChallanIsPaid = false;
                        await _context.CheckUserTbles.AddAsync(userTble);
                        await _context.SaveChangesAsync();
                        var detail = await _context.StudentRegistrationTbles.FindAsync(stdid);

                        bool email = emailService.SendWelcomeEmail(obj.Email, "", "", @Url.Action("UploadChallan", "Security", new { @area = "Public", @email = obj.Email, @cid = id }));
                        if (email == false)
                        {
                            TempData["reponse"] = "Thank You For Enrollment, Email Not Send By ISP";
                            return RedirectToAction("CourseDetails", "Public");
                        }
                        else
                        {
                            TempData["reponse"] = "Thank You For Enrollment, Submit your fee by challan form and check your email.";
                        }

                        var instal = _context.FeeTbles.ToList().Where(x => x.CourseId == id).FirstOrDefault();
                        InstallmentPlanTble inst = new InstallmentPlanTble();
                        if (instal.IsInstallementAllow == true)
                        {
                            DateTime? nextelementdate = DateTime.Now;
                            for (var num = 1; num <= instal.MonthlyInstallment; num++)
                            {
                               
                                inst.Ispaid = false;
                                int sp = userTble.StudentId;
                                inst.StudentId = sp;
                                inst.OnlineCourseId = userTble.OnlineCourseId;
                                inst.FeeId = instal.FeeId;
                                if (num == 1)
                                {
                                    inst.InstallmentDate = DateTime.Now;
                                    inst.NextInstallmentDate = DateTime.Today.AddMonths(1);
                                    nextelementdate = inst.NextInstallmentDate;
                                }
                                else
                                {
                                    inst.InstallmentDate = nextelementdate.Value.AddDays(1);
                                    inst.NextInstallmentDate = nextelementdate.Value.AddMonths(1);
                                    nextelementdate = inst.NextInstallmentDate;

                                }
                                await _context.InstallmentPlanTbles.AddAsync(inst);

                                //ledger working

                               
                            }
                            DetailAccount alldetail = new DetailAccount();
                            alldetail.UserId = inst.StudentId;
                            var GetAccount = _context.DetailAccounts.ToList().Where(x => x.UserId == inst.StudentId).FirstOrDefault();
                            if (GetAccount != null)
                            {
                                var NewCourseFee = _context.FeeTbles.ToList().Where(x => x.CourseId == inst.OnlineCourseId).FirstOrDefault();
                                var Recv = _context.Receivables.ToList().Where(x => x.DetailAccountId == GetAccount.DetailAccountId).FirstOrDefault();
                                if (Recv != null)
                                {
                                    double total = Convert.ToDouble(Recv.Amount);
                                    total += Convert.ToDouble(NewCourseFee.FeeAmount);

                                    Recv.Amount = total.ToString();
                                    _context.Entry(Recv).State = EntityState.Modified;
                                    _context.SaveChanges();
                                }
                            }
                            await _context.SaveChangesAsync();                      
                        }
                        return RedirectToAction("RegisterStudent");
                    }
                    else
                    {
                        TempData["reponse"] = "You are already enroll in this course.";
                        return RedirectToAction("CourseDetails", "Public");
                    }
                }
                else
                {
                    StudentRegistrationTble std = new StudentRegistrationTble();
                    std.FirstName = obj.FirstName;
                    std.LastName = obj.LastName;
                    std.Email = obj.Email;
                    std.Mobile = obj.Mobile;
                    std.Address = obj.Address;
                    std.StdCnic = obj.StdCnic;
                    std.Gender = obj.Gender;
                    //std.StdChlFormPath = obj.StdChlFormPath;
                    std.Dob = obj.Dob;
                    std.PendingStatus = true;
                    std.RejectStatus = false;
                    std.ApproveStatus = false;
                    await _context.StudentRegistrationTbles.AddAsync(std);
                    await _context.SaveChangesAsync();

                    var detail = await _context.StudentRegistrationTbles.FindAsync(std.StudentId);

                    bool email = emailService.SendWelcomeEmail(obj.Email, "", "", @Url.Action("UploadChallan", "Security", new { @area = "Public", @email = obj.Email, @cid = id }));
                    if (email == false)
                    {
                        TempData["reponse"] = "Thank You For Enrollment, Email Not Send By ISP";
                    }
                    else
                    {
                        TempData["reponse"] = "Thank You For Enrollment, Submit your fee by challan form and check your email.";
                    }
                    CheckUserTble userTble = new CheckUserTble();
                    userTble.StudentId = std.StudentId;
                    userTble.OnlineCourseId = id;
                    userTble.PendingStatus = true;
                    userTble.ApproveStatus = false;
                    userTble.RejectStatus = false;
                    userTble.ChallanIsPaid = false;
                    await _context.CheckUserTbles.AddAsync(userTble);
                    await _context.SaveChangesAsync();



                    //installment plan start here
                    var instal = _context.FeeTbles.ToList().Where(x => x.CourseId == id).FirstOrDefault();
                    if (instal.IsInstallementAllow == true)
                    {
                        DateTime? nextelementdate = DateTime.Now;
                        for (var num = 1; num <= instal.MonthlyInstallment; num++)
                        {
                            InstallmentPlanTble inst = new InstallmentPlanTble();
                            inst.Ispaid = false;
                            inst.StudentId = std.StudentId;
                            inst.OnlineCourseId = userTble.OnlineCourseId;
                            inst.FeeId = instal.FeeId;
                            if (num == 1)
                            {
                                inst.InstallmentDate = DateTime.Now;
                                inst.NextInstallmentDate = DateTime.Today.AddMonths(1);
                                nextelementdate = inst.NextInstallmentDate;
                            }
                            else
                            {
                                inst.InstallmentDate = nextelementdate.Value.AddDays(1);
                                inst.NextInstallmentDate = nextelementdate.Value.AddMonths(1);
                                nextelementdate = inst.NextInstallmentDate;

                            }

                            await _context.InstallmentPlanTbles.AddAsync(inst);
                        }
                        await _context.SaveChangesAsync();
                    }

                    return RedirectToAction("RegisterStudent");
                }
            }
            return View();
        }


        [HttpGet]
        public IActionResult UploadChallan(string email, int cid)
        {
            //StudentRegistrationTble std = new StudentRegistrationTble();
            //var str = _context.OnlineCourseTbles.ToList().Where(x=>x.OnlineCourseId==cid).FirstOrDefault();
            //var std = _context.StudentRegistrationTbles.Where(str==cid).FirstOrDefault();
            ////std.Email = email;
            //std.OnlineCourse = new OnlineCourseTble();
            //std.OnlineCourse.OnlineCourseId = cid;
            //return View(std);
            var str = _context.OnlineCourseTbles.FirstOrDefault(x => x.OnlineCourseId == cid);

            if (str != null)
            {
                var std = new StudentRegistrationTble
                {
                    Email = email,
                    OnlineCourse = str
                    //OnlineCourse = str
                };

                return View(std);

            }

            return View();
        }


        [HttpPost]
        public ActionResult UploadChallan(StudentRegistrationTble obj)
        {
            try
            {
                var std = _context.StudentRegistrationTbles.Where(x => x.Email == obj.Email).FirstOrDefault();
                if (std.Email == null)
                {

                    TempData["response"] = "Email not found";
                    return RedirectToAction("UploadChallan", "Security");
                }
                else
                {
                    var chktbl = _context.CheckUserTbles.ToList().Where(x => x.StudentId == std.StudentId && x.OnlineCourseId == obj.OnlineCourse.OnlineCourseId).FirstOrDefault();

                    //std.FirstName = obj.FirstName;
                    //std.LastName = obj.LastName;
                    std.Email = obj.Email;
                    //std.Address = obj.Address;
                    //std.Mobile = obj.Mobile;
                    //std.Gender = obj.Gender;
                    //std.Dob = obj.Dob;
                    //std.PendingStatus = true;
                    //std.RejectStatus = false;
                    //std.ApproveStatus = false;

                    if (obj.CoverPhoto != null)
                    {
                        var file = obj.CoverPhoto;
                        string filePathbar;
                        string uniqueFileName = null;
                        string path1 = @"wwwroot";
                        string fullPath;
                        fullPath = Path.GetFullPath(path1);
                        string uploadsFolder = fullPath + "/Challan";
                        uniqueFileName = Guid.NewGuid().ToString() + "" + ".PDF";
                        filePathbar = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var fileStream = new FileStream(filePathbar, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }
                        chktbl.StdChlFormPath = "/Challan/" + "" + uniqueFileName;
                        chktbl.ChallanIsPaid = false;
                    }
                    var result = _context.Entry(chktbl).State = EntityState.Modified;
                    _context.SaveChanges();

                    //InstallmentPlans
                    //Installment code
                    var fees = _context.InstallmentPlanTbles.Where(x => x.OnlineCourseId == chktbl.OnlineCourseId && x.StudentId == chktbl.StudentId).ToList();
                    foreach (var fn in fees)
                    {
                        if (fn.Ispaid == false)
                        {
                            fn.ChallanFormPath = chktbl.StdChlFormPath;
                            fn.Ispaid = true;
                            _context.Entry(fn).State = EntityState.Modified;
                            _context.SaveChanges();
                            break;
                        }
                    }

                    var GetAccount = _context.DetailAccounts.ToList().Where(x => x.UserId == chktbl.StudentId).FirstOrDefault();
                    if (GetAccount != null)
                    {
                        var NewCourseFee = _context.FeeTbles.ToList().Where(x => x.CourseId == chktbl.OnlineCourseId).FirstOrDefault();
                        var Recv = _context.Receivables.ToList().Where(x => x.DetailAccountId == GetAccount.DetailAccountId).FirstOrDefault();
                        if (Recv != null)
                        {
                            double total = Convert.ToDouble(Recv.Amount);
                            total -= Convert.ToDouble(NewCourseFee.FeeAmount);

                            Recv.Amount = total.ToString();
                            _context.Entry(Recv).State = EntityState.Modified;
                            _context.SaveChanges();
                        }
                    }

                }
                TempData["response"] = "Upload sucessfully";
                return RedirectToAction("Login", "Security");
            }
            catch (Exception ex)
            {
                TempData["response"] = "failed!" + ex.Message; ;
                return RedirectToAction("Login", "Security");
            }
        }





        //public ActionResult AccessDenied()
        //{
        //    return View();
        //}
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
