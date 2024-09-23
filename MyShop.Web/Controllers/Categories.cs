using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MyShop.Web.Data;
using MyShop.Web.Models;
using MyShop.Web.Settings;

namespace MyShop.Web.Controllers
{
 
    public class Categories : Controller
    {
        private readonly AppDbContext appDbContext;

        public Categories( AppDbContext appDbContext )
        {
            this.appDbContext = appDbContext;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await appDbContext.Categories.ToListAsync();   
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
            
           await appDbContext.Set<Category>().AddAsync(category);
            appDbContext.SaveChanges();

            TempData[ToastrKeys.Create] = "item has created successfully";

            return RedirectToAction(nameof(Index));

        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            var cat = await appDbContext.Categories.FindAsync(id);
            if(id == null || cat is null) 
                return NotFound();


            return View(cat);
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(int id,Category category)
        {
            if (!ModelState.IsValid) return View(category);

            var cat = await appDbContext.Categories.FindAsync(id);

            cat.Name = category.Name;
            cat.Description = category.Description;

            if (id == null || cat is null)
                 appDbContext.Set<Category>().Update(cat);
                var effectedRows= appDbContext.SaveChanges();
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
            if(id == null)
                return NotFound();
            
            var cat = await appDbContext.Categories.FindAsync(id);
            if ( cat is null)
                return NotFound();

                appDbContext.Set<Category>().Remove(cat);
              appDbContext.SaveChanges();

            return RedirectToAction(nameof(Index));

        }



    }
}
