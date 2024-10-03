using MyShop.Entities.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyShop.Web.ViewModels
{
    public class ProductForDetails
    {
        [Required]
        public int Id { get; set; }
        //[Required]
        [MaxLength(100)]
        public string? Name { get; set; }
        [MaxLength(2500)]
        public string? Description { get; set; }
        //[Required]
        public decimal? Price { get; set; }

        [Range(1, 100,ErrorMessage ="you must enter value between 1 to 100")]
        public int Count {  get; set; }
        public string? ImgSrc { get; set; }

        public string? CategoryName {  get; set; }
    }
}
