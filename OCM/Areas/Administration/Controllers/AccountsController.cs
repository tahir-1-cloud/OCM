using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    public class AccountsController : Controller
    {
         private readonly OCMContext _context;
        protected UserManager<Users> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private IWebHostEnvironment env;
        private readonly IMapper mapper;
        public AccountsController(OCMContext context, UserManager<Users> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, IWebHostEnvironment env)
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

        [HttpGet]
        public async Task<IActionResult> Payments()
        { 

            ModelClass model = new ModelClass();
            var penstdfee =  _context.StudentRegistrationTbles.ToList();
            model.ListSrt = penstdfee;

            var penfeestd = _context.CheckUserTbles.ToList();
            model.listCut = penfeestd;

            var listcourse =  _context.OnlineCourseTbles.ToList();
            model.ListOCT = listcourse;
            return View(model);

        }
        [HttpPost]
        public IActionResult CheckFee(int id, string status)
        {
            try
            { 
                var Getcheckid = _context.CheckUserTbles.Find(id);
                if (Getcheckid != null)
                {
                    if (status == "false")
                    {
                        Getcheckid.ChallanIsPaid = false;
                    }
                    else
                    {
                        Getcheckid.ChallanIsPaid = true;
                    }
                    _context.Entry(Getcheckid).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();
                    TempData["response"] = "Fee Approved Successfully";
                    return RedirectToAction("Payments");
                }
                else
                {
                    TempData["response"] = "Cannot Change Status";
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
        public IActionResult InstallmentPlan()
        {
            var getinstallment = _context.InstallmentPlanTbles.ToList();
            return View(getinstallment);
        }

        [HttpGet]
        public IActionResult PaymentHistory()
        {
            var getpayment = _context.DetailAccounts.ToList();
            //return View(getpayment);

            var getdetail = _context.Receivables.ToList();
            ViewBag.model = getdetail;
            return View(getpayment);
        }

        [HttpGet]
        public IActionResult Ledger()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AllPaymentHistory()
        {
            var gethistory = _context.Vouchers.ToList();
            return View(gethistory);
        }
    }
}
