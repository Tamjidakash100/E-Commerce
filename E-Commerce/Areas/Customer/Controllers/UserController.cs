using E_Commerce.Data;
using E_Commerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class UserController : Controller
    {
        ApplicationDbContext _db;
        UserManager<IdentityUser> _userManager;
        public UserController(UserManager<IdentityUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db=db;
        }
        public IActionResult Index()
        {
            return View(_db.ApplicationUsers.ToList());
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ApplicationUsers users)
        {
            if(ModelState.IsValid)
            {
                var res = await _userManager.CreateAsync(users, users.PasswordHash);
                if (res.Succeeded)
                {
                    TempData["Save"] = "User Has Been Created Successfully";
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in res.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                
            }
            return View();

        }
        //Get Edit action method
        public IActionResult Edit(string id)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(c => c.Id==id);
            if(user==null)
            {
                return NotFound();
            }
            return View(user);
        }
        //Post Edit action method
        [HttpPost]
        public async Task<IActionResult> Edit(ApplicationUsers user)
        {
            var userinfo = _db.ApplicationUsers.FirstOrDefault(c => c.Id==user.Id);
            if (userinfo==null)
            {
                return NotFound();
            }
            userinfo.FirstName = user.FirstName;
            userinfo.LastName = user.LastName;
            await _userManager.UpdateAsync(userinfo);
            return RedirectToAction(nameof(Index));
        }
        //Get details action method
        public IActionResult Details(string id)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(c => c.Id==id);
            if (user==null)
            {
                return NotFound();
            }
            return View(user);
        }
        //Get delete action method
        public IActionResult Ban(string id)
        {
            if (id==null)
            {
                return NotFound();
            }
            var user = _db.ApplicationUsers.FirstOrDefault(c => c.Id==id);
            if (user==null)
            {
                return NotFound();
            }
            return View(user);
        }
        //Post delete action method
        [HttpPost]
        public async Task<IActionResult> Ban(ApplicationUsers user)
        {
            var userinfo = _db.ApplicationUsers.FirstOrDefault(c=>c.Id==user.Id);
            if(userinfo==null)
            {
                return NotFound();
            }
            userinfo.LockoutEnd = DateTime.Now.AddYears(10);
            userinfo.LockoutEnabled = true;
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        //Get Active Action Method
        public IActionResult Active(string? id)
        {
            if(id==null)
            {
                return NotFound();
            }
            var user = _db.ApplicationUsers.FirstOrDefault(c => c.Id==id);
            if (user==null)
            {
                return NotFound();
            }
            return View(user);
        }
        //Post Active Action Method
        [HttpPost]
        public async Task<IActionResult> Active(ApplicationUsers user)
        {
            var userinfo = _db.ApplicationUsers.FirstOrDefault(c=>c.Id == user.Id);
            if(userinfo==null)
            {
                return NotFound();
            }
            userinfo.LockoutEnd= null;
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        //Get Detele Action Method
        public IActionResult Delete(string? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            var user = _db.ApplicationUsers.FirstOrDefault(c => c.Id==id);
            if (user==null)
            {
                return NotFound();
            }
            return View(user);
        }
        //Post Detele Action Method
        [HttpPost]
        public async Task<IActionResult> Delete(ApplicationUsers user)
        {
            var userinfo = _db.ApplicationUsers.FirstOrDefault(c => c.Id == user.Id);
            if (userinfo==null)
            {
                return NotFound();
            }
            _db.ApplicationUsers.Remove(userinfo);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

    }
}
