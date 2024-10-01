using MyShop.Entities.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyShop.Web.ViewModels
{
    public class ProductForIndexVM
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }

        public string? ImgSrc { get; set; }

    }
}
