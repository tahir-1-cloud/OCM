using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OCMDomain.Repository.Edmx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace OCM.Areas.Chat.Controllers
{
    [Area("Chat")]
    public class ChattingController : Controller
    {
        private readonly OCMContext _db;
        private UserManager<Users> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public ChattingController(OCMContext db, UserManager<Users> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            this.roleManager = roleManager;

            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult QAForm()
        {
            var users = userManager.Users.Where(x => x.RoleName == "Student").ToList();
            ViewBag.model = users;
            return View();
        }
    }
}
