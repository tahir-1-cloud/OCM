using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OCMDomain.Repository.Edmx;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using System.Globalization;

namespace OCM.Areas.HumanResource.Controllers
{
    [Area("HumanResource")]
    public class EmployeeController : Controller
    {
        private readonly OCMContext _db;
        public EmployeeController(OCMContext db)
        {
            _db = db;
        }

        //Employe List
        public IActionResult EmployeeList()
        {
            var data = new EmployeValidation();
            data.EmployList = _db.EmployTbles.AsQueryable().ToList().Where(x => x.PendingStatus == true & x.ApproveStatus == false & x.RejectStatus == false).ToList();
            return View(data);

        }
        public IActionResult AddEmployee()
        {
            return View();
        }

        //Post Employee
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> AddEmployee(EmployTble employee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (employee.CoverPhoto != null)
                    {
                        var file = employee.CoverPhoto;
                        string filePathbar;
                        string uniqueFileName = null;
                        string path1 = @"wwwroot";
                        string fullPath;

                        fullPath = Path.GetFullPath(path1);
                        string uploadsFolder = fullPath + "/Employe/degree";
                        uniqueFileName = Guid.NewGuid().ToString() + "" + ".PDF";
                        filePathbar = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePathbar, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        employee.EmpExperLetterPath = "/Employe/degree/" + "" + uniqueFileName;

                    }

                    if (employee.CoverPhoto1 != null)
                    {
                        var file = employee.CoverPhoto1;
                        string filePathbar;
                        string uniqueFileName = null;
                        string path1 = @"wwwroot";
                        string fullPath;
                        fullPath = Path.GetFullPath(path1);
                        string uploadsFolder = fullPath + "/Images";
                        uniqueFileName = Guid.NewGuid().ToString() + "" + ".PDF";
                        filePathbar = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var fileStream = new FileStream(filePathbar, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }
                      
                        employee.EmpDegrePath = "/Images/" + "" + uniqueFileName;

                    }

                    int count = _db.EmployTbles.ToList().Where(x => x.EmpEmail == employee.EmpEmail).Count();
                    int countUser = _db.Users.ToList().Where(x => x.Email == employee.EmpEmail).Count();
                    if (count != 0 && countUser != 0)
                    {
                        TempData["response"] = "User with same email already exists.";
                        return View(employee);
                    }
                    else
                    {
                        EmployTble emp = new EmployTble();
                        emp.EmpType = employee.EmpType;
                        emp.FirstName = employee.FirstName;
                        emp.MiddleName = employee.MiddleName;
                        emp.LastName = employee.LastName;
                        emp.EmpAddress = employee.EmpAddress;
                        emp.EmpCnic = employee.EmpCnic;
                        emp.EmpEmail = employee.EmpEmail;
                        emp.EmpPhoneNum = employee.EmpPhoneNum;
                        emp.EmpMobileNum = employee.EmpMobileNum;
                        emp.Gender = employee.Gender;
                        emp.EmpMaritalStatus = employee.EmpMaritalStatus;
                        emp.EmpDob = employee.EmpDob;
                        emp.DrivingLicence = employee.DrivingLicence;
                        emp.EmpQualification = employee.EmpQualification;
                        emp.EmpSpecializedDegree = employee.EmpSpecializedDegree;
                        emp.EmpDegreeYear = employee.EmpDegreeYear;
                        emp.EmpExperience = employee.EmpExperience;
                        emp.RefName = employee.RefName;
                        emp.RefMobileNum = employee.RefMobileNum;
                        emp.RefEmail = employee.RefEmail;
                        emp.RefAddress = employee.RefAddress;
                        emp.EmpExperLetterPath = employee.EmpExperLetterPath;
                        emp.EmpDegrePath = employee.EmpDegrePath;
                        emp.PendingStatus = true;
                        emp.ApproveStatus = false;
                        emp.RejectStatus = false;
                        //Add Employe on Db
                        _db.EmployTbles.Add(emp);
                        await _db.SaveChangesAsync();
                        TempData["response"] = "Employee Registered Successfully, Forwarded for approval";
                        return RedirectToAction("EmployeeList", "Employee");
                    }

                }
                else
                {
                    TempData["response"] = "Validate Required " + ModelState.Values;
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["response"] = "Something wrong " + ex.Message;
                return View();
            }
        }


