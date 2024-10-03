using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using MyShop.Web.Services.Implementations;
using MyShop.Web.Services.Interfaces;
using MyShop.Web.ViewModels;
using System.Security.Claims;

namespace MyShop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ICartService  cartService;
        private readonly IProductService productService;

        public HomeController(ICartService  cartService,IProductService productService)
        {
            this.cartService = cartService;
            this.productService = productService;
        }
        public IActionResult Index()
        {
            var productsForIndex=productService.GetAllProductForIndex();
            return View(productsForIndex);
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if(id==null || productService.GetProductById(id??0) is null) 
                return NotFound();
            var productForDetails = productService.GetProductDetails(id??0);

            return View(productForDetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ProductForDetails productForDetails)
        {
            if(!ModelState.IsValid)
                return View(productForDetails);
            // use service to add cart
            var claimIdentity = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (claimIdentity == null)
                return BadRequest();
            var UserId = claimIdentity.Value;

            var result=cartService.AddProductToCart(productForDetails, UserId);
            if (result > 0)
            {
            return RedirectToAction(nameof(Index));

            }

            return View(productForDetails);


        }
    }


}
