using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Entities.Models;
using MyShop.Web.Constants;
using MyShop.Web.ViewModels;
using System.Security.Claims;

namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.AdminRole)]

    public class UsersController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UsersController(UserManager<AppUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public IActionResult Index()
        {
            var claimIdentity = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (claimIdentity == null)
                return BadRequest();

                var userId = claimIdentity.Value;
            var users = userManager.Users.Where(u => u.Id != userId).Select(u => new UserForIndexVM
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                UserName = u.UserName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Address = u.Address,
                IsLocked = u.LockoutEnd != null && u.LockoutEnd > DateTime.UtcNow

            });
            
            return View(users);
        }
        public async Task<IActionResult> LockUnLock(string id)
        {
            // Find the user by their Id
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return NotFound();

            // If user is not locked out, lock them for 1 year
            if (user.LockoutEnd == null || user.LockoutEnd < DateTime.UtcNow)
            {
                user.LockoutEnd = DateTime.UtcNow.AddYears(1);  // Lock the user
            }
            else
            {
                // Unlock the user by setting LockoutEnd to null
                user.LockoutEnd = null;
            }

            // Update the user in the database
            await userManager.UpdateAsync(user);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ManageRoles(string Id)
        {
            var user = await userManager.FindByIdAsync(Id);

            if (user == null)
                return NotFound();

            var roles = await roleManager.Roles.ToListAsync();

            var viewModel = new UserRolesVM
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = roles.Select(role => new RoleVM
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    IsSelected = userManager.IsInRoleAsync(user, role.Name).Result
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageRoles(UserRolesVM model)
        {
            var user = await userManager.FindByIdAsync(model.UserId);

            if (user == null)
                return NotFound();

            var userRoles = await userManager.GetRolesAsync(user);

            foreach (var role in model.Roles)
            {
                if (userRoles.Any(r => r == role.RoleName) && !role.IsSelected)
                    await userManager.RemoveFromRoleAsync(user, role.RoleName);

                if (!userRoles.Any(r => r == role.RoleName) && role.IsSelected)
                    await userManager.AddToRoleAsync(user, role.RoleName);
            }

            return RedirectToAction(nameof(Index));
        }


    }
}
