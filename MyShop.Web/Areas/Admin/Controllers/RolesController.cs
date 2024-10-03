using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.DAL.Repositories;
using MyShop.Web.Constants;
using MyShop.Web.ViewModels;

namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =Roles.AdminRole)]
    public class RolesController:Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await roleManager.Roles.ToListAsync());
        }

        public async Task<IActionResult> Create(CreatedRole role)
        {
            if(role.roleName == null)
            {
                ModelState.AddModelError("roleName", "you must enter Role  !");
                return RedirectToAction(nameof(Index));
            }
                
            if (await roleManager.RoleExistsAsync(role.roleName))
            {
                ModelState.AddModelError("roleName", "Role is already exist !");
            }
            else
            {
            await roleManager.CreateAsync(new IdentityRole(role.roleName));

            }
            return RedirectToAction(nameof(Index));

        }
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
                return NotFound();

            var role = await roleManager.Roles.FirstOrDefaultAsync(r=>r.Id==id);
            if (role is null)
                return NotFound();

            await roleManager.DeleteAsync(role);

            return RedirectToAction(nameof(Index));

        }
    }
}
