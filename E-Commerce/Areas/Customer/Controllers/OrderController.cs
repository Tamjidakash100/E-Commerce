using E_Commerce.Data;
using E_Commerce.Models;
using E_Commerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Stripe;
using System;
using System.Security.Claims;

namespace E_Commerce.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
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
        public async Task<IActionResult> Checkout(Orders order, string stripeEmail, string stripeToken, string? token)
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

                Amount = Convert.ToInt32((order.Total)*100),
                Description="Test Payment",
                Currency="usd",
                Customer = customer.Id
            }); ;
            if (charge.Status=="succeeded")
            {
                ViewBag.success= true;
                ViewBag.Message = "Payment Successful";
                string BalanceTransactionId = charge.BalanceTransactionId;
                
                order.OrderNo=GetOrderNo();
                List<Products> products = HttpContext.Session.Get<List<Products>>("products");
                OrderDetails orderDetails = new OrderDetails();
                
                _db.Orders.Add(order);
                await _db.SaveChangesAsync();
                var orderId = _db.Orders.FirstOrDefault(c => c.OrderNo == order.OrderNo);
                foreach (var i in products)
                {
                    orderDetails.ProductId = i.Id;
                    orderDetails.Quantity = i.Quantity;
                    orderDetails.OrderId = orderId.Id;
                    _db.OrderDetails.Add(orderDetails);
                    _db.SaveChanges();
                }
                ViewBag.orderNo= order.OrderNo;
               
                HttpContext.Session.Set("order", products);
                HttpContext.Session.Set("products", new List<Products>());
                return RedirectToAction(nameof(GenerateReceipt));
            }
            ViewBag.Message = "Payment Failed";
            return View();
            

        }
        public string GetOrderNo()
        {
            int RCount = _db.Orders.Count()+1;
            return RCount.ToString("0000");
        }
        public IActionResult GenerateReceipt()
        {
            return View();
        }
    }
}
