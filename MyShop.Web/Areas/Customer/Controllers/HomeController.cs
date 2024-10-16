using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using MyShop.Web.Constants;
using MyShop.Web.Services.Implementations;
using MyShop.Web.Services.Interfaces;
using MyShop.Web.ViewModels;
using System.Security.Claims;
using X.PagedList;

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
        public IActionResult Index(int ?page)
        {

            int pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            int pageSize = 8;
            var productsForIndex=productService.GetAllProductForIndex().ToPagedList(pageNumber, pageSize);          
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
                //// add session card
                //var count=cartService.GetAllShoppingCarts(UserId).Count();
                //HttpContext.Session.SetInt32(SD.SessionKey, count);

                // add session card
                var count = (HttpContext.Session.GetInt32(SD.SessionKey) ?? 0)+ productForDetails.Count ;
                HttpContext.Session.SetInt32(SD.SessionKey, count);

                return RedirectToAction(nameof(Index));

            }

            return View(productForDetails);


        }
    }


}
