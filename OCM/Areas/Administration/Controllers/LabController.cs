using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OCMDomain.Repository.Edmx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCM.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class LabController : Controller
    {
        private readonly OCMContext _db;
        public LabController(OCMContext db)
        {

            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult LabDetail()
        {
            var lab = _db.LabTbles.ToList();
            return View(lab);
        }

        public IActionResult AddLab()
        {
            return View();
        }

        //Add Lab in db using Post Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddLab(LabTble addlab)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    LabTble lab = new LabTble();
                    lab.Department = addlab.Department;
                    lab.LabName = addlab.LabName;
                    lab.AssistantName = addlab.AssistantName;
                    lab.RoomNo = addlab.RoomNo;
                    lab.SittingCapacity = addlab.SittingCapacity;
                    _db.LabTbles.Add(lab);
                    await _db.SaveChangesAsync();
                    TempData["response"] = "Lab Added Successfully !";
                    return RedirectToAction("LabDetail");
                }
            }
            catch (Exception ex)
            {
                TempData["response"] = "Something wrong " + ex.Message;
                return View();
            }
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> EditLab(int? id)
        {

            if (id == null)
            {
                return RedirectToAction("LabDetail", "Administration");
            }
            var lab = await _db.LabTbles.FindAsync(id);

            if (lab == null)
            {
                return NotFound();
            }

            return View(lab);
        }
        //Update Lab 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditLab(LabTble lab, int id)
        {
            if (id != lab.LabId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _db.LabTbles.Update(lab);
                    await _db.SaveChangesAsync();
                    TempData["response"] = "Lab Successfully Updated !";
                    return RedirectToAction("LabDetail", "Lab");
                }
                catch (Exception ex)
                {
                    TempData["response"] = "Something wrong " + ex.Message;
                    return View();
                }

            }
            return View();
        }

        //Delete Lab
        public async Task<IActionResult> DeleteLab(int? id)
        {

            try
            {
                var delLab = await _db.LabTbles.FindAsync(id);
                if (delLab == null)
                {
                    return RedirectToAction("LabDetail", "Lab");
                }
                else
                {
                    var delresult = _db.LabTbles.Remove(delLab);
                    await _db.SaveChangesAsync();
                    TempData["response"] = "Successfully Deleted !";
                    return RedirectToAction("LabDetail", "Lab");
                }

            }
            catch (Exception ex)
            {
                TempData["response"] = "Something wrong " + ex.Message;
                return View();
            }
        }



        public IActionResult AddAsset()
        {
            try
            {
                var AllLabs = _db.LabTbles.ToList();
                ViewBag.Lab = new SelectList(AllLabs.ToList(), "LabId", "LabName");
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Something wrong " + ex.Message;
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAsset(AssetTble addassests)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AssetTble asset = new AssetTble();

                    asset.AssetName = addassests.AssetName;
                    asset.AssetQuantity = addassests.AssetQuantity;
                    asset.DamagedAsset = addassests.DamagedAsset;

                    LabTble labTble = new LabTble();
                    var getlabname = _db.LabTbles.Where(x => x.LabId == addassests.labtab.LabId).FirstOrDefault();
                    asset.SelectLab = getlabname.LabName;

                    _db.AssetTbles.Add(asset);
                    await _db.SaveChangesAsync();
                    TempData["response"] = "Asset Added Successfully !";
                    return RedirectToAction("AssetDetail");
                }
            }
            catch (Exception ex)
            {
                TempData["response"] = "Something wrong " + ex.Message;
                return View();
            }
            return View();
        }
        [HttpGet]
        public IActionResult AssetDetail()
        {
            var assest = _db.AssetTbles.ToList();
            return View(assest);
        }

        public async Task<IActionResult> DeleteLabAsset(int? id)
        {

            try
            {
                var delLabasset = await _db.AssetTbles.FindAsync(id);
                if (delLabasset == null)
                {
                    return RedirectToAction("AssetDetail");
                }
                else
                {
                    var result = _db.AssetTbles.Remove(delLabasset);
                    await _db.SaveChangesAsync();
                    TempData["response"] = "Successfully Deleted !";
                    return RedirectToAction("AssetDetail");
                }

            }
            catch (Exception ex)
            {
                TempData["response"] = "Something wrong " + ex.Message;
                return View();
            }
        }

    }
}