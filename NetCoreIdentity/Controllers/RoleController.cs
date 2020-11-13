using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols;
using NetCoreIdentity.Context;
using NetCoreIdentity.Models;

namespace NetCoreIdentity.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        public RoleController(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View(_roleManager.Roles.ToList());
        }

        public IActionResult AddRole()
        {
            return View(new RoleViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> AddRole(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppRole role = new AppRole
                {
                    Name = model.Name
                };
                var identityResult = await _roleManager.CreateAsync(role);
                if (identityResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var item in identityResult.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }

            return View(model);
        }

        public IActionResult UpdateRole(int id)
        {
            var role = _roleManager.Roles.FirstOrDefault(I => I.Id == id);
            RoleUpdateViewModel model = new RoleUpdateViewModel
            {
                Id = role.Id,
                Name = role.Name
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateRole(RoleUpdateViewModel model)
        {
            var tobeUpdatedRole = _roleManager.Roles.Where(I => I.Id == model.Id).FirstOrDefault();

            tobeUpdatedRole.Name = model.Name;
            var identityResult = await _roleManager.UpdateAsync(tobeUpdatedRole);
            if (identityResult.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var item in identityResult.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> DeleteRole(int id)
        {
            var tobeDeletedRole = _roleManager.Roles.FirstOrDefault(i => i.Id == id);
            var identityResult = await _roleManager.DeleteAsync(tobeDeletedRole);
            if (identityResult.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Errors"] = identityResult.Errors;
                return RedirectToAction("Index");
            }
        }
        public IActionResult UserList()
        {
            return View(_userManager.Users.ToList());
        }

        public async Task<IActionResult> AssignRole(int id)
        {
            var user = _userManager.Users.FirstOrDefault(I => I.Id == id);
            var roles = _roleManager.Roles.ToList();
            var userRoles = await _userManager.GetRolesAsync(user);
            TempData["userId"] = user.Id;

            List<RoleAssignViewModel> models = new List<RoleAssignViewModel>();
            foreach (var item in roles)
            {
                RoleAssignViewModel model = new RoleAssignViewModel();
                model.RoleId = item.Id;
                model.Name = item.Name;
                model.Exists = userRoles.Contains(item.Name);
                models.Add(model);
            }
            return View(models);
        }
        [HttpPost]
        public async Task<IActionResult> AssignRole(List<RoleAssignViewModel> models)
        {
            var userId = (int)TempData["userId"];
            var user = _userManager.Users.FirstOrDefault(I => I.Id == userId);
            foreach (var item in models)
            {
                if (item.Exists)
                {
                    await _userManager.AddToRoleAsync(user, item.Name);
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(user, item.Name);
                }
            }
            return RedirectToAction("UserList");
        }
    }
}