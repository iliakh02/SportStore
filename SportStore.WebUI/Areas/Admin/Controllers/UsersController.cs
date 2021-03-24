using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportStore.Models.Entities;
using SportStore.WebUI.Areas.Admin.Models;
using SportStore.WebUI.Models;
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

        public int PageSize { get; } = 4;

        public UsersController(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index(int page = 1, UsersSortState sortOrder = UsersSortState.IdAsc)
        {
            ViewData["IdSort"] = sortOrder == UsersSortState.IdAsc ? UsersSortState.IdDesk : UsersSortState.IdAsc;
            ViewData["FirstNameSort"] = sortOrder == UsersSortState.FirstNameAsc ? UsersSortState.FirstNameDesc : UsersSortState.FirstNameAsc;
            ViewData["LastNameSort"] = sortOrder == UsersSortState.LastNameAsc ? UsersSortState.LastNameDesc : UsersSortState.LastNameAsc;

            var users = _userManager.Users;

            IQueryable<User> sortUsers = sortOrder switch
            {
                UsersSortState.IdDesk => users.OrderByDescending(n => n.Id),
                UsersSortState.FirstNameAsc => users.OrderBy(n => n.FirstName),
                UsersSortState.FirstNameDesc => users.OrderByDescending(n => n.FirstName),
                UsersSortState.LastNameAsc => users.OrderBy(n => n.LastName),
                UsersSortState.LastNameDesc => users.OrderByDescending(n => n.LastName),
                _ => users.OrderBy(n => n.Id),
            };
            
            List<string> rolesPerPage = new List<string>();
            List<User> usersPerPage = sortUsers.ToList().Skip((page - 1) * PageSize).Take(PageSize).ToList();
            foreach (var user in usersPerPage)
            {
                var rolesPerUser = await _userManager.GetRolesAsync(user);
                rolesPerPage.Add(string.Join(", ", rolesPerUser));
            }

            UsersViewModel usersViewModel = new UsersViewModel
            {
                Users = usersPerPage,
                Roles = rolesPerPage,
                SortViewModel = new UsersSortViewModel(sortOrder),
                PageModel = new PageViewModel(_userManager.Users.Count(), page, PageSize)
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
