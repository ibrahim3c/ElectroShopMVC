using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using MyShop.Entities.IRepositories;
using MyShop.Entities.Models;
using MyShop.Services.Interfaces;
using MyShop.Web.Services.Interfaces;
using MyShop.Web.ViewModels;
using System.Security.Claims;

namespace MyShop.Web.Services.Implementations
{
    public class CartService:ICartService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IFileService fileService;
        //private readonly CategoryService categoryService;

        public CartService(IUnitOfWork unitOfWork, IFileService fileService)
        {
            this.unitOfWork = unitOfWork;
            this.fileService = fileService;
        }

        public int AddProductToCart(ProductForDetails productForDetails,string UserId)
        {
            var shoppingCart=unitOfWork.ShoppingCarts.Find(s=>s.ProductID==productForDetails.Id && s.UserID==UserId);

            if (shoppingCart == null) {
                ShoppingCart cart = new ShoppingCart()
                {
                    Count = productForDetails.Count,
                    ProductID = productForDetails.Id,
                    UserID = UserId // Assign the user ID from claims
                };

                // Add the cart to the unit of work
                unitOfWork.ShoppingCarts.Add(cart);
            }
            else
            {

                shoppingCart.Count=unitOfWork.ShoppingCarts.IncreaseCount(shoppingCart,productForDetails.Count);
            }

            // Create a new ShoppingCart object
         
            var effectedRows = unitOfWork.Complete();
            return effectedRows;

        }
        public ShoppingCartVM GetAllShoppingCartsOfUser(string UserID)
        {
            var shoppingCarts = unitOfWork.ShoppingCarts.FindAll(s => s.UserID == UserID, new string[] { "Product" });
            decimal totalCarts = 0;
            foreach (var cart in shoppingCarts) { 
            totalCarts+= cart.Count*cart.Product.Price;
            }

            return new ShoppingCartVM
            {
                shoppingCarts = shoppingCarts
            ,
                TotalCarts = totalCarts
            };
        }

        public ShoppingCart GetShoppingCart(int cartID)
        {
            return unitOfWork.ShoppingCarts.GetById(cartID);
        }
		public int RemoveShoppingCart(int cartID)
		{
			var cart= unitOfWork.ShoppingCarts.GetById(cartID);
            if (cart == null) return 0;
            unitOfWork.ShoppingCarts.Delete(cart);
            return unitOfWork.Complete();

		}

        public int Plus(int cartID,int count)
        {
            var shoppingCart = unitOfWork.ShoppingCarts.GetById(cartID);
            if (shoppingCart == null) return 0;
          shoppingCart.Count=  unitOfWork.ShoppingCarts.IncreaseCount(shoppingCart, count);
             return unitOfWork.Complete();
        }


		public int Minus(int cartID, int count)
		{
			var shoppingCart = unitOfWork.ShoppingCarts.GetById(cartID);
			if (shoppingCart == null) return 0;
            if (shoppingCart.Count <= 1) { 
              RemoveShoppingCart(cartID);
                return 1000;
            }

			shoppingCart.Count=unitOfWork.ShoppingCarts.DecreaseCount(shoppingCart, count);
			return unitOfWork.Complete();
		}
	}
}
