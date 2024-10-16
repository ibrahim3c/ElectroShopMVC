using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using MyShop.Entities.IRepositories;
using MyShop.Entities.Models;
using MyShop.Services.Interfaces;
using MyShop.Web.Constants;
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

        public IEnumerable<ShoppingCart>GetAllShoppingCarts(string userID)
        {
            return  unitOfWork.ShoppingCarts.FindAll(s => s.UserID == userID, new string[] { "Product" });
           
        }

        public bool UpdateOrderHeader(OrderHeader orderHeader)
        {
           unitOfWork.OrderHeaders.Update(orderHeader);
            return unitOfWork.Complete() > 0;
        }

        public bool RemoveShoppingCarts(IEnumerable<ShoppingCart> shoppingCarts)
        {
            unitOfWork.ShoppingCarts.DeleteRange(shoppingCarts);
           var result= unitOfWork.Complete();
            return result > 0;
        }

        public bool RemoveShoppingCartsOfOrderHeader( int orderHeaderID)
        {
            var orderHeader = GetOrderHeader(orderHeaderID);
            var shoppingCarts = unitOfWork.ShoppingCarts.FindAll(u => u.UserID == orderHeader.UserId);

           return RemoveShoppingCarts(shoppingCarts);

        }

        public bool AddOrderHeader(OrderHeader orderHeader)
        {
            unitOfWork.OrderHeaders.Add(orderHeader);
            var result = unitOfWork.Complete();
            if (result  >0)
                return true;

            else return false;
        }

        public bool AddOrdersDetails(List<OrderDetails> orderDetails)
        {
            unitOfWork.OrderDetails.AddRange(orderDetails);
            var result = unitOfWork.Complete();
            if (result > 0)
                return true;

            else return false;
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
            var shoppingCarts = unitOfWork.ShoppingCarts.FindAll(s => s.UserID == UserID, new string[] { "Product"});
            var user = unitOfWork.userRepository.Find(u => u.Id == UserID);
            decimal totalCarts = 0;
            foreach (var cart in shoppingCarts) { 
            totalCarts+= cart.Count*cart.Product.Price;
            }



            var orderHeader = new OrderHeader
            {
                UserId = UserID,
                //User=user,
                TotalPrice=totalCarts,
                UserName=user.FirstName??user.LastName,
                Email=user.Email,
                Address=user.Address,
                PhoneNumber=user.PhoneNumber
            };

            var shoppinCartsVM= new ShoppingCartVM
            {
                shoppingCarts = shoppingCarts
            ,
                TotalCarts = totalCarts,
                OrderHeader=orderHeader
            };


            return shoppinCartsVM;
        }

        public ShoppingCart GetShoppingCart(int cartID)
        {
            return unitOfWork.ShoppingCarts.GetById(cartID);
        }
        public int RemoveShoppingCartAndReturnCount(int cartID)
        {
            var cart = unitOfWork.ShoppingCarts.GetById(cartID);
            if (cart == null) return 0;
            var count = cart.Count;
            unitOfWork.ShoppingCarts.Delete(cart);

          
             unitOfWork.Complete();
            return count;

        }
        public int RemoveShoppingCart(int cartID)
		{
			var cart= unitOfWork.ShoppingCarts.GetById(cartID);
            if (cart == null) return 0;
            unitOfWork.ShoppingCarts.Delete(cart);

      
            return unitOfWork.Complete();

		}

        public int saveChages()
        {
            return unitOfWork.Complete();
        }


        public void ChangeStatusOfOrderHeader(int id,string st1,string st2)
        {
            unitOfWork.OrderHeaders.ChangeStatus(id, st1, st2);
            unitOfWork.Complete();
        }
        public OrderHeader GetOrderHeader(int id)
        {
            return unitOfWork.OrderHeaders.GetById(id);
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

        public int GetCountOfShoppinCartsOfUser(string UserID)
        {
            var shoppingCarts = unitOfWork.ShoppingCarts.FindAll(s => s.UserID == UserID, new string[] { "Product" });
            var user = unitOfWork.userRepository.Find(u => u.Id == UserID);
            int totalCarts = 0;
            foreach (var cart in shoppingCarts)
            {
                totalCarts += cart.Count;
            }
            return totalCarts;

        }
    }
}
