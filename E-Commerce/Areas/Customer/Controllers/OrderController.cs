using E_Commerce.Data;
using E_Commerce.Models;
using E_Commerce.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Stripe;
using System.Security.Claims;

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
        public async Task<IActionResult> Checkout(Orders order, string stripeEmail, string stripeToken)
        {
            var customers = new CustomerService();
            var charges = new ChargeService();

            var customer = customers.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });
            var charge = charges.Create(new ChargeCreateOptions
            {
                Amount = Convert.ToInt32(order.Total*100),
                Description="Test Payment",
                Currency="usd",
                Customer = customer.Id
            }); ;
            if (charge.Status=="Succeeded")
            {
                string BalanceTransactionId = charge.BalanceTransactionId;
                order.OrderNo=GetOrderNo();
                _db.Orders.Add(order);
                await _db.SaveChangesAsync();
                HttpContext.Session.Set("products", new List<Products>());
                
            }
            return View();

        }
        public string GetOrderNo()
        {
            int RCount = _db.Orders.ToList().Count()+1;
            return RCount.ToString("0000");
        }
        public IActionResult charge()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Charge(string stripeEmail, string stripeToken)
        {
            var customers = new CustomerService();
            var charges = new ChargeService();

            var customer = customers.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });
            var charge = charges.Create(new ChargeCreateOptions
            {
                Amount = 500,
                Description="Test Payment",
                Currency="usd",
                Customer = customer.Id
            }) ;
            if (charge.Status=="Succeeded")
            {
                string BalanceTransactionId = charge.BalanceTransactionId;
            }
            return View();
        }
    }
}
