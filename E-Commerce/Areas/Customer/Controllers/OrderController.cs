using E_Commerce.Data;
using E_Commerce.Models;
using E_Commerce.Utility;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class OrderController : Controller
    {
        private ApplicationDbContext _db;
        public OrderController (ApplicationDbContext db)
        {
            _db=db;
        }
        //Get checkout action method
        public IActionResult Checkout()
        {
            return View();
        }
        //Post Checkout action method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(Orders order)
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if (products != null)
            {
                foreach(var prod in products)
                {
                    OrderDetails orderDetails = new OrderDetails();
                    orderDetails.ProductId = prod.Id;
                    order.OrderDetails.Add(orderDetails);
                }
            }
            order.OrderNo=GetOrderNo();
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();
            HttpContext.Session.Set("products", new List<Products>());
            return View();
        }
        public string GetOrderNo()
        {
            int RCount = _db.Orders.ToList().Count()+1;
            return RCount.ToString("0000");
        }
    }
}
