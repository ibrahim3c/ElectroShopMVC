using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using MyShop.Web.Constants;
using MyShop.Web.Services.Interfaces;
using MyShop.Web.Settings;
using MyShop.Web.ViewModels;

namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]

    [Authorize(Roles = Roles.AdminRole)]
    public class OrdersController:Controller
    {
        private readonly IOrderService orderService;
		private readonly ICartService cartService;

		public OrdersController(IOrderService orderService,ICartService cartService)
        {
            this.orderService = orderService;
			this.cartService = cartService;
		}
        public IActionResult Index()
        {
            
            return View();
        }

     

        public IActionResult GetOrders()
        
        {
            var orders = orderService.GetAllOrderVMs();
            return Json(new {data=orderService.GetAllOrderVMs()}); 
        }
        public IActionResult Details(int id) {

            var orderDetails=orderService.GetOrderDetails(id);
            return View(orderDetails);
        }

		public IActionResult UpdateOrderDetails(OrderDetailsVM orderDetails)
        {
            if(ModelState.IsValid) 
                return View(nameof(Details), orderDetails);
            var orderHeader = cartService.GetOrderHeader(orderDetails.OrderHeader.Id);

            orderHeader.UserName=orderDetails.OrderHeader.UserName;
            orderHeader.Address=orderDetails.OrderHeader.Address;
            if(orderHeader.Email is not null && orderDetails.OrderHeader.Email != orderHeader.Email)
                orderHeader.Email=orderDetails.OrderHeader.Email;

			if (orderDetails.OrderHeader.PhoneNumber is not null && orderDetails.OrderHeader.PhoneNumber!=orderHeader.PhoneNumber)
				orderHeader.PhoneNumber = orderDetails.OrderHeader.PhoneNumber;

			if (orderDetails.OrderHeader.City is not null && orderDetails.OrderHeader.City != orderHeader.City)
				orderHeader.City = orderDetails.OrderHeader.City;

			if (orderDetails.OrderHeader.Carrier is not null && orderDetails.OrderHeader.Carrier != orderHeader.Carrier)
				orderHeader.Carrier = orderDetails.OrderHeader.Carrier;

			if (orderDetails.OrderHeader.TrackingNumber is not null && orderDetails.OrderHeader.TrackingNumber != orderHeader.TrackingNumber)
				orderHeader.TrackingNumber = orderDetails.OrderHeader.TrackingNumber;


            //cartService.UpdateOrderHeader(orderHeader);
            cartService.saveChages();
			TempData[ToastrKeys.Update] = "item has updated successfully";
			return RedirectToAction(nameof(Details), new { id = orderDetails.OrderHeader.Id });
        }
	}

}
