using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OCMDomain.Repository.Edmx;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;

namespace OCM.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Authorize(Roles ="Admin")]
    public class HomeController : Controller
    {
        private readonly OCMContext _context;
        protected UserManager<Users> _usermanager;
        private readonly RoleManager<IdentityRole> roleManager;
        public HomeController(OCMContext context, UserManager<Users> usermanager, RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
            _context = context;
            _usermanager = usermanager;
        }
        public IActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }
        public ActionResult Check()
        {
            return View();
        }
    }
}
