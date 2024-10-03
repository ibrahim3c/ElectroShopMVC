using MyShop.Entities.Models;
using MyShop.Web.ViewModels;

namespace MyShop.Web.Services.Interfaces
{
    public interface ICartService
    {
        public int AddProductToCart(ProductForDetails productForDetails, string UserId);
        public ShoppingCartVM GetAllShoppingCartsOfUser(string UserID);
        public int Plus(int cartID, int count);
        public int Minus(int cartID, int count);
        public ShoppingCart GetShoppingCart(int cartID);
        public int RemoveShoppingCart(int cartID);

	}
}
