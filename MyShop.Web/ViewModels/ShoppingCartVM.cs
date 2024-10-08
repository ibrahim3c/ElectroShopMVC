using MyShop.Entities.Models;

namespace MyShop.Web.ViewModels
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCart> shoppingCarts { get; set; }
        public decimal TotalCarts {  get; set; }
        public OrderHeader OrderHeader { get; set; }
    }
}
