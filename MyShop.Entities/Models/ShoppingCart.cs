using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyShop.Entities.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Product))]
        public int ProductID {  get; set; }

        [Range(1,100,ErrorMessage ="you must enter value between 1 to 100")]
        public int Count { get; set; }
        public Product Product { get; set; }

        [ForeignKey(nameof(User))]
        public string UserID {  get; set; }
        public AppUser User { get; set; }
    }
}
