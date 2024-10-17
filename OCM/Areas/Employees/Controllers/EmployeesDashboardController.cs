using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCM.Areas.Employees.Controllers
{
    [Area("Employees")]
    [Authorize(Roles = "Admin,Employee")]
    public class EmployeesDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
