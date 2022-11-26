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
        //Get index action method
        public IActionResult Index()
        {
            return View(_db.Products.Include(p => p.Category).Include(t => t.Tags).ToList());
        }
        //Post index action method
        [HttpPost]
        public IActionResult index(decimal? LowAmount, decimal? HighAmount)
        {
            var products = _db.Products.Include(p => p.Category).Include(c => c.Tags).Where(c => c.Price>=LowAmount && c.Price <= HighAmount).ToList();
            if(LowAmount==null)
            {
                products = _db.Products.Include(p => p.Category).Include(c => c.Tags).Where(c =>c.Price <= HighAmount).ToList();
            }
            if (HighAmount==null)
            {
                products = _db.Products.Include(p => p.Category).Include(c => c.Tags).Where(c => c.Price >= LowAmount).ToList();
            }
            if(LowAmount==null && HighAmount==null)
            {
                if (LowAmount==null || HighAmount==null)
                {
                    products = _db.Products.Include(p => p.Category).Include(c => c.Tags).ToList();
                }
            }
            return View(products);
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
                var match= _db.Products.FirstOrDefault(c=>c.Name==products.Name);
                if (match!=null)
                {
                    ViewBag.message = "This Product Already Exists!!!";
                    ViewData["productTypeId"]=new SelectList(_db.Categories.ToList(), "Id", "CategoryName");
                    ViewData["TagId"]=new SelectList(_db.Tags.ToList(), "Id", "Tag");
                    return View(products);
                }

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
                var match = _db.Products.FirstOrDefault(c => c.Name==products.Name);
                if (match!=null)
                {
                    ViewBag.message ="This Product Already Exists!!!";
                    ViewData["productTypeId"]=new SelectList(_db.Categories.ToList(), "Id", "CategoryName");
                    ViewData["TagId"]=new SelectList(_db.Tags.ToList(), "Id", "Tag");
                    return View(products);
                }

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
        //GET Delete action method
        public ActionResult Delete(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            var product = _db.Products.Include(c => c.Category).Include(c => c.Tags).Where(c => c.Id==id).FirstOrDefault();
            if (product==null)
            {
                return NotFound();
            }
            
            return View(product);

        }
        //Post Delete action method
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            var product = _db.Products.FirstOrDefault(c=>c.Id==id);
            if (product==null)
            {
                return NotFound();
            }
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
    
}
