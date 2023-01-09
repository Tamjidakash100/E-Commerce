using E_Commerce.Data;
using E_Commerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;

namespace E_Commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Super Admin")]
    public class TagsController : Controller
    {
        private ApplicationDbContext _db;
        public TagsController(ApplicationDbContext db)
        {
            _db=db;
        }

        public IActionResult Index()
        {
            return View(_db.Tags.ToList());
        }
        //Create Get Action Method
        public ActionResult Create()
        {
            return View();
        }
        //Create Post Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tags tags)
        {
            if (ModelState.IsValid)
            {
                _db.Tags.Add(tags);
                await _db.SaveChangesAsync();
                TempData["Save"]="Saved Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(tags);
        }
        //Edit Get Action method
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var tag = _db.Tags.Find(id);
            if (tag == null)
            {
                return NotFound();
            }
            return View(tag);
        }
        //Edit Post Action method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Tags tags)
        {
            if (ModelState.IsValid)
            {
                _db.Update(tags);
                await _db.SaveChangesAsync();
                TempData["Edit"]="Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(tags);
        }
        //GET Details Action Method

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Tag = _db.Tags.Find(id);
            if (Tag == null)
            {
                return NotFound();
            }
            return View(Tag);
        }

        //POST Details Action Method

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(Tags Tags)
        {
            return RedirectToAction(nameof(Index));

        }

        //Delete Get Action method
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var tag = _db.Tags.Find(id);
            if (tag == null)
            {
                return NotFound();
            }
            return View(tag);
        }
        //Delete Post Action method
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> Remove(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            var tag = _db.Tags.FirstOrDefault(c=>c.Id==id);
            if (tag==null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _db.Remove(tag);
                await _db.SaveChangesAsync();
                TempData["Delete"]="Deleted Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(tag);
        }
    }
}
