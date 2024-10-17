using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OCMDomain.Repository.Edmx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OCM.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class UsersManagementController : Controller
    {
        private readonly EmailService emailService;
        private readonly OCMContext _context;

        protected UserManager<Users> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private IWebHostEnvironment env;
        private readonly IMapper mapper;
        public UsersManagementController(EmailService emailService, OCMContext context, UserManager<Users> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, IWebHostEnvironment env)
        {
            this.roleManager = roleManager;
            _context = context;
            this.userManager = userManager;
            this.mapper = mapper;
            this.env = env;
            this.emailService = emailService;
        }

        public IActionResult AddNewAdmin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewAdmin(Users model)
        {
            try
            {
                Users data = mapper.Map<Users>(model);
                model.RoleName = "Admin";
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

                    bool email = emailService.NewAdmin(model.Email, model.UserName, "http://localhost:25149/public/Security/Login");
                    if (email == false)
                    {
                        TempData["response"] = "User with same email already exists.";
                        return RedirectToAction("AddNewAdmin", "UsersManagement", new { area = "Administration" });
                    }
                    else
                    {
                        TempData["reponse"] = "Admin Account Generated Successfully";
                        return RedirectToAction("AddNewAdmin", "UsersManagement", new { area = "Administration" });
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Something wrong " + ex.Message;
                return RedirectToAction("AddNewAdmin", "UsersManagement", new { area = "Administration" });
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



        [HttpGet]
        public IActionResult UpdateProfile(string? id)
        {
            try
            {
                if (id != null)
                {
                    Users user = userManager.FindByIdAsync(id).Result;
                    if (user == null)
                    {
                        return RedirectToAction("Login", "Home");
                    }
                    else
                    {
                        return View(user);
                    }
                }
                else
                {
                    var user = userManager.GetUserAsync(User).Result;

                    if (user != null)
                    {
                        Users userr = userManager.FindByIdAsync(user.Id).Result;
                        if (userr == null)
                        {
                            return RedirectToAction("Login", "Home");
                        }
                        else
                        {
                            return View(userr);
                        }
                    }
                    else
                    {
                        return RedirectToAction("Login", "Home");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("Login", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(Users userdetails)
        {
            try
            {
                var good = await userManager.FindByIdAsync(userdetails.Id);
                if (userdetails.Email == null)
                {
                    TempData["response"] = "Username not found";
                    return RedirectToAction("UpdateProfile", "UsersManagement");
                }
                else
                {
                    good.FirstName = userdetails.FirstName;
                    good.LastName = userdetails.LastName;
                    good.Email = userdetails.Email;
                    good.PhoneNumber = userdetails.PhoneNumber;
                    good.Address = userdetails.Address;
                    good.Cnic = userdetails.Cnic;
                }
                var result = await userManager.UpdateAsync(good);
                await _context.SaveChangesAsync();
                TempData["response"] = "Updated Successfully";
                return RedirectToAction("ExistingUser");
            }
            catch (Exception ex)
            {
                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("UpdateProfile", "UsersManagement");
            }
        }

        public IActionResult AddRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddRole(AspNetRole model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    IdentityRole identityRole = new IdentityRole
                    {
                        Name = model.Name
                    };
                    IdentityResult result = await roleManager.CreateAsync(identityRole);
                    if (result.Succeeded)
                    {
                        TempData["response"] = "Role Added Successfully !";
                        return RedirectToAction("ListRole", "Home");
                    }
                    foreach (IdentityError error in result.Errors)
                    {
                        TempData["response"] = "Role Adding Failed !";
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public IActionResult BlockedUserList()
        {
            var users = userManager.Users.ToList();
            var CurrentUser = User.Identity.Name;
            if (CurrentUser != null)
            {
                var CurrentUserID = _context.Users.ToList().Where(x => x.Email == CurrentUser).FirstOrDefault();
                if (CurrentUserID != null)
                {
                    ViewBag.CurrentUserID = CurrentUserID.Id;
                }
                else
                {
                    return View(users);
                }
            }
            else
            {
                TempData["response"] = "Session Expire";
                return RedirectToAction("Login", "Security", new { area = "Public" });
            }
            return View(users);
            //var users = userManager.Users.ToList();
            //return View(users);
        }

        [HttpPost]
        public IActionResult BlockUser(string id, string status)
        {
            try
            {
                var GetUser = _context.Users.Find(id);
                if (GetUser != null)
                {
                    if (status == "true")
                    {
                        GetUser.Status = false;
                    }
                    else
                    {
                        GetUser.Status = true;
                    }
                    _context.Entry(GetUser).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();
                    TempData["response"] = "User Access Status Changed.";
                    return RedirectToAction("BlockedUserList", "UsersManagement");
                }
                else
                {
                    TempData["response"] = "User Id Is Invalid.";
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Something wrong " + ex.Message;
                return View();
            }
        }

        //public IActionResult BlockedUserList()
        //{
        //    var users = userManager.Users.ToList();
        //    return View(users);
        //}

        //public IActionResult BlockUser(int id,bool status)
        //{
        //    var current = User.Identity.Name;
        //    var user = _context.AspNetUser.ToList().Where(x => x.Id == current).FirstOrDefault();
        //    //var user = _context.Users.Where(x => x.Email == model.Email).FirstOrDefault();
        //    if (user != null)
        //    {
        //        user.Status = true;
        //        _context.SaveChanges();
        //    }
        //    else
        //    {
        //        user.Status = false;
        //        return RedirectToAction("Login","Security");
        //    }

        //        return View();                 
        //}


        //public IActionResult UnBlockUser()
        //{
        //    var current = User.Identity.Name;
        //    var user = _context.AspNetUser.ToList().Where(x => x.Id == current).FirstOrDefault();
        //    if (user != null)
        //    {
        //        user.Status = false;

        //        //_context.Entry(user).State = System.Data.Modified;

        //       _context.SaveChanges();              
        //        return View();
        //    }
        //    return View();
        //}

        public IActionResult ExistingUser()
        {
            var users = userManager.Users.ToList();
            var CurrentUser = User.Identity.Name;
            if (CurrentUser != null)
            {
                var CurrentUserID = _context.Users.ToList().Where(x => x.Email == CurrentUser).FirstOrDefault();
                if (CurrentUserID != null)
                {
                    ViewBag.CurrentUserID = CurrentUserID.Id;
                }
                else
                {
                    return View(users);
                }
            }
            else
            {
                TempData["response"] = "Session Expire";
                return RedirectToAction("Login", "Security", new { area = "Public" });
            }
            return View(users);
        }


        [HttpGet]
        public async Task<IActionResult> DeleteUser(String id)
        {
            try
            {
                var user = await userManager.FindByIdAsync(id);
                if (user == null)
                {
                    TempData["response"] = "User Delete Failed !";
                    return RedirectToAction("ExistingUser", "UsersManagement");
                }
                else
                {
                    var result = await userManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        TempData["response"] = "User Deleted Successfully !";
                          return RedirectToAction("ExistingUser", "UsersManagement");
                    }
                    foreach (var error in result.Errors)
                    {
                        TempData["response"] = "User Delete Failed !";
                    }
                    return RedirectToAction("ExistingUser", "UsersManagement");
                }
            }
            catch (Exception ex)
            {
                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("ExistingUser", "UsersManagement");
            }
        }




        public IActionResult ListRole()
        {
            // var RoleList = _context.AspNetRoles.ToList();
            var role = roleManager.Roles.ToList();
            return View(role);
        }
        [HttpGet]
        public async Task<IActionResult> UpdateRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                TempData["response"] = "Role Not Found !";
                return RedirectToAction("ListRole", "Administration");
            }
            IdentityRole identityRole = new IdentityRole
            {
                Name = role.Name
            };
            //AspNetRole obj = new AspNetRole();
            //obj.Name = role.Name;
            //obj.Id = role.Id;
            return View(identityRole);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateRole(IdentityRole model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);
            if (role == null)
            {
                TempData["response"] = "Role Update Failed !";
                return RedirectToAction("ListRole", "Home");
            }
            else
            {
                role.Name = model.Name;
                var result = await roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    TempData["response"] = "Role Updated Successfully !";
                    return RedirectToAction("ListRole", "Home");
                }
                foreach (var error in result.Errors)
                {
                    TempData["response"] = "Failed !";
                }
                return View();


            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteRole(String id)
        {
            try
            {
                var role = await roleManager.FindByIdAsync(id);
                if (role == null)
                {
                    TempData["response"] = "Role Delete Failed !";
                    return RedirectToAction("ListRoles", "Home");
                }
                else
                {
                    var result = await roleManager.DeleteAsync(role);
                    if (result.Succeeded)
                    {
                        TempData["response"] = "Role Deleted Successfully !";
                        return RedirectToAction("ListRole", "Home");
                    }
                    foreach (var error in result.Errors)
                    {
                        TempData["response"] = "Role Delete Failed !";
                    }
                    return RedirectToAction("ListRole", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("ListRole", "Home");
            }
        }


    }
}
