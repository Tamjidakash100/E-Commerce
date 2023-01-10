using E_Commerce.Areas.Customer.Controllers;
using E_Commerce.Data;
using E_Commerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_Commerce.Components
{
    public class CategoriesViewComponent: ViewComponent
    {
        private ApplicationDbContext _db;
        public CategoriesViewComponent(ApplicationDbContext db)
        {
            _db=db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.CategoryList=new SelectList(_db.Categories, "Id", "CategoryName");

            return View();
        }
    }
}