        [HttpGet]
        public async Task<IActionResult> UpdateEmploye(int? id)
        {

            if (id == null)
            {
                return RedirectToAction("EmployeeList", "HumanResource");
            }
            var emp = await _db.EmployTbles.FindAsync(id);

            if (emp == null)
            {
                return NotFound();
            }
            return View(emp);
        }
        //Update Employe 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateEmploye(EmployTble employ, int id)
        {
            if (id != employ.EmpId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (employ.CoverPhoto != null)
                    {

                        var file = employ.CoverPhoto;
                        string filePathbar;
                        string uniqueFileName = null;
                        string path1 = @"wwwroot";

                        string fullPath;

                        fullPath = Path.GetFullPath(path1);
                        string uploadsFolder = fullPath + "/Employe/degree";
                        uniqueFileName = Guid.NewGuid().ToString() + "" + ".PDF";
                        filePathbar = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePathbar, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        employ.EmpExperLetterPath = "/Employe/degree/" + "" + uniqueFileName;


                    }

                    if (employ.CoverPhoto1 != null)
                    {
                        var file = employ.CoverPhoto1;
                        string filePathbar;
                        string uniqueFileName = null;
                        string path1 = @"wwwroot";
                        string fullPath;
                        fullPath = Path.GetFullPath(path1);
                        string uploadsFolder = fullPath + "/Images/";
                        uniqueFileName = Guid.NewGuid().ToString() + "" + ".PDF";
                        filePathbar = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var fileStream = new FileStream(filePathbar, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        // employee.EmpExperLetterPath = "/Employe/degree/" + "" + uniqueFileName;

                        employ.EmpDegrePath = "/Images/" + "" + uniqueFileName;
                    }
                    if (employ.ApproveStatus == null && employ.RejectStatus == null && employ.PendingStatus ==true)
                    {
                        TempData["response"] = "Already Approved";
                        return RedirectToAction("UpdateEmploye");
                    }

                    else 
                    {
                        employ.PendingStatus = true;
                        employ.RejectStatus = false;
                        employ.ApproveStatus = false;
                        _db.EmployTbles.Update(employ);
                        await _db.SaveChangesAsync();
                        TempData["response"] = "Updated Successfully";
                        return RedirectToAction("EmployeeList", "Employee");
                    }                                      
                }
                catch (Exception ex)
                {
                    TempData["response"] = "Something wrong " + ex.Message;
                    return View();
                }
            }
            else
            {
                TempData["response"] = "Validate Required " + ModelState.Values;
                return View();
            }
        }


        //Delete Employe
        [HttpGet]
        public async Task<IActionResult> DeleteEmploye(int? id)
        {

            try
            {
                var delemp = await _db.EmployTbles.FindAsync(id);
                if (delemp == null)
                {
                    TempData["response"] = "Invalid Employee Profile";
                    return RedirectToAction("EmployeeList", "Employee");
                }
                else
                {
                    var delresult = _db.EmployTbles.Remove(delemp);
                    TempData["response"] = "Employee  Deleted Successfully";
                    return RedirectToAction("EmployeeList", "Employee");
                }

            }
            catch (Exception ex)
            {
                TempData["response"] = "Something wrong " + ex.Message;
                return View();
            }
        }

        //Contract Employe List
        [HttpGet]
        public IActionResult EmpContractList()
        {
            var data = new EmployeContarctValidation();
            data.EmpContractList = _db.EmployContractTbles.ToList();
            return View(data);
        }

        public async Task<IActionResult> EmployeeContract()
            {
            try
            {
                var allemployees = await _db.EmployTbles.ToListAsync();
                for (int i = 0; i < allemployees.Count(); i++)
                {
                    allemployees[i].FullName = allemployees[i].FirstName + " " + allemployees[i].MiddleName + " " + allemployees[i].LastName;
                }
                ViewBag.Employe = new SelectList(allemployees.Where(x=> x.ApproveStatus==true && x.PendingStatus==false && x.RejectStatus == false).ToList(), "EmpId", "FullName");
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Something wrong " + ex.Message;
                return View();
            }
        }
        //Add Data in Db
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> EmployeeContract(EmployContractTble employContract)

