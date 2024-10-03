using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.Web.Services.Interfaces;
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
			if (ShoppingCarts == null) return View("EmptyShoppingCart");
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
	}
}
