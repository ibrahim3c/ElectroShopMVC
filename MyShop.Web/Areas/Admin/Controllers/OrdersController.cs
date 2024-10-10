using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using MyShop.Entities.Models;
using MyShop.Web.Constants;
using MyShop.Web.Services.Interfaces;
using MyShop.Web.Settings;
using MyShop.Web.ViewModels;
using Stripe;

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

        [HttpPost]
        [ValidateAntiForgeryToken]
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
			TempData[ToastrKeys.Update] = "The Item has updated successfully";
			return RedirectToAction(nameof(Details), new { id = orderDetails.OrderHeader.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
       public IActionResult StartProccess(OrderHeader orderHeader)
        {
            var id = orderHeader.Id;
            if (orderHeader == null)
                return NotFound();

            cartService.ChangeStatusOfOrderHeader(id, SD.Proccessing, null);

			TempData[ToastrKeys.Update] = "the Order Status has updated successfully";
			return RedirectToAction(nameof(Details), new { id =id });
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult StartShip(OrderHeader orderHeader)
		{
			var id = orderHeader.Id;
            var order=cartService.GetOrderHeader(id);
			if (orderHeader == null)
				return NotFound();

            order.TrackingNumber=orderHeader.TrackingNumber;
            order.Carrier=orderHeader.Carrier;
            order.OrderStatus = SD.Shipped;
            order.ShippingDate=DateTime.Now;

            cartService.saveChages();


			TempData[ToastrKeys.Update] = "The Order  has been Shipped successfully";
			return RedirectToAction(nameof(Details), new { id = id });
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult CancelOrder(OrderHeader orderHeader)
		{
			var id = orderHeader.Id;
			var order = cartService.GetOrderHeader(id);
			if (orderHeader == null)
				return NotFound();

            // my code 
            if (order.OrderStatus == SD.Cancelled)
            {
				TempData[ToastrKeys.Update] = "The order is already cancelled.";
				return RedirectToAction(nameof(Details), new { id = id });
			}
                 


            if (order.PaymentStatus == SD.Approved)
            {
                var options = new RefundCreateOptions()
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderHeader.PaymentIntentId
                };
                var service =new  RefundService();
                Refund refund=service.Create(options);
				cartService.ChangeStatusOfOrderHeader(id, SD.Cancelled, SD.Refund);


			}
            else
            {
				cartService.ChangeStatusOfOrderHeader(id, SD.Cancelled, SD.Cancelled);

			}

			cartService.saveChages();


			TempData[ToastrKeys.Update] = "the Order has been Cancelled successfully";
			return RedirectToAction(nameof(Details), new { id = id });
		}
	}

}
