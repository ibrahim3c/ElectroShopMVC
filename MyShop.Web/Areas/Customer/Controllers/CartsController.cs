using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyShop.Entities.Models;
using MyShop.Web.Constants;
using MyShop.Web.Services.Interfaces;
using MyShop.Web.ViewModels;
using Stripe.Checkout;
using System.Security.Claims;

namespace MyShop.Web.Areas.Customer.Controllers
{
	[Authorize]
	[Area("Customer")]
	public class CartsController : Controller
	{
		private readonly ICartService cartService;

		public CartsController(ICartService cartService)
		{
			this.cartService = cartService;
		}

		public IActionResult Index()
		{
			var UserID = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
			if (UserID == null) return View();
			var ShoppingCarts = cartService.GetAllShoppingCartsOfUser(UserID);


			//TODO:never forget to do view of EmptyShoppingCart
			if (ShoppingCarts == null || ShoppingCarts.shoppingCarts == null || !ShoppingCarts.shoppingCarts.Any()) return View("EmptyShoppingCart");
			return View(ShoppingCarts);
		}

		public IActionResult Plus(int Id)
		{
			var result = cartService.Plus(Id, 1);
			if (result > 0)
				return RedirectToAction(nameof(Index));
			return BadRequest();
		}


		public IActionResult Minus(int Id)
		{
			var result = cartService.Minus(Id, 1);

			if (result > 0)
				return RedirectToAction(nameof(Index));

			if (result == 1000)
				return RedirectToAction("Index", "Home");
			return BadRequest();


			//return RedirectToAction(nameof(Index));
		}

		public IActionResult Remove(int CartID)
		{
			var result=cartService.RemoveShoppingCart(CartID);
			if (result > 0)
				return RedirectToAction(nameof(Index));

			return BadRequest();
		}

		[HttpGet]
		public IActionResult Summary()
		{

            var UserID = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
			if (UserID == null) NotFound();
            var ShoppingCartVM = cartService.GetAllShoppingCartsOfUser(UserID);

		

            //TODO:never forget to do view of EmptyShoppingCart
            if (ShoppingCartVM == null || ShoppingCartVM.shoppingCarts==null ||!ShoppingCartVM.shoppingCarts.Any()) return View("EmptyShoppingCart");
            return View(ShoppingCartVM);
		}

		[HttpPost]
		public IActionResult Summary(ShoppingCartVM shoppingCartVM)
		{
            var UserID = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if (UserID == null) NotFound();
			//shoppingCartVM = cartService.GetAllShoppingCartsOfUser(UserID);
			shoppingCartVM.shoppingCarts = cartService.GetAllShoppingCarts(UserID);
			decimal totalCarts = 0;
            foreach (var cart in shoppingCartVM.shoppingCarts)
            {
                totalCarts += cart.Count * cart.Product.Price;
            }


            shoppingCartVM.OrderHeader.OrderStatus = SD.Pending;
			shoppingCartVM.OrderHeader.PaymentStatus = SD.Pending;
			shoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
			shoppingCartVM.OrderHeader.UserId = UserID;
			shoppingCartVM.OrderHeader.TotalPrice = totalCarts;

			//// add order header

			cartService.AddOrderHeader(shoppingCartVM.OrderHeader);


			// add order details
			List<OrderDetails> ordersDetails = new List<OrderDetails>();

			foreach(var cart in shoppingCartVM.shoppingCarts)
			{
				OrderDetails orderDetails = new OrderDetails()
				{
					OrderHeaderId = shoppingCartVM.OrderHeader.Id,
					ProductId=cart.ProductID,
					Price=cart.Product.Price,
					Quantity=cart.Count
				};
				ordersDetails.Add(orderDetails);
			}

			cartService.AddOrdersDetails(ordersDetails);


			// stripe

			var domain = "http://localhost:5074";

            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = $"{domain}/Customer/Carts/OrderConfirmation?id={shoppingCartVM.OrderHeader.Id}",
                CancelUrl = $"{domain}/Customer/Carts/Index"
            };

			var sessionLineItemOptions=new List<SessionLineItemOptions>();
			foreach(var cart in shoppingCartVM.shoppingCarts)
			{
				var sessionLineItem= new SessionLineItemOptions
				{
					PriceData = new SessionLineItemPriceDataOptions
					{
						UnitAmount = (long)(cart.Product.Price * 100),
						Currency = "usd",
						ProductData = new SessionLineItemPriceDataProductDataOptions
						{
							Name = cart.Product.Name,
						},
					},
					Quantity = cart.Count,
				};

				sessionLineItemOptions.Add(sessionLineItem);

            }
			options.LineItems.AddRange(sessionLineItemOptions);

            var service = new SessionService();
            Session session = service.Create(options);

			shoppingCartVM.OrderHeader.SessionId = session.Id;
			//shoppingCartVM.OrderHeader.PaymentIntentId = session.PaymentIntentId;

			cartService.saveChages();

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);



            


           
		}

		public IActionResult OrderConfirmation(int id)
		{
			var orderHeader=cartService.GetOrderHeader(id);
            var service = new SessionService();
            Session session = service.Get(orderHeader.SessionId);


			if (session.PaymentStatus.ToLower() == "paid")
			{
				cartService.ChangeStatusOfOrderHeader(id, SD.Approved, SD.Approved);
				orderHeader.PaymentIntentId = session.PaymentIntentId;
				//cartService.UpdateOrderHeader(orderHeader);
				cartService.saveChages();


			}

			//remove shoppng carts that already order it

			cartService.RemoveShoppingCartsOfOrderHeader(orderHeader.Id);
            return View(id);


        }



    }
}
