using System.ComponentModel.DataAnnotations;

namespace MyShop.Entities.Models
{
    public class Category
    {
        public int Id { get; set; } = default!;
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }=default!;
        [MaxLength(2500)]
        public string Description { get; set; } = default!;

        [Display(Name ="Created Date")]
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public List<Category> Categories { get; set;} = new List<Category>();
    }
}
