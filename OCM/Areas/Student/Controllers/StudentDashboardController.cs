using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCM.Areas.Student.Controllers
{
    [Area("Student")]
    [Authorize(Roles = "Admin,Student")]
    public class StudentDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Index3()
        {
            return View();
        }
    }
}
