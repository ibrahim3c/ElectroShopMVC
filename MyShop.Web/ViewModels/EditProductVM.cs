using Microsoft.AspNetCore.Mvc.Rendering;
using MyShop.Web.Attributes;
using MyShop.Web.Settings;
using System.ComponentModel.DataAnnotations;

namespace MyShop.Web.ViewModels
{
    public class EditProductVM
    {
        [Required]
         public int Id { get; set; }
        [MaxLength(250)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(2500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        [AllowedExtenstion(FileSettings.AllowedExtensions),
        MaxFileSize(FileSettings.MaxFileSizeInMB)]
        [Display(Name = "Image")]
        public IFormFile ProductImage { get; set; } = default!;
        public string? ImgSrc { get; set; }

        [Display(Name = "Category")]
        [Required]
        public int CatID { get; set; } = default!;
        public IEnumerable<SelectListItem>? Categories { get; set; } = Enumerable.Empty<SelectListItem>();

    }

}
