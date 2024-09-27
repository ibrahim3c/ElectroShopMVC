using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using MyShop.DAL.Data;
using MyShop.Entities.IRepositories;
using MyShop.Entities.Models;
using MyShop.Web.Settings;
using System;
namespace MyShop.Web.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public CategoriesController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await unitOfWork.Categories.GetAllAsync();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid) return View(category);

            await unitOfWork.Categories.AddAsync(category);
            var effectedRows = unitOfWork.Complete();

            if (effectedRows > 0) TempData[ToastrKeys.Create] = "item has created successfully";

            return RedirectToAction(nameof(Index));

        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
                return NotFound();

            var cat = await unitOfWork.Categories.GetByIdAsync(id);
            if (cat is null)
                return NotFound();


            return View(cat);
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (!ModelState.IsValid) return View(category);

            var cat = await unitOfWork.Categories.GetByIdAsync(id);

            cat.Name = category.Name;
            cat.Description = category.Description;

            if (id == null || cat is null)
                unitOfWork.Categories.Update(cat);
            var effectedRows = unitOfWork.Complete();
            if (effectedRows > 0)
            {
                TempData[ToastrKeys.Update] = "item has updated successfully";
            }


            return RedirectToAction(nameof(Index));

        }


        //[HttpGet]
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //        return NotFound();

        //    var cat = await appDbContext.Categories.FindAsync(id);
        //    if (cat is null)
        //        return NotFound();


        //    return View(cat);
        //}



        [HttpGet]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (id == null)
                return NotFound();

            var cat = await unitOfWork.Categories.GetByIdAsync(id);
            if (cat is null)
                return NotFound();

            unitOfWork.Categories.Delete(cat);
            unitOfWork.Complete();

            return RedirectToAction(nameof(Index));

        }



    }
}
