using System.ComponentModel.DataAnnotations;

namespace MyShop.Web.Models
{
    public class Category
    {
        public int Id { get; set; } = default!;
        [Required]
        public string Name { get; set; }=default!;
        public string Description { get; set; } = default!;

        [Display(Name ="Created Date")]
        public DateTime CreatedTime { get; set; } = DateTime.Now;
    }
}
