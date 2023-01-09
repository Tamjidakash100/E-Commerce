using E_Commerce.Areas.Admin.Models;
using E_Commerce.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_Commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Super Admin")]
    public class RoleController : Controller
    {
        UserManager<IdentityUser> _userManager;
        RoleManager<IdentityRole> _rolemanager;
        ApplicationDbContext _db;
        public RoleController(ApplicationDbContext db, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _db=db;
            _rolemanager = roleManager;
            _userManager = userManager;
        }
        //Get Index Action Method
        public IActionResult Index()
        {
            var roles = _rolemanager.Roles.ToList();
            ViewBag.Roles=roles;
            return View(ViewBag.Roles);
        }
        //Get Create Action Method
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }
        //Post Create Action Method
        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            bool isexist = await _rolemanager.RoleExistsAsync(name);
            if (isexist)
            {
                ViewBag.message= "Role Name Already Exists!!!!";
                ViewBag.name= name;
                return View();
            }
            IdentityRole role= new IdentityRole();
            role.Name= name;
            var result = await _rolemanager.CreateAsync(role);
            if (result.Succeeded)
            {
                TempData["Save"]= "Role Is Saved Successfully";
            }
            return RedirectToAction(nameof(Index));
        }
        //Get Edit Action Method
        public async Task<IActionResult> Edit(string id)
        {
            var role= await _rolemanager.FindByIdAsync(id);
            if(role==null)
            {
                return NotFound();
            }
            ViewBag.id=role.Id;
            ViewBag.name=role.Name;

            return View();
        }
        //Post Edit Action Method
        [HttpPost]
        public async Task<IActionResult> Edit(string id, string name)
        {
            var role = await _rolemanager.FindByIdAsync(id);
            if (role==null)
            {
                return NotFound();
            }
            bool isexist = await _rolemanager.RoleExistsAsync(name);
            if (isexist)
            {
                ViewBag.message= "Role Name Already Exists!!!!";
                ViewBag.name= name;
                ViewBag.id= id;

                return View();
            }
            role.Name= name;
            var result = await _rolemanager.UpdateAsync(role);
            if (result.Succeeded)
            {
                TempData["Edit"]= "Role Has been Updated Successfully";
            }
            return RedirectToAction(nameof(Index));
        }
        //Get Delete Action Method
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _rolemanager.FindByIdAsync(id);
            if (role==null)
            {
                return NotFound();
            }
            ViewBag.id=role.Id;
            ViewBag.name=role.Name;

            return View();
        }
        //Post Delete Action Method
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(string id)
        {
            var role = await _rolemanager.FindByIdAsync(id);
            if (role==null)
            {
                return NotFound();
            }
            var result = await _rolemanager.DeleteAsync(role);
            if (result.Succeeded)
            {
                TempData["Delete"]= "Role Has been Deleted Successfully";
            }
            return RedirectToAction(nameof(Index));
        }
        //Get Assign action method
        public IActionResult Assign()
        {
            ViewData["UserId"] = new SelectList(_db.ApplicationUsers.ToList(),"Id","UserName");
            ViewData["RoleId"] = new SelectList(_rolemanager.Roles.ToList(),"Name","Name");
            return View();
        }
        //Post Assign action method
        [HttpPost]
        public async Task<IActionResult> Assign(AssignRolesVM assignRoles)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(c => c.Id==assignRoles.UserId);
            bool exist = await _userManager.IsInRoleAsync(user, assignRoles.RoleId);
            if (exist)
            {
                ViewData["UserId"] = new SelectList(_db.ApplicationUsers.Where(c=>c.LockoutEnd<DateTime.Now || c.LockoutEnd==null).ToList(), "Id", "UserName");
                ViewData["RoleId"] = new SelectList(_rolemanager.Roles.ToList(), "Name", "Name");
                ViewBag.message = "This role is already assigned to this user";
                return View();
            }

            var role = await _userManager.AddToRoleAsync(user,assignRoles.RoleId);
            if (role.Succeeded)
            {
                TempData["Save"]="User Role has been assigned";
            }
            return RedirectToAction(nameof(Index));
        }
        public ActionResult AssignRoles()
        {
            var result = from ur in _db.UserRoles
                         join r in _db.Roles on ur.RoleId equals r.Id
                         join au in _db.ApplicationUsers on ur.UserId equals au.Id
                         select new UserRoleMaping()
                         {
                             UserId = ur.UserId,
                             RoleId = ur.RoleId,
                             UserName = au.UserName,
                             RoleName = r.Name
                         };
            ViewBag.UserRoles = result;
            return View();
        }

    }
}
