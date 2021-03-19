using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportStore.Models.Entities;
using SportStore.WebUI.Areas.Admin.Models;
using SportStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportStore.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class UsersController : Controller
    {
        UserManager<User> _userManager;
        RoleManager<IdentityRole<int>> _roleManager;
        public UsersController(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 3;
            List<string> rolesPerPage = new List<string>();
            List<User> usersPerPage = _userManager.Users.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            foreach (var user in usersPerPage)
            {
                var rolesPerUser = await _userManager.GetRolesAsync(user);
                rolesPerPage.Add(string.Join(", ", rolesPerUser));
            }

            UsersViewModel usersViewModel = new UsersViewModel
            {
                Users = usersPerPage,
                Roles = rolesPerPage,
                PageModel = new PageViewModel(_userManager.Users.Count(), page, pageSize)
            };

            return View(usersViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int userId)
        {
            User user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return NotFound();

            var roles = await _userManager.GetRolesAsync(user);

            UserEditViewModel userEditViewModel = new UserEditViewModel
            {
                User = user,
                AllRoles = _roleManager.Roles.ToList(),
                ActiveRoles = roles.ToList()
            };

            return View(userEditViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int userId, List<string> roles)
        {
            User user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);
            var addedRoles = roles.Except(userRoles);
            var removedRoles = userRoles.Except(roles);

            await _userManager.AddToRolesAsync(user, addedRoles);
            await _userManager.RemoveFromRolesAsync(user, removedRoles);

            return RedirectToAction("Index");
        }

        [HttpPost]

        public async Task<IActionResult> Delete(int id)
        {
            User user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
                return NotFound();

            await _userManager.DeleteAsync(user);

            return RedirectToAction("Index");
        }
    }
}
