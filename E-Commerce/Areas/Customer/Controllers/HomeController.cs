using E_Commerce.Data;
using E_Commerce.Models;
using E_Commerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using X.PagedList;

namespace E_Commerce.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private ApplicationDbContext _db;

        private readonly ILogger<HomeController> _logger;
        
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {

            _db= db;
            _logger = logger;
        }

        public IActionResult Index(int? page)
        {
            List<Category> categories= _db.Categories.ToList();
            ViewBag.CatList= categories;
            ViewData["categories"]=new SelectList(_db.Categories.ToList(), "Id", "CategoryName");
            return View(_db.Products.Include(c=> c.Category).Include(c=>c.Tags).ToList().ToPagedList(pageNumber:page??1,pageSize:10));

        }
        //Get Details action method
        public IActionResult Details(int? id)
        {
            if(id== null)
            {
                return NotFound();
            }
            var products = _db.Products.Include(c => c.Category).FirstOrDefault(c => c.Id == id);
            if(products == null)
            {
                return NotFound();
            }
            return View(products);
        }
        //Post Details action method
        [HttpPost]
        [ActionName("Details")]
        public IActionResult ProductDetails(int? id, Products getproduct)
        {
            List<Products> products = new List<Products>();
            if (id==null)
            {
                return NotFound();
            }
            var product = _db.Products.Include(c => c.Category).FirstOrDefault(c => c.Id==id);
            
            if (product==null)
            {
                return NotFound();
            }
            product.Quantity = getproduct.Quantity;
            var PQuantity = 
            products = HttpContext.Session.Get<List<Products>>("products");
            if (products==null)
            {
                products = new List<Products>();
            }
            products.Add(product);
            HttpContext.Session.Set("products", products);
            return RedirectToAction(nameof(Cart));
        }
        
        public IActionResult Remove(int? id)
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            
            if (products!=null)
            {
                var product = products.FirstOrDefault(c => c.Id==id);
                if (product!=null)
                {
                    products.Remove(product);
                    HttpContext.Session.Set("products", products);
                }
            }
            
            return RedirectToAction(nameof(Cart));
        }
        [HttpPost]
        [ActionName("Remove")]
        public IActionResult PRemove(int? id)
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");

            if (products!=null)
            {
                var product = products.FirstOrDefault(c => c.Id==id);
                if (product!=null)
                {
                    products.Remove(product);
                    HttpContext.Session.Set("products", products);
                }
            }

            return RedirectToAction(nameof(Index));
        }
        //Get cart action method
        [Authorize]
        public IActionResult Cart()
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if (products==null)
            {
                products =new List<Products>();
            }
            return View(products);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Cart(string? token)
        {
            if (token==null)
            {
                return View();
            }
            if(token =="ekushe52")
            {
                ViewBag.offer = 21;
                HttpContext.Session.SetInt32("offer", 21);
            }
            else
            {
                ViewBag.message ="This Token is Invalid";
            }
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if (products==null)
            {
                products =new List<Products>();
            }
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult ProductsByCategory(int? id, int? page)
        {
            if (id==null)
            {
                return NotFound();
            }
            List<Category> categories = _db.Categories.ToList();
            ViewBag.CatList = categories;
            return View(_db.Products.Include(c => c.Category).Include(c => c.Tags).Where(c => c.CategoryId==id).ToList().ToPagedList(pageNumber: page??1, pageSize: 10));
        }
        public IActionResult Search(int? id,string? query, int? page)
        {
            List<Category> categories = _db.Categories.ToList();
            ViewBag.CatList = categories;
            return View(_db.Products.Include(c => c.Category).Include(c => c.Tags).Where(c=>c.CategoryId==id && (c.Name.Contains(query) || c.Details.Contains(query))).ToList().ToPagedList(pageNumber: page??1, pageSize: 10));
        }


    }
}