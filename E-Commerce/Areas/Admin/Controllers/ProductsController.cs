using E_Commerce.Data;
using E_Commerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace E_Commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private Data.ApplicationDbContext _db;
        private IHostingEnvironment _he;
        public ProductsController(ApplicationDbContext db, IHostingEnvironment he)
        {
            _db=db;
            _he=he;
        }

        public IActionResult Index()
        {
            return View(_db.Products.Include(p => p.Category).Include(t => t.Tags).ToList());
        }
        //Create get action method
        public IActionResult Create()
        {
            ViewData["productTypeId"]=new SelectList(_db.Categories.ToList(), "Id", "CategoryName");
            ViewData["TagId"]=new SelectList(_db.Tags.ToList(), "Id", "Tag");
            return View();

        }
        //Create post action method
        [HttpPost]
        public async Task<IActionResult> Create(Products products, IFormFile? image)
        {
            if (ModelState.IsValid)
            {

                if (image!=null)
                {
                    var name = Path.Combine(_he.WebRootPath+"/Images",Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    products.Image = "Images/"+ image.FileName;
                }
                
                if (image==null)
                {
                    products.Image="Images/No-image-found.jpg";
                }
                _db.Products.Add(products);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(products);
        }
        //Get Edit Method
        public ActionResult Edit(int? id)
        {
            ViewData["productTypeId"]=new SelectList(_db.Categories.ToList(), "Id", "CategoryName");
            ViewData["TagId"]=new SelectList(_db.Tags.ToList(), "Id", "Tag");
            if (id==null)
            {
                return NotFound();
            }
            var product = _db.Products.Include(c=>c.Category).Include(t=>t.Tags).FirstOrDefault(i=>i.Id==id);
            if (product==null)
            {
                return NotFound();
            }
            return View(product);
        }
        //Edit post action method
        [HttpPost]
        public async Task<IActionResult> Edit(Products products, IFormFile? image)
        {
            if (ModelState.IsValid)
            {

                if (image!=null)
                {
                    var name = Path.Combine(_he.WebRootPath+"/Images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    products.Image = "Images/"+ image.FileName;
                }

                if (image==null)
                {
                    products.Image="Images/No-image-found.jpg";
                }
                _db.Products.Update(products);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(products);
        }
        //GET Details action method
        public ActionResult Details(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            var product = _db.Products.Include(c => c.Category).Include(c => c.Tags).FirstOrDefault(c => c.Id==id);
            if (product==null)
            {
                return NotFound();
            }
            return View(product);
        }
    }
    
}
