using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using MyShop.Web.Services.Implementations;
using MyShop.Web.Services.Interfaces;

namespace MyShop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IProductService productService;
        public HomeController(IProductService productService)
        {
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
    }


}
