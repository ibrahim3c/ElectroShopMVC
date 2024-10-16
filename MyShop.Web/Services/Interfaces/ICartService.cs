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
        public bool UpdateOrderHeader(OrderHeader orderHeader);

		public IEnumerable<ShoppingCart> GetAllShoppingCarts(string userID);
        public bool AddOrderHeader(OrderHeader orderHeader);
        public bool AddOrdersDetails(List<OrderDetails> orderDetails);
        public bool RemoveShoppingCarts(IEnumerable<ShoppingCart> shoppingCarts);
        public OrderHeader GetOrderHeader(int id);
        public void ChangeStatusOfOrderHeader(int id, string st1, string st2);
        public bool RemoveShoppingCartsOfOrderHeader(int orderHeaderID);
        public int RemoveShoppingCartAndReturnCount(int cartID);
        public int GetCountOfShoppinCartsOfUser(string UserID);
        public int saveChages();


    }
}
