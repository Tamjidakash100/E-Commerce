using E_Commerce.Data;
using E_Commerce.Models;
using E_Commerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            return RedirectToAction(nameof(Index));
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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}