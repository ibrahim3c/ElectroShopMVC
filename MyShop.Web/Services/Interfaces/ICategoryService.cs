using Microsoft.AspNetCore.Mvc.Rendering;

namespace MyShop.Web.Services.Interfaces
{
    public interface ICategoryService
    {
        IEnumerable<SelectListItem> GetSelectListItems();
    }
}