        {
            try
            {
                if (ModelState.IsValid)
                {

                    EmployContractTble employ = new EmployContractTble();                   
                    employ.FullName = employContract.FullName;
                    employ.Email = employContract.Email;
                    employ.Cnic = employContract.Cnic;
                    employ.Address = employContract.Address;
                    employ.Mobile = employContract.Mobile;
                    employ.ContractType = employContract.ContractType;
                    employ.IsProbation = employContract.IsProbation;
                    employ.ProbationEndDate = employContract.ProbationEndDate;
                    //string hhh = employ.ProbationStartDate.Value.ToShortDateString();
                    //employ.ProbationStartDate = DateTime.ParseExact(hhh, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    employ.ProbationStartDate = employContract.ProbationStartDate;
                    employ.ProbationPeriod = employContract.ProbationPeriod;              
                    employ.JoiningDate = employContract.JoiningDate;
                    employ.ContractExpireDate = employContract.ContractExpireDate;
                    employ.SalaryType = employContract.SalaryType;
                    employ.SalaryAmount = employContract.SalaryAmount;

                    //employ.ProbationStartDate = employContract.ProbationStartDate;
                    _db.EmployContractTbles.Add(employ);
                    await _db.SaveChangesAsync();
                    TempData["response"] = "Contract Employee  Added Successfully";
                    return RedirectToAction("EmpContractList");
                }
                else
                {
                    TempData["response"] = "Validate Required " + ModelState.Values;
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["response"] = "Something wrong " + ex.Message;
                return View();
            }
        }

        //Update Contract Employe 
        [HttpGet]
        public async Task<IActionResult> UpdateContractEmploye(int? id)
        {

            if (id == null)
            {
                return RedirectToAction("EmpContractList", "HumanResource");
            }
            var emp = await _db.EmployContractTbles.FindAsync(id);
        
            if (emp == null )
            {
                return NotFound();
            }
             
            return View(emp);
        }
        //Update Contract Employe 
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> UpdateContractEmploye(EmployContractTble employContract, int id)
        {
            if (id != employContract.ConId)
            {
                TempData["response"] = "Invalid Record to update.";
                return View();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _db.EmployContractTbles.Update(employContract);
                    await _db.SaveChangesAsync();
                    TempData["response"] = "Employee Contract  Updated Successfully";
                    return RedirectToAction("EmpContractList", "Employee");
                }
                catch (Exception ex)
                {
                    TempData["response"] = "Something wrong " + ex.Message;
                    return View();
                }
            }
            else
            {
                TempData["response"] = "Validate Required " + ModelState.Values;
                return View();
            }
        }

        //Delete Contract Employe
        [HttpGet]
        public async Task<IActionResult> DeleteContractEmploye(int? id)
        {

            try
            {
                var delemp = await _db.EmployContractTbles.FindAsync(id);
                if (delemp == null)
                {
                    return RedirectToAction("EmpContractList", "Employee");
                }
                else
                {
                    var delresult = _db.EmployContractTbles.Remove(delemp);
                    await _db.SaveChangesAsync();
                    TempData["response"] = "Employee Contract  Deleted Successfully";
                    return RedirectToAction("EmpContractList", "Employee");
                }

            }
            catch (Exception ex)
            {
                TempData["response"] = "Something wrong " + ex.Message;
                return View();
            }
        }

        public IActionResult GetEmployeeByID(int id)
        {
            var msg = "";
            if (id > 0)
            {
                var data = _db.EmployTbles.Find(id);
                if (data != null)
                {
                    data.FullName = data.FirstName + " " + data.MiddleName + " " + data.LastName;
                    return Json(data);
                }
                else
                {
                    msg = "Data not Found";
                    return Json(msg);
                }
            }
            else
            {
                msg = "Id not Found";
                return Json(msg);
            }
        }
    }
}
