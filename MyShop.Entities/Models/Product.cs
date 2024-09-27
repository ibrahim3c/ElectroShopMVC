using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Entities.Models
{
    public class Product
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(2500)]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }

        public string? ImgSrc {  get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryID { get; set; }

        public Category Category { get; set; }
    }
}
