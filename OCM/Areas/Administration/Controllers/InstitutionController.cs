using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OCMDomain.Repository.Edmx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OCM.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class InstitutionController : Controller
    {
        private readonly OCMContext _context;

        protected IHostingEnvironment _environment;

        public InstitutionController(OCMContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public async Task<IActionResult> Index()
        {

            return View();
        }

        [HttpGet]
        public IActionResult OurInstitute()
        {
            var data = _context.InstitutionTbles.FirstOrDefault();
            if(data != null)
            {
                return RedirectToAction("EditInstitute");
            }
            else
            {
                return View(data);
            }
        }
 

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> OurInstitute(InstitutionTble instiution )
        {
            try
            {
                if (ModelState.IsValid)
                {

                    //if (instiution.CoverPhoto != null)
                    //{
                    //    var file = instiution.CoverPhoto;
                    //        string filePathbar;
                    //        string uniqueFileName = null;
                    //        string path1 = @"wwwroot";
                    //        string fullPath;
                    //        fullPath = Path.GetFullPath(path1);
                    //        string uploadsFolder = fullPath + "/Institute/Logo";
                    //        uniqueFileName = Guid.NewGuid().ToString() + "" + ".PNG";
                    //        filePathbar = Path.Combine(uploadsFolder, uniqueFileName);
                    //        using (var fileStream = new FileStream(filePathbar, FileMode.Create))
                    //        {
                    //            file.CopyTo(fileStream);
                    //        }
                        
                    //        instiution.LogoByPath = "~/Institute/Logo" + "" + uniqueFileName;

                    //end

                        //    string folder = "Institute/Logo/";
                        //    folder += Guid.NewGuid().ToString() + "" + intstiution.CoverPhoto.FileName;
                        //    // folder += intstiution.CoverPhoto.FileName + Guid.NewGuid().ToString();

                        //    intstiution.LogoByPath = "/"+folder;
                        //    string serverfolder = Path.Combine(_environment.WebRootPath, folder);

                        //   await intstiution.CoverPhoto.CopyToAsync(new FileStream(serverfolder,FileMode.Create));
                   // }

                    InstitutionTble inst = new InstitutionTble();
                    inst.Name = instiution.Name;
                    inst.RegistrationNo = instiution.RegistrationNo;
                    inst.Phone = instiution.Phone;
                    inst.Address = instiution.Address;
                    inst.Email = instiution.Email;
                    inst.OwnerName = instiution.OwnerName;
                    inst.StartedYear = instiution.StartedYear;
                    inst.NoofCourses = instiution.NoofCourses;
                    inst.NoofStudents = instiution.NoofStudents;
                    inst.LogoByPath = instiution.LogoByPath;
                    inst.Aolevel = instiution.Aolevel;
                    inst.Board = instiution.Board;

                   

                    _context.InstitutionTbles.Add(inst);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("OurInstitute");
                }
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IActionResult EditInstitute()
        {
            var instituation = _context.InstitutionTbles.FirstOrDefault();
            return View(instituation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditInstitute(InstitutionTble updateInstitution)
        {
            //if (id != updateInstitution.InstituteId)
            //{
            //    return NotFound();
            //}

            var institute = _context.InstitutionTbles.FirstOrDefault();

            //if (institute == null)
            //{
            //    return NotFound();
            //}

            if (ModelState.IsValid)
            {
                try
                {
                    institute.Name = updateInstitution.Name;
                    institute.RegistrationNo = updateInstitution.RegistrationNo;
                    institute.Phone = updateInstitution.Phone;
                    institute.Address = updateInstitution.Address;
                    institute.Email = updateInstitution.Email;
                    institute.OwnerName = updateInstitution.OwnerName;
                    institute.StartedYear = updateInstitution.StartedYear;
                    institute.Logo = updateInstitution.Logo;
                    institute.NoofCourses = updateInstitution.NoofCourses;
                    institute.NoofStudents = updateInstitution.NoofStudents;
                    institute.Aolevel = updateInstitution.Aolevel;
                    institute.Board = updateInstitution.Board;
                    institute.IsBoard = updateInstitution.IsBoard;

                    _context.InstitutionTbles.Update(institute);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("OurInstitute", institute);
                }
                catch (Exception ex)
                {
                        throw ex;
                }
            }
            return View();
        }


        //public ActionResult Edit(int id)
        //{

        //    var data = new InstitutaionValidation();
        //    data.ListInstitutionTbles = _context.InstitutionTbles.ToList();
        //    _context.InstitutionTbles.FirstOrDefault(a => a.InstituteId == id);

        //    return View("OurInstitute",data);
        //    //var empdata = new EmployeeDTO()
        //    //{
        //    //    EmployeeList = _db.Employees.ToList(),
        //    //    EmployeeData = _db.Employees.FirstOrDefault(a => a.EmployeeId == id)
        //    //};

        //    //return View("CreateUpdateEmp", empdata);
        //}



    }

}
