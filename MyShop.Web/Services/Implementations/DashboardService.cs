using MyShop.Entities.IRepositories;
using MyShop.Web.Constants;
using MyShop.Web.Services.Interfaces;
using MyShop.Web.ViewModels;

namespace MyShop.Web.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork unitOfWork;

        public DashboardService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public DashboardVM GetDashboardData()
        {
            return new DashboardVM
            {
                UsersCount = unitOfWork.userRepository.Count(),
                ProductsCount = unitOfWork.Products.Count(),
                OrdersCount = unitOfWork.OrderHeaders.Count(),
                ApprovedOrders = unitOfWork.OrderHeaders.FindAll(o => o.OrderStatus == SD.Approved).Count(),
                CancelledOrders = unitOfWork.OrderHeaders.FindAll(o => o.OrderStatus == SD.Cancelled).Count(),
                PendingOrders = unitOfWork.OrderHeaders.FindAll(o => o.OrderStatus == SD.Pending).Count()
            };
        }
    }
}
