using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportStore.Models.Entities;
using SportStore.WebUI.Interfaces;
using SportStore.WebUI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportStore.WebUI.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly IUrlService _urlService;

        public int PageSize { get; } = 4;

        public UsersController(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager, IUrlService urlService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _urlService = urlService;
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
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            User user = await _userManager.FindByIdAsync(model.User.Id.ToString());

            if (user == null)
                return NotFound();

            user.FirstName = model.User.FirstName;
            user.LastName = model.User.LastName;
            user.Email = model.User.Email;
            user.PhoneNumber = model.User.PhoneNumber;

            var userRoles = await _userManager.GetRolesAsync(user);
            var addedRoles = model.ActiveRoles.Except(userRoles);
            var removedRoles = userRoles.Except(model.ActiveRoles);

            await _userManager.AddToRolesAsync(user, addedRoles);
            await _userManager.RemoveFromRolesAsync(user, removedRoles);
            await _userManager.UpdateAsync(user);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, int pageSize, string queries)
        {
            string redirectUrl = _urlService.ReditectUrlForDelete(id, pageSize, queries);

            User user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
                return NotFound();

            await _userManager.DeleteAsync(user);

            return Redirect(redirectUrl);
        }
    }
}
