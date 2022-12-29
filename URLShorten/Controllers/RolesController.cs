using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using URLShorten.Models;

namespace MyApp.Controllers
{
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public class YourModelClass
        {
            public List<IdentityRole> Roles { get; set; }
            public List<IdentityUser> Users { get; set; }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(name);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Role did not found");
            }
            return View("Index", _roleManager.Roles);
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            IdentityRole role = _roleManager.Roles.FirstOrDefault(r => r.Id == id);
            if (role == null)
            {
                return RedirectToAction("ListRoles");
            }
            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(IdentityRole role)
        {
            IdentityRole roleFromDb = _roleManager.Roles.FirstOrDefault(r => r.Id == role.Id);
            if (roleFromDb != null)
            {
                roleFromDb.Name = role.Name;
                IdentityResult result = await _roleManager.UpdateAsync(roleFromDb);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Role not found");
            }
            return View(role);
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AssignRole(string userId, string role)
        {

            var model = new UserRoleManager();
            model.Users = _userManager.Users.ToList();
            model.Roles = _roleManager.Roles.ToList();
          

            IdentityUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                IdentityResult result = await _userManager.AddToRoleAsync(user, role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "User not found");
            }
              return View(model);
        }
    }
}