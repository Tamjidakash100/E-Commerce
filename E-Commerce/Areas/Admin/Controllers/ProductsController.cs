using E_Commerce.Data;
using E_Commerce.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace E_Commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private ApplicationDbContext _db;
        private IHostingEnvironment _he;
        public ProductsController(ApplicationDbContext db, IHostingEnvironment he)
        {
            _db=db;
            _he=he;
        }

        public IActionResult Index()
        {
            return View(_db.Products.Include(p=>p.Category).Include(t=>t.Tags).ToList());
        }
        //Create get action method
        public IActionResult Create()
        {
            return View();
        }
        //Create post action method
        [HttpPost]
        public async Task<IActionResult> Create(Products products, IFormFile image)
        {
            if(image!=null)
            {
                var name = Path.Combine(_he.WebRootPath+"/Images"+Path.GetFileName(image.FileName));
                await image.CopyToAsync(new FileStream(name, FileMode.Create));
                products.Image = "Images/"+ image.FileName;
            }
            if (ModelState.IsValid)
            {
                _db.Products.Add(products);
                await _db.SaveChangesAsync();
                return RedirectToAction (nameof(Index));
            }
            return View(products);
        }
    }
}
