using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetProject.Models;
using NetProject.ViewModels;

namespace NetProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: /Admin
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.Select(r => r.Name!).ToListAsync();

            var users = await _userManager.Users.ToListAsync();
            var vm = new List<UserWithRolesViewModel>();

            foreach (var u in users)
            {
                var userRoles = await _userManager.GetRolesAsync(u);
                vm.Add(new UserWithRolesViewModel
                {
                    Id = u.Id,
                    Email = u.Email!,
                    FullName = u.FullName ?? u.Email!,
                    Roles = userRoles,
                    AllRoles = roles,
                    SelectedRole = userRoles.FirstOrDefault() ?? string.Empty
                });
            }

            return View(vm);
        }

        // POST: /Admin/Index
        [HttpPost]
        public async Task<IActionResult> Index(List<UserWithRolesViewModel> model)
        {
            foreach (var item in model)
            {
                var user = await _userManager.FindByIdAsync(item.Id);
                if (user == null) continue;

                var currentRoles = await _userManager.GetRolesAsync(user);
                // usuwamy wszystkie
                await _userManager.RemoveFromRolesAsync(user, currentRoles);

                if (!string.IsNullOrEmpty(item.SelectedRole))
                {
                    // dodajemy wybraną
                    await _userManager.AddToRoleAsync(user, item.SelectedRole);
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
