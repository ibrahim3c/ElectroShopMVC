using MyShop.Entities.IRepositories;
using MyShop.Web.Services.Interfaces;
using MyShop.Web.ViewModels;

namespace MyShop.Web.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<OrderVM> GetAllOrderVMs()
        {
            return unitOfWork.OrderHeaders.GetAll().Select(o => new OrderVM
            {
                Email = o.Email,
                Name = o.UserName,
                OrderStatus = o.OrderStatus,
                OrderID = o.Id,
                PhoneNumber = o.PhoneNumber,
                OrderTotal = o.TotalPrice

            }).ToList();


        }
        public OrderDetailsVM GetOrderDetails(int id)
        {
            return new OrderDetailsVM
            {
                OrderDetails = unitOfWork.OrderDetails.FindAll(o => o.OrderHeaderId == id, new string[] { "Product" }),
                OrderHeader= unitOfWork.OrderHeaders.Find(o => o.Id == id, new string[] { "User" })
			};

        }
    }
}