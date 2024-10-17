using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCM.Areas.Accountant.Controllers
{
   [Area("Accountant")]
   [Authorize(Roles = "Admin, Accountant")]
    public class AccountantDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

