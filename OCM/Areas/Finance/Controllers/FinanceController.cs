using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCM.Areas.Finance.Controllers
{
    public class FinanceController : Controller
    {
        [Area("Finance")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
