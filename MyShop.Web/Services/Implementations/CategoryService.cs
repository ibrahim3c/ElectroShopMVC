using Microsoft.AspNetCore.Mvc.Rendering;
using MyShop.Entities.IRepositories;
using MyShop.Web.Services.Interfaces;

namespace MyShop.Web.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<SelectListItem> GetSelectListItems()
        {
            return unitOfWork.Categories.GetAll().Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString(),
            }).OrderBy(c=>c.Text).ToList();
        } 
    }
}
