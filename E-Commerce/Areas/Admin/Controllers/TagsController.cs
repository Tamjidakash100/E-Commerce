using E_Commerce.Data;
using E_Commerce.Models;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;

namespace E_Commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
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
                return RedirectToAction(nameof(Index));
            }
            return View(tags);
        }
    }
}
