using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OCMDomain.Repository.Edmx;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OCM.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class EmployeesController : Controller
    {
        private readonly EmailService emailService;

        private readonly OCMContext _db;
        protected UserManager<Users> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IMapper mapper;
        public EmployeesController(OCMContext db, EmailService emailService, UserManager<Users> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _db = db;
            this.emailService = emailService;
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.mapper = mapper;
        }

    
        public IActionResult ApproveStatus(string Status)
        {

            if (Status == null)
            {
                var empapp = _db.EmployTbles.Where(x => x.ApproveStatus == true).ToList();
                return View(empapp);
            }
            return View();
        }
        public IActionResult RejectStatus(string status)
        {
            if (status == null)
            {
                var emprej = _db.EmployTbles.Where(x => x.RejectStatus == true).ToList();
                return View(emprej);
            }
            return View();
        }

        public IActionResult PendingEmploye()
        {
            var data = new EmployeValidation();
            data.EmployList = _db.EmployTbles.ToList().Where(x => x.PendingStatus == true).ToList();
            return View(data);
        }

        public async Task<IActionResult> ApproveEmployee(int id)
        {
            try
            {

                var ApproveEmp = await _db.EmployTbles.FindAsync(id);
                ApproveEmp.ApproveStatus = true;
                ApproveEmp.PendingStatus = false;
                ApproveEmp.RejectStatus = false;
                _db.Entry(ApproveEmp).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await _db.SaveChangesAsync();
                Users obj = new Users();
                obj.Email = ApproveEmp.EmpEmail;
                obj.FirstName = ApproveEmp.FirstName;
                obj.LastName = ApproveEmp.LastName;
                obj.IsVerified = IsVerified.Approved;
                obj.PhoneNumber = ApproveEmp.EmpMobileNum;
                obj.TwoFactorEnabled = false;
                obj.LockoutEnabled = false;
                obj.EmailConfirmed = true;
                obj.Cnic = ApproveEmp.EmpCnic;
                obj.City = "";
                obj.Address = ApproveEmp.EmpAddress;
                obj.AccessFailedCount = 0;
                obj.RoleName = ApproveEmp.EmpType;
                var Id = AddAsync(obj);
                if(Id.ToString() != "0")
                {
                    bool email = emailService.SendApproveMail(ApproveEmp.EmpEmail, "", "", "http://localhost:25149/public/Security/Login");
                    if (email == false)
                    {
                        TempData["response"] = "Employee profile approved successfully, Email Not Send By ISP";
                    }
                    else
                    {
                        TempData["response"] = "Employee profile approved successfully, Email Send Successfully To User";

                    }
                }
                return RedirectToAction("ApproveStatus", "Employees");
            }
            catch (Exception ex)
            {
                TempData["response"] = "Something wrong " + ex.Message;
                return RedirectToAction("ApproveEmployee");
            }
        }
        public async Task<IActionResult> RejectedEmployee(int id)
        {
            try
            {

                var RejectedEmp = await _db.EmployTbles.FindAsync(id);
                RejectedEmp.RejectStatus = true;
                RejectedEmp.ApproveStatus = false;
                RejectedEmp.PendingStatus = false;
                _db.Entry(RejectedEmp).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await _db.SaveChangesAsync();
                bool email = emailService.SendRejectMail(RejectedEmp.EmpEmail, "", "", "");
                if (email == false)
                {
                    TempData["response"] = "Employee Profile Rejected Successfully, Email Not Send By ISP";
                }
                else
                {
                    TempData["response"] = "Employee Profile Rejected Successfully, Email send successfully to user.";
                   
                }
                return RedirectToAction("RejectedEmployee");

            }
            catch (Exception ex)
            {
                TempData["response"] = "Something wrong " + ex.Message;
                return RedirectToAction("RejectedEmployee");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            try
            {
                var detail = await _db.EmployTbles.FindAsync(id);
                if (detail == null)
                {
                    return RedirectToAction("PendingEmploye", "Employees");
                }
                else
                {
                   
                    return View(detail);
                }
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
                model.Password = model.Email;
                    //"Multronica@123";
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
             
                return model.Id;
            }
            catch (Exception ex)
            {
                TempData["response"] = "Something wrong " + ex.Message;
                return "0";
            }
        }


    }
}
