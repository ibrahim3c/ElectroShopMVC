using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.Web.Constants;
using MyShop.Web.Services.Interfaces;

namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =Roles.AdminRole)]
    public class DashboardsController:Controller
    {
        private readonly IDashboardService dashboardService;

        public DashboardsController(IDashboardService dashboardService)
        {
            this.dashboardService = dashboardService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View(dashboardService.GetDashboardData());
        }
    }
}
