using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyShop.Entities.IRepositories;
using MyShop.Entities.Models;
using MyShop.Web.Constants;
using MyShop.Web.Services.Implementations;
using MyShop.Web.Services.Interfaces;
using MyShop.Web.Settings;
using MyShop.Web.ViewModels;

namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.AdminRole)]

    public class ProductsController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICategoryService categoryService;
        private readonly IProductService productService;

        public ProductsController(IUnitOfWork unitOfWork,ICategoryService categoryService,IProductService productService)
        {
            this.unitOfWork = unitOfWork;
            this.categoryService = categoryService;
            this.productService = productService;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }


        [HttpGet]
        public  IActionResult GetProducts()
        {
            var productTable = productService.GetProductForTable();
            return Json(new {data=productTable});
        }


        

        [HttpGet]
        public IActionResult Create()
        {
            var productVM = new CreateProductVM
            {
                Categories = categoryService.GetSelectListItems()
            };
            return View(productVM);
        }



        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(CreateProductVM productVM)
        {
            if (!ModelState.IsValid) return View(productVM);

            var effectedRows = await productService.CreateProductAsync(productVM);


            if (effectedRows > 0) TempData[ToastrKeys.Create] = "item has created successfully";

            return RedirectToAction(nameof(Index));

        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
                return NotFound();

            var product = await productService.GetProductById(id);
            if (product is null)
                return NotFound();

            // get data from service to send it to view
            var ProductForEdit = await  productService.GetDataForEdit(product);
            ProductForEdit.Categories = categoryService.GetSelectListItems();


            return View(ProductForEdit);
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(EditProductVM editProductVM)
        {
            if (!ModelState.IsValid) return View(editProductVM);
            var id=editProductVM.Id;
            var product = await productService.GetProductById(id);
            if (product is null)
                return NotFound();


            var result =  await productService.EditProduct(editProductVM);
            if (result)
            {
                TempData[ToastrKeys.Update] = "item has updated successfully";
            }


            return RedirectToAction(nameof(Index));

        }






        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
                return NotFound();

            var product = await productService.GetProductById(id);
            if (product is null)
                return NotFound();

            // service
            await productService.DeleteProduct(product);

            return RedirectToAction(nameof(Index));

        }

    }
}
