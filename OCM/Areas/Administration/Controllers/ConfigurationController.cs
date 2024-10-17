using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OCMDomain.Repository.Edmx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCM.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class ConfigurationController : Controller
    {

        private readonly OCMContext _db;

        public ConfigurationController(OCMContext db)
        {
            _db = db;
        }
        //Batch List
        public IActionResult Index()
        {
            var data = new BatchValidation();
            data.BatchNameList = _db.BatchTbles.ToList();
            return View(data);

        }

        public IActionResult AddBatch()
        {
            return View();
        }

        //Post batch in db
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBatch(BatchTble batch)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BatchTble ba = new BatchTble();
                    ba.BtachName = batch.BtachName;
                    ba.StartDate = batch.StartDate;
                    ba.EndDate = batch.EndDate;
                    if (ba.EndDate <= ba.StartDate)
                    {
                        return RedirectToAction("AddBatch");

                    }
                    _db.BatchTbles.Add(ba);
                    await _db.SaveChangesAsync();
                    TempData["response"] = "Batch Added Successfully!";
                    return RedirectToAction("Index");

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
        public async Task<IActionResult> EditBatch(int? id)
        {

            if (id == null)
            {
                return RedirectToAction("Index", "Administration");
            }
            var batch = await _db.BatchTbles.FindAsync(id);

            if (batch == null)
            {
                return NotFound();
            }
            return View(batch);
        }
        //Update Batch 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBatch(BatchTble batches, int id)
        {

            if (id != batches.BatchId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _db.BatchTbles.Update(batches);
                    await _db.SaveChangesAsync();
                    TempData["response"] = "Successfully Updated !";
                    return RedirectToAction("Index", "Configuration");
                }
                catch (Exception ex)
                {
                    TempData["response"] = "Something wrong " + ex.Message;
                    return View();
                }

            }
            return View();
        }

        //Delete Batch
        [HttpGet]
        public async Task<IActionResult> DeleteBatch(int? id)
        {

            try
            {
                var delbatch = await _db.BatchTbles.FindAsync(id);
                if (delbatch == null)
                {
                    return RedirectToAction("Index", "Configuration");
                }
                else
                {
                    var delresult = _db.BatchTbles.Remove(delbatch);
                    await _db.SaveChangesAsync();
                    TempData["response"] = "Successfully Deleted !";
                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                TempData["response"] = "Something wrong " + ex.Message;
                return View();
            }
        }

        public IActionResult MangeFaq()
        {
            var FaqList = _db.FaqsTbles.ToList();
            return View(FaqList);

        }
        public async Task<IActionResult> FAQ()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FAQ(FaqsTble faqs)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    FaqsTble faqsTble = new FaqsTble();
                    faqsTble.Title = faqs.Title;
                    faqsTble.Description = faqs.Description;
                    faqsTble.Date = faqs.Date;
                    _db.FaqsTbles.Add(faqsTble);
                    await _db.SaveChangesAsync();
                    TempData["response"] = "Successfully Added !";
                    return RedirectToAction("MangeFaq");
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
        public async Task<IActionResult> EditFaq(int? id)
        {

            if (id == null)
            {
                return RedirectToAction("MangeFaq", "Administration");
            }
            var faq = await _db.FaqsTbles.FindAsync(id);

            if (faq == null)
            {
                return NotFound();
            }
            return View(faq);
        }
        //Update Batch 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFaq(FaqsTble faqsTble, int id)
        {
            if (id != faqsTble.FaqId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _db.FaqsTbles.Update(faqsTble);
                    await _db.SaveChangesAsync();
                    TempData["response"] = "Successfully Updated !";
                    return RedirectToAction("MangeFaq", "Configuration");
                }
                catch (Exception ex)
                {
                    TempData["response"] = "Something wrong " + ex.Message;
                    return View();
                }

            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> DeleteFaq(int? id)
        {

            try
            {
                var delfaq = await _db.FaqsTbles.FindAsync(id);
                if (delfaq == null)
                {
                    return RedirectToAction("MangeFaq", "Configuration");
                }
                else
                {
                    var delresult = _db.FaqsTbles.Remove(delfaq);
                    await _db.SaveChangesAsync();
                    TempData["response"] = "Successfully Deleted !";
                    return RedirectToAction("MangeFaq", "Configuration");
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
